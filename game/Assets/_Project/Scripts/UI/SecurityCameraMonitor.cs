using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using BusShift.Bus;
using BusShift.Core;

namespace BusShift.UI
{
    /// <summary>
    /// FNAF-style security-camera monitor for the bus cab dashboard.
    ///
    /// <para>
    /// Supports up to four <see cref="Camera"/> feeds, each rendered into a
    /// dedicated <see cref="RenderTexture"/> and displayed on a single
    /// <see cref="RawImage"/> dashboard element.  The player can:
    /// <list type="bullet">
    ///   <item>Toggle the monitor on / off with <b>C</b>.</item>
    ///   <item>Cycle cameras with the <b>← →</b> arrow keys.</item>
    /// </list>
    /// </para>
    ///
    /// <para>
    /// A <i>static noise</i> overlay flickers at random intervals (0.2–0.5 s)
    /// with an alpha of 0.3–0.8, simulating analogue interference.
    /// When <see cref="SanitySystem.CurrentSanity"/> falls below 30 % the
    /// flicker rate doubles and maximum alpha rises to 0.95, increasing tension.
    /// </para>
    ///
    /// <para>
    /// Camera references are populated from the Inspector.  If a slot is empty
    /// the component auto-falls-back to <see cref="CameraSystem.RearCamera"/>
    /// via <see cref="FindAnyObjectByType{T}"/> (Unity 6 API).
    /// </para>
    /// </summary>
    /// <remarks>
    /// Requires the <b>Unity Input System</b> package.
    /// In Project Settings → Player → Active Input Handling, select
    /// <i>Both</i> or <i>Input System Package (New)</i>.
    /// </remarks>
    public class SecurityCameraMonitor : MonoBehaviour
    {
        // ─────────────────────────────────────────────────────────────────────
        //  Inspector
        // ─────────────────────────────────────────────────────────────────────

        [Header("Camera Sources (up to 4)")]
        [Tooltip("Security cameras to cycle through.  Leave empty to auto-grab CameraSystem.RearCamera.")]
        [SerializeField] private Camera[] securityCameras = new Camera[4];

        [Tooltip("One RenderTexture per camera slot.  Must match index order of securityCameras.")]
        [SerializeField] private RenderTexture[] cameraRenderTextures = new RenderTexture[4];

        [Header("Monitor UI")]
        [Tooltip("RawImage on the dashboard that displays the active camera feed.")]
        [SerializeField] private RawImage _monitorDisplay;

        [Tooltip("Root GameObject for the entire monitor UI.  Activated / deactivated on toggle.")]
        [SerializeField] private GameObject _monitorRoot;

        [Tooltip("RawImage overlay used for static noise effect (set alpha each frame).")]
        [SerializeField] private RawImage _staticOverlay;

        [Header("Static Noise Tuning")]
        [Tooltip("Minimum seconds between static flicker events (normal sanity).")]
        [SerializeField] [Range(0.05f, 1f)] private float _staticIntervalMin = 0.2f;

        [Tooltip("Maximum seconds between static flicker events (normal sanity).")]
        [SerializeField] [Range(0.05f, 2f)] private float _staticIntervalMax = 0.5f;

        [Tooltip("Minimum alpha of static overlay flash (normal sanity).")]
        [SerializeField] [Range(0f, 1f)] private float _staticAlphaMin = 0.3f;

        [Tooltip("Maximum alpha of static overlay flash (normal sanity).")]
        [SerializeField] [Range(0f, 1f)] private float _staticAlphaMax = 0.8f;

        [Tooltip("When sanity is below this threshold the static becomes more intense.")]
        [SerializeField] [Range(0f, 1f)] private float _lowSanityThreshold = 0.30f;

        [Tooltip("Duration (seconds) each static flash is visible before fading.")]
        [SerializeField] [Range(0.02f, 0.5f)] private float _staticFlashDuration = 0.08f;

        // ─────────────────────────────────────────────────────────────────────
        //  Public state
        // ─────────────────────────────────────────────────────────────────────

        /// <summary>True while the security monitor is powered on and visible.</summary>
        public bool IsMonitorActive => _isMonitorActive;

        // ─────────────────────────────────────────────────────────────────────
        //  Events
        // ─────────────────────────────────────────────────────────────────────

        /// <summary>
        /// Fired whenever the active camera changes.
        /// Argument is the new camera index (0-based).
        /// </summary>
        public static event Action<int> OnCameraChanged;

