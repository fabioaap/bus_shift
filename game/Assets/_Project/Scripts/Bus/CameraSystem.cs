using System;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace BusShift.Bus
{
    /// <summary>
    /// Manages all cameras in the bus cab:
    /// <list type="bullet">
    ///   <item><b>Driver camera</b> – primary first-person view.</item>
    ///   <item>
    ///     <b>Rearview mirror</b> (hold <b>E</b>) – enables the rear camera,
    ///     optionally blurs the driver view via a <see cref="Material"/> swap,
    ///     and fires <see cref="OnMirrorFocused"/>.
    ///   </item>
    ///   <item>
    ///     <b>Security / CCTV monitor</b> (tap <b>C</b>) – shows a small rear
    ///     camera feed on a dashboard UI element.  The feed is covered by static
    ///     noise by default; tapping C clears the static for
    ///     <see cref="StaticClearDuration"/> seconds, then a cooldown of
    ///     <see cref="StaticClearCooldown"/> seconds prevents immediate re-use.
    ///   </item>
    /// </list>
    /// </summary>
    /// <remarks>
    /// <para>
    /// All cameras are plain Unity <see cref="Camera"/> components — no URP
    /// Camera Data is assumed, so the script compiles in any URP or Built-in
    /// project without extra package references.
    /// </para>
    /// <para>
    /// DOF / blur for the mirror view is achieved by swapping the driver
    /// camera's background or by toggling a full-screen blur <see cref="Canvas"/>
    /// overlay.  Hook <see cref="OnMirrorFocused"/> to drive a Post-Processing
    /// Volume parameter from a separate component if a more sophisticated
    /// effect is needed.
    /// </para>
    /// </remarks>
    public class CameraSystem : MonoBehaviour
    {
        // ─────────────────────────────────────────────────────────────────────
        //  Inspector
        // ─────────────────────────────────────────────────────────────────────

        [Header("Cameras")]
        [Tooltip("The main driver (first-person) camera.")]
        public Camera DriverCamera;

        [Tooltip("Rear-facing camera used for both the mirror and the CCTV monitor.")]
        public Camera RearCamera;

        // ── Mirror ───────────────────────────────────────────────────────────

        [Header("Mirror (hold E)")]
        [Tooltip("Key to hold for the rearview-mirror focus.")]
        public KeyCode MirrorKey = KeyCode.E;

        [Tooltip("(Optional) Canvas/GameObject shown as a blur overlay on the driver view while the mirror is active.")]
        public GameObject BlurOverlay;

        // ── Security camera ───────────────────────────────────────────────────

        [Header("Security Camera (tap C)")]
        [Tooltip("Key tapped to clear the CCTV static.")]
        public KeyCode SecurityCameraKey = KeyCode.C;

        [Tooltip("Seconds the CCTV feed stays clear after tapping the key.")]
        public float StaticClearDuration = 5f;

        [Tooltip("Cooldown seconds before the player can clear static again.")]
        public float StaticClearCooldown = 15f;

        [Header("CCTV Monitor UI")]
        [Tooltip("RawImage on the dashboard that displays the rear camera's RenderTexture.")]
        public RawImage MonitorDisplay;

        [Tooltip("Image used as the static / noise overlay on top of the monitor feed.")]
        public Image StaticOverlay;

        [Tooltip("Animated static texture frames cycled to produce a noise effect.")]
        public Texture2D[] StaticFrames;

        [Tooltip("Frames per second at which static frames are cycled.")]
        public float StaticAnimFPS = 12f;

        // ─────────────────────────────────────────────────────────────────────
        //  Public state
        // ─────────────────────────────────────────────────────────────────────

        /// <summary>True while the player is holding the mirror key.</summary>
        public bool IsMirrorFocused { get; private set; }

        /// <summary>True while the CCTV feed is free of static (within the clear window).</summary>
        public bool IsCameraClean { get; private set; }

        /// <summary>
        /// Remaining cooldown seconds before the player can clear the CCTV static again.
        /// Zero when no cooldown is active.
        /// </summary>
        public float CameraCooldownRemaining { get; private set; }

        // ─────────────────────────────────────────────────────────────────────
        //  Events
        // ─────────────────────────────────────────────────────────────────────

        /// <summary>
        /// Fired when mirror focus changes.
        /// <c>true</c> = mirror is now active; <c>false</c> = mirror released.
        /// </summary>
        public static event Action<bool> OnMirrorFocused;

        /// <summary>Fired each time the player successfully clears the CCTV static.</summary>
        public static event Action OnCameraCleared;

        // ─────────────────────────────────────────────────────────────────────
        //  Private
        // ─────────────────────────────────────────────────────────────────────

        /// <summary>Countdown while the static is cleared.</summary>
        private float _staticClearTimer;

        /// <summary>True when the cooldown is actively counting down.</summary>
        private bool _cooldownActive;

        /// <summary>Index into <see cref="StaticFrames"/> for the animated noise.</summary>
        private int _staticFrameIndex;

        /// <summary>Accumulator for static animation timing.</summary>
        private float _staticFrameTimer;

        // ─────────────────────────────────────────────────────────────────────
        //  Unity lifecycle
        // ─────────────────────────────────────────────────────────────────────

        private void Awake()
        {
            // Rear camera renders continuously to a RenderTexture (set in Inspector
            // or at runtime here).  We keep it enabled at a low priority so the
            // monitor always has a live feed.
            if (RearCamera != null)
            {
                RearCamera.enabled = true;
                RearCamera.depth   = -2;   // renders before driver camera
            }

            // Driver camera takes priority
            if (DriverCamera != null)
                DriverCamera.depth = 0;

            // Start with static visible (camera is dirty)
            SetStaticVisible(true);
        }

        private void Update()
        {
            HandleMirrorInput();
            HandleSecurityCameraInput();

            TickStaticClearTimer();
            TickCooldownTimer();
            AnimateStaticFrames();
        }

        // ─────────────────────────────────────────────────────────────────────
        //  Mirror
        // ─────────────────────────────────────────────────────────────────────

        /// <summary>
        /// While E is held, activates mirror focus.
        /// On release, restores normal driver view.
        /// </summary>
        private void HandleMirrorInput()
        {
            if (Input.GetKeyDown(MirrorKey) && !IsMirrorFocused)
                SetMirrorFocus(true);

            if (Input.GetKeyUp(MirrorKey) && IsMirrorFocused)
                SetMirrorFocus(false);
        }

        /// <summary>
        /// Activates or deactivates the mirror focus.
        /// Toggles the blur overlay and fires <see cref="OnMirrorFocused"/>.
        /// </summary>
        /// <param name="focused"><c>true</c> to activate mirror view.</param>
        private void SetMirrorFocus(bool focused)
        {
            IsMirrorFocused = focused;

            // Show/hide the full-screen blur overlay on the driver view
            if (BlurOverlay != null)
                BlurOverlay.SetActive(focused);

            // The RearCamera is always rendering, but we can raise/lower its
            // display priority to bring it to the foreground if it shares a
            // render target or Camera Stack (URP).
            if (RearCamera != null)
            {
                // When focused: promote rear camera to render on top of the driver cam
                RearCamera.depth = focused ? 1 : -2;
            }

            OnMirrorFocused?.Invoke(focused);
        }

        // ─────────────────────────────────────────────────────────────────────
        //  Security camera
        // ─────────────────────────────────────────────────────────────────────

        /// <summary>
        /// Reads the security-camera clear key.
        /// Ignored while the cooldown is active.
        /// </summary>
        private void HandleSecurityCameraInput()
        {
            if (!Input.GetKeyDown(SecurityCameraKey)) return;
            if (_cooldownActive) return;

            ClearStatic();
        }

        /// <summary>
        /// Clears the CCTV static for <see cref="StaticClearDuration"/> seconds
        /// and fires <see cref="OnCameraCleared"/>.
        /// </summary>
        private void ClearStatic()
        {
            IsCameraClean      = true;
            _staticClearTimer  = StaticClearDuration;

            SetStaticVisible(false);

            OnCameraCleared?.Invoke();

#if UNITY_EDITOR || DEVELOPMENT_BUILD
            Debug.Log($"[CameraSystem] CCTV static cleared for {StaticClearDuration}s.");
#endif
        }

        // ─────────────────────────────────────────────────────────────────────
        //  Timers
        // ─────────────────────────────────────────────────────────────────────

        /// <summary>
        /// Counts down the static-clear window and re-introduces static when
        /// it expires, starting the cooldown.
        /// </summary>
        private void TickStaticClearTimer()
        {
            if (!IsCameraClean) return;

            _staticClearTimer -= Time.deltaTime;
            if (_staticClearTimer > 0f) return;

            // Clear window expired
            IsCameraClean = false;
            SetStaticVisible(true);

            // Begin cooldown
            _cooldownActive         = true;
            CameraCooldownRemaining = StaticClearCooldown;
        }

        /// <summary>
        /// Counts down the re-use cooldown after static was cleared.
        /// </summary>
        private void TickCooldownTimer()
        {
            if (!_cooldownActive) return;

            CameraCooldownRemaining -= Time.deltaTime;
            if (CameraCooldownRemaining > 0f) return;

            CameraCooldownRemaining = 0f;
            _cooldownActive         = false;
        }

        // ─────────────────────────────────────────────────────────────────────
        //  Static / noise animation
        // ─────────────────────────────────────────────────────────────────────

        /// <summary>
        /// Cycles through <see cref="StaticFrames"/> at <see cref="StaticAnimFPS"/>
        /// to produce animated noise on the monitor when static is visible.
        /// </summary>
        private void AnimateStaticFrames()
        {
            if (StaticOverlay == null) return;
            if (!StaticOverlay.gameObject.activeSelf) return;
            if (StaticFrames == null || StaticFrames.Length == 0) return;

            _staticFrameTimer += Time.deltaTime;
            float frameDuration = 1f / Mathf.Max(1f, StaticAnimFPS);

            if (_staticFrameTimer < frameDuration) return;

            _staticFrameTimer -= frameDuration;
            _staticFrameIndex  = (_staticFrameIndex + 1) % StaticFrames.Length;

            Texture2D frame = StaticFrames[_staticFrameIndex];
            if (frame != null)
                StaticOverlay.sprite = Sprite.Create(
                    frame,
                    new Rect(0, 0, frame.width, frame.height),
                    new Vector2(0.5f, 0.5f));
        }

        // ─────────────────────────────────────────────────────────────────────
        //  UI helpers
        // ─────────────────────────────────────────────────────────────────────

        /// <summary>
        /// Shows or hides the static noise overlay on the CCTV monitor.
        /// When hidden the live <see cref="MonitorDisplay"/> feed is fully visible.
        /// </summary>
        /// <param name="visible"><c>true</c> to show static; <c>false</c> for clean feed.</param>
        private void SetStaticVisible(bool visible)
        {
            if (StaticOverlay != null)
                StaticOverlay.gameObject.SetActive(visible);

            // Make monitor slightly dim when static is covering it
            if (MonitorDisplay != null)
            {
                Color c = MonitorDisplay.color;
                c.a                = visible ? 0.3f : 1f;
                MonitorDisplay.color = c;
            }
        }

#if UNITY_EDITOR
        // ─────────────────────────────────────────────────────────────────────
        //  Editor helpers
        // ─────────────────────────────────────────────────────────────────────

        private void OnValidate()
        {
            StaticClearDuration  = Mathf.Max(0.1f, StaticClearDuration);
            StaticClearCooldown  = Mathf.Max(0.1f, StaticClearCooldown);
            StaticAnimFPS        = Mathf.Max(1f,   StaticAnimFPS);
        }
#endif
    }
}