        /// <summary>
        /// Fired whenever the monitor is toggled.
        /// Argument is <c>true</c> when the monitor turns on, <c>false</c> when it turns off.
        /// </summary>
        public static event Action<bool> OnMonitorToggled;

        // ─────────────────────────────────────────────────────────────────────
        //  Private
        // ─────────────────────────────────────────────────────────────────────

        private int        _activeCameraIndex  = 0;
        private bool       _isMonitorActive    = false;
        private float      _cachedSanity       = 0f;
        private Coroutine  _staticCoroutine;

        // ─────────────────────────────────────────────────────────────────────
        //  Unity lifecycle
        // ─────────────────────────────────────────────────────────────────────

        private void Awake()
        {
            AutoPopulateCamerasFromCameraSystem();

            // Monitor starts powered off
            if (_monitorRoot != null)
                _monitorRoot.SetActive(false);

            // Overlay invisible until a flash fires
            SetStaticAlpha(0f);
        }

        private void OnEnable()
        {
            SanitySystem.OnSanityChanged += HandleSanityChanged;
        }

        private void OnDisable()
        {
            SanitySystem.OnSanityChanged -= HandleSanityChanged;

            if (_staticCoroutine != null)
            {
                StopCoroutine(_staticCoroutine);
                _staticCoroutine = null;
            }
        }

        private void Update()
        {
            var kb = Keyboard.current;
            if (kb == null) return;

            // C — toggle monitor on / off
            if (kb.cKey.wasPressedThisFrame)
                ToggleMonitor();

            // Arrow keys — cycle camera (only while monitor is on)
            if (_isMonitorActive)
            {
                if (kb.leftArrowKey.wasPressedThisFrame)
                    CycleCamera(-1);
                else if (kb.rightArrowKey.wasPressedThisFrame)
                    CycleCamera(1);
            }
        }

        // ─────────────────────────────────────────────────────────────────────
        //  Public API
        // ─────────────────────────────────────────────────────────────────────

        /// <summary>
        /// Cycles the active camera by <paramref name="direction"/> steps.
        /// </summary>
        /// <param name="direction">+1 = next camera, -1 = previous camera.</param>
        public void CycleCamera(int direction)
        {
            int count = CountActiveCameras();
            if (count == 0) return;

            _activeCameraIndex = ((_activeCameraIndex + direction) % count + count) % count;
            ApplyActiveCamera();

            OnCameraChanged?.Invoke(_activeCameraIndex);

#if UNITY_EDITOR || DEVELOPMENT_BUILD
            Debug.Log($"[SecurityCameraMonitor] Camera → {_activeCameraIndex}.");
#endif
        }

        // ─────────────────────────────────────────────────────────────────────
        //  Monitor toggle
        // ─────────────────────────────────────────────────────────────────────

        /// <summary>
        /// Toggles the monitor on or off and starts / stops the static coroutine.
        /// </summary>
        private void ToggleMonitor()
        {
            _isMonitorActive = !_isMonitorActive;

            if (_monitorRoot != null)
                _monitorRoot.SetActive(_isMonitorActive);

            if (_isMonitorActive)
            {
                ApplyActiveCamera();
                RestartStaticCoroutine();
            }
            else
            {
                StopStaticCoroutine();
                SetStaticAlpha(0f);
            }

            OnMonitorToggled?.Invoke(_isMonitorActive);

#if UNITY_EDITOR || DEVELOPMENT_BUILD
            Debug.Log($"[SecurityCameraMonitor] Monitor {(_isMonitorActive ? "ON" : "OFF")}.");
#endif
        }

        // ─────────────────────────────────────────────────────────────────────
        //  Camera rendering
        // ─────────────────────────────────────────────────────────────────────

        /// <summary>
        /// Points <see cref="_monitorDisplay"/> at the <see cref="RenderTexture"/>
        /// for the currently selected camera index.
        /// </summary>
        private void ApplyActiveCamera()
        {
            if (_monitorDisplay == null) return;

            if (cameraRenderTextures != null
                && _activeCameraIndex < cameraRenderTextures.Length
                && cameraRenderTextures[_activeCameraIndex] != null)
            {
                _monitorDisplay.texture = cameraRenderTextures[_activeCameraIndex];
            }
        }

        // ─────────────────────────────────────────────────────────────────────
        //  Static noise coroutine
        // ─────────────────────────────────────────────────────────────────────

        /// <summary>
        /// Continuously flickers the static overlay at random intervals.
        /// Frequency and intensity increase when sanity is below
        /// <see cref="_lowSanityThreshold"/>.
        /// </summary>
        private IEnumerator StaticCoroutine()
        {
            while (_isMonitorActive)
            {
                bool isLowSanity = _cachedSanity < _lowSanityThreshold;

                // Wait a random interval before the next flash
                float interval = isLowSanity
                    ? UnityEngine.Random.Range(_staticIntervalMin * 0.5f,
                                               _staticIntervalMax * 0.5f)   // twice as frequent
                    : UnityEngine.Random.Range(_staticIntervalMin,
                                               _staticIntervalMax);

                yield return new WaitForSeconds(interval);

                if (!_isMonitorActive) yield break;

                // Determine flash alpha — capped higher when sanity is low
                float maxAlpha = isLowSanity
                    ? Mathf.Clamp(_staticAlphaMax * 1.2f, _staticAlphaMin, 0.95f)
                    : _staticAlphaMax;

                float alpha = UnityEngine.Random.Range(_staticAlphaMin, maxAlpha);

                // Flash on
                SetStaticAlpha(alpha);
                yield return new WaitForSeconds(_staticFlashDuration);

                // Flash off
                SetStaticAlpha(0f);
            }
        }

        // ─────────────────────────────────────────────────────────────────────
        //  Helpers
        // ─────────────────────────────────────────────────────────────────────

        /// <summary>Sets the alpha of the static overlay image.</summary>
        private void SetStaticAlpha(float alpha)
        {
            if (_staticOverlay == null) return;
            Color c = _staticOverlay.color;
            c.a = Mathf.Clamp01(alpha);
            _staticOverlay.color = c;
        }

        /// <summary>Stops and immediately restarts the static coroutine.</summary>
        private void RestartStaticCoroutine()
        {
            StopStaticCoroutine();
            _staticCoroutine = StartCoroutine(StaticCoroutine());
        }

        /// <summary>Stops the static coroutine if it is running.</summary>
        private void StopStaticCoroutine()
        {
            if (_staticCoroutine != null)
            {
                StopCoroutine(_staticCoroutine);
                _staticCoroutine = null;
            }
        }

        /// <summary>
        /// Counts how many camera slots in <see cref="securityCameras"/> are non-null.
        /// Returns at least 1 to prevent modulo-by-zero.
        /// </summary>
        private int CountActiveCameras()
        {
            int n = 0;
            if (securityCameras == null) return 1;
            foreach (var cam in securityCameras)
                if (cam != null) n++;
            return Mathf.Max(n, 1);
        }

        /// <summary>
        /// If the first camera slot is empty, tries to borrow
        /// <see cref="CameraSystem.RearCamera"/> as a fallback.
        /// </summary>
        private void AutoPopulateCamerasFromCameraSystem()
        {
            if (securityCameras != null
                && securityCameras.Length > 0
                && securityCameras[0] != null)
                return; // already assigned in the Inspector

            var camSys = FindAnyObjectByType<CameraSystem>();
            if (camSys == null) return;

            if (securityCameras == null || securityCameras.Length == 0)
                securityCameras = new Camera[1];

            if (securityCameras[0] == null && camSys.RearCamera != null)
            {
                securityCameras[0] = camSys.RearCamera;
#if UNITY_EDITOR || DEVELOPMENT_BUILD
                Debug.Log("[SecurityCameraMonitor] Auto-assigned CameraSystem.RearCamera to slot 0.");
#endif
            }
        }

        // ─────────────────────────────────────────────────────────────────────
        //  Event handlers
        // ─────────────────────────────────────────────────────────────────────

        /// <summary>Caches the latest sanity value for use inside the static coroutine.</summary>
        private void HandleSanityChanged(float sanity)
        {
            _cachedSanity = sanity;
        }

        // ─────────────────────────────────────────────────────────────────────
        //  Editor helpers
        // ─────────────────────────────────────────────────────────────────────

#if UNITY_EDITOR
        private void OnValidate()
        {
            _staticIntervalMin   = Mathf.Max(0.05f, _staticIntervalMin);
            _staticIntervalMax   = Mathf.Max(_staticIntervalMin, _staticIntervalMax);
            _staticAlphaMin      = Mathf.Clamp01(_staticAlphaMin);
            _staticAlphaMax      = Mathf.Clamp01(_staticAlphaMax);
            _lowSanityThreshold  = Mathf.Clamp01(_lowSanityThreshold);
            _staticFlashDuration = Mathf.Max(0.02f, _staticFlashDuration);
        }
#endif
    }
}
