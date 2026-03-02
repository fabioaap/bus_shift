using System;
using System.Collections;
using UnityEngine;

namespace BusShift.Core
{
    // ─────────────────────────────────────────────────────────────────────────
    //  Data
    // ─────────────────────────────────────────────────────────────────────────

    /// <summary>
    /// Serializable snapshot of scene lighting at a given time-of-day.
    /// Assigned to <see cref="LightingController._dayPreset"/> and
    /// <see cref="LightingController._nightPreset"/> in the Inspector.
    /// </summary>
    [Serializable]
    public struct LightPreset
    {
        /// <summary>Global ambient light color (<see cref="RenderSettings.ambientLight"/>).</summary>
        public Color ambientColor;

        /// <summary>Exponential fog color (<see cref="RenderSettings.fogColor"/>).</summary>
        public Color fogColor;

        /// <summary>Exponential fog density (<see cref="RenderSettings.fogDensity"/>).</summary>
        public float fogDensity;

        /// <summary>Color tint applied to the main Directional Light.</summary>
        public Color directionalLightColor;

        /// <summary>Intensity of the main Directional Light (0–8 range).</summary>
        public float directionalLightIntensity;

        /// <summary>
        /// X rotation (pitch) of the Directional Light in degrees.
        /// 50° ≈ overhead morning sun; 210° ≈ low night moon.
        /// </summary>
        public float directionalLightAngle;
    }

    // ─────────────────────────────────────────────────────────────────────────
    //  Controller
    // ─────────────────────────────────────────────────────────────────────────

    /// <summary>
    /// Singleton MonoBehaviour that drives scene lighting between a Day preset
    /// and a Night preset with a smooth Lerp transition.
    ///
    /// <para>
    /// <b>Controlled render properties:</b>
    /// <list type="bullet">
    ///   <item><see cref="RenderSettings.skybox"/> — swapped at transition end.</item>
    ///   <item><see cref="RenderSettings.ambientLight"/> — lerped each frame.</item>
    ///   <item><see cref="RenderSettings.fogColor"/> — lerped each frame.</item>
    ///   <item><see cref="RenderSettings.fogDensity"/> — lerped each frame.</item>
    ///   <item>Main Directional Light color, intensity, and X-rotation — lerped each frame.</item>
    /// </list>
    /// </para>
    ///
    /// <para>
    /// Subscribes to <see cref="DayManager.OnPeriodChanged"/>:
    /// period 0 (Morning) → Day preset; period 1 (Night) → Night preset.
    /// </para>
    ///
    /// <para>
    /// Setup: Attach to a persistent GameObject alongside <see cref="DayManager"/>.
    /// Assign <c>_daySkybox</c>, <c>_nightSkybox</c>, and <c>_directionalLight</c>
    /// in the Inspector (light is auto-found on Awake if not set).
    /// </para>
    /// </summary>
    public class LightingController : MonoBehaviour
    {
        // ─────────────────────────────────────────────────────────────────────
        //  Singleton
        // ─────────────────────────────────────────────────────────────────────

        /// <summary>Global singleton instance.</summary>
        public static LightingController Instance { get; private set; }

        // ─────────────────────────────────────────────────────────────────────
        //  Static Events
        // ─────────────────────────────────────────────────────────────────────

        /// <summary>
        /// Fired when a day/night transition Coroutine finishes.
        /// <c>true</c> = the scene is now Night; <c>false</c> = the scene is now Day.
        /// Subscribe from ghost spawn systems or UI that react to time-of-day.
        /// </summary>
        public static event Action<bool> OnNightTransitionComplete;

        // ─────────────────────────────────────────────────────────────────────
        //  Inspector — Skyboxes
        // ─────────────────────────────────────────────────────────────────────

        [Header("Skybox Materials")]
        [Tooltip("Skybox material applied during the Day (Morning) preset.")]
        [SerializeField] private Material _daySkybox;

        [Tooltip("Skybox material applied during the Night preset.")]
        [SerializeField] private Material _nightSkybox;

        // ─────────────────────────────────────────────────────────────────────
        //  Inspector — Directional Light
        // ─────────────────────────────────────────────────────────────────────

        [Header("Directional Light")]
        [Tooltip("The main scene Directional Light (sun/moon). Auto-found via FindAnyObjectByType if null.")]
        [SerializeField] private Light _directionalLight;

        // ─────────────────────────────────────────────────────────────────────
        //  Inspector — Presets
        // ─────────────────────────────────────────────────────────────────────

        [Header("Light Presets")]
        [Tooltip("Lighting configuration for the Morning period. Defaults are tuned for a low-poly horror bus ride.")]
        [SerializeField] private LightPreset _dayPreset = new LightPreset
        {
            // #87CEEB — light sky blue typical of an overcast morning
            ambientColor             = new Color(0.529f, 0.808f, 0.922f),
            fogColor                 = new Color(0.529f, 0.808f, 0.922f),
            fogDensity               = 0.005f,
            directionalLightColor    = Color.white,
            directionalLightIntensity = 1.2f,
            directionalLightAngle    = 50f      // overhead-ish morning sun
        };

        [Tooltip("Lighting configuration for the Night period. Defaults create a dark, oppressive horror atmosphere.")]
        [SerializeField] private LightPreset _nightPreset = new LightPreset
        {
            // #1a1a2e — deep dark blue typical of late-night dread
            ambientColor             = new Color(0.102f, 0.102f, 0.180f),
            fogColor                 = new Color(0.047f, 0.047f, 0.094f),
            fogDensity               = 0.02f,
            directionalLightColor    = new Color(0.300f, 0.300f, 0.500f),
            directionalLightIntensity = 0.3f,
            directionalLightAngle    = 210f     // low backlight for eerie rim
        };

        // ─────────────────────────────────────────────────────────────────────
        //  Inspector — Transition
        // ─────────────────────────────────────────────────────────────────────

        [Header("Transition")]
        [Tooltip("Duration in seconds of the Day↔Night lighting crossfade.")]
        [SerializeField] private float _transitionDuration = 3.0f;

        // ─────────────────────────────────────────────────────────────────────
        //  Private State
        // ─────────────────────────────────────────────────────────────────────

        private Coroutine _activeTransition;

        // ─────────────────────────────────────────────────────────────────────
        //  Lifecycle
        // ─────────────────────────────────────────────────────────────────────

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            if (_directionalLight == null)
                _directionalLight = FindAnyObjectByType<Light>();
        }

        private void OnEnable()
        {
            DayManager.OnPeriodChanged += HandlePeriodChanged;
        }

        private void OnDisable()
        {
            DayManager.OnPeriodChanged -= HandlePeriodChanged;
        }

        // ─────────────────────────────────────────────────────────────────────
        //  Public API
        // ─────────────────────────────────────────────────────────────────────

        /// <summary>
        /// Instantly snaps the scene to the specified preset with no Lerp.
        /// Useful for scene initialisation before the player can see the world.
        /// </summary>
        /// <param name="night"><c>true</c> to apply the Night preset; <c>false</c> for Day.</param>
        public void ApplyPresetImmediate(bool night)
        {
            LightPreset preset = night ? _nightPreset : _dayPreset;
            ApplyToRenderSettings(preset);

            Material skybox = night ? _nightSkybox : _daySkybox;
            if (skybox != null)
                RenderSettings.skybox = skybox;

#if UNITY_EDITOR || DEVELOPMENT_BUILD
            Debug.Log($"[LightingController] Preset applied immediately: {(night ? "Night" : "Day")}.");
#endif
        }

        // ─────────────────────────────────────────────────────────────────────
        //  Event Handlers
        // ─────────────────────────────────────────────────────────────────────

        /// <summary>
        /// Handles <see cref="DayManager.OnPeriodChanged"/>.
        /// period 0 = Morning → Day preset; period 1 = Night → Night preset.
        /// </summary>
        private void HandlePeriodChanged(int day, int period)
        {
            bool isNight    = period == 1;
            LightPreset target = isNight ? _nightPreset : _dayPreset;

            if (_activeTransition != null)
                StopCoroutine(_activeTransition);

            _activeTransition = StartCoroutine(TransitionToPreset(target, isNight));
        }

        // ─────────────────────────────────────────────────────────────────────
        //  Coroutine
        // ─────────────────────────────────────────────────────────────────────

        /// <summary>
        /// Lerps all lighting properties from their current values to <paramref name="target"/>
        /// over <see cref="_transitionDuration"/> seconds, then fires
        /// <see cref="OnNightTransitionComplete"/>.
        /// </summary>
        /// <param name="target">Destination lighting preset.</param>
        /// <param name="isNight">Passed through to the completion event.</param>
        private IEnumerator TransitionToPreset(LightPreset target, bool isNight)
        {
            // Snapshot current render state so we can Lerp from it
            Color startAmbient        = RenderSettings.ambientLight;
            Color startFogColor       = RenderSettings.fogColor;
            float startFogDensity     = RenderSettings.fogDensity;

            Color startLightColor     = _directionalLight != null ? _directionalLight.color     : Color.white;
            float startLightIntensity = _directionalLight != null ? _directionalLight.intensity  : 1f;
            float startLightAngle     = _directionalLight != null ? _directionalLight.transform.eulerAngles.x : 50f;

            float elapsed = 0f;

            while (elapsed < _transitionDuration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / _transitionDuration);

                RenderSettings.ambientLight = Color.Lerp(startAmbient,    target.ambientColor, t);
                RenderSettings.fogColor     = Color.Lerp(startFogColor,   target.fogColor,     t);
                RenderSettings.fogDensity   = Mathf.Lerp(startFogDensity, target.fogDensity,   t);

                if (_directionalLight != null)
                {
                    _directionalLight.color     = Color.Lerp(startLightColor,     target.directionalLightColor,     t);
                    _directionalLight.intensity  = Mathf.Lerp(startLightIntensity, target.directionalLightIntensity, t);

                    Vector3 angles = _directionalLight.transform.eulerAngles;
                    angles.x = Mathf.LerpAngle(startLightAngle, target.directionalLightAngle, t);
                    _directionalLight.transform.eulerAngles = angles;
                }

                yield return null;
            }

            // Snap to final values to eliminate floating-point drift
            ApplyToRenderSettings(target);

            // Swap skybox at the end of the transition (avoids a jarring mid-fade pop)
            Material skybox = isNight ? _nightSkybox : _daySkybox;
            if (skybox != null)
                RenderSettings.skybox = skybox;

            OnNightTransitionComplete?.Invoke(isNight);

#if UNITY_EDITOR || DEVELOPMENT_BUILD
            Debug.Log($"[LightingController] Transition complete → {(isNight ? "Night" : "Day")}.");
#endif

            _activeTransition = null;
        }

        // ─────────────────────────────────────────────────────────────────────
        //  Private Helpers
        // ─────────────────────────────────────────────────────────────────────

        /// <summary>
        /// Writes all fields of <paramref name="preset"/> directly to
        /// <see cref="RenderSettings"/> and the Directional Light — no interpolation.
        /// </summary>
        private void ApplyToRenderSettings(LightPreset preset)
        {
            RenderSettings.ambientLight = preset.ambientColor;
            RenderSettings.fogColor     = preset.fogColor;
            RenderSettings.fogDensity   = preset.fogDensity;

            if (_directionalLight == null) return;

            _directionalLight.color     = preset.directionalLightColor;
            _directionalLight.intensity  = preset.directionalLightIntensity;

            Vector3 angles = _directionalLight.transform.eulerAngles;
            angles.x = preset.directionalLightAngle;
            _directionalLight.transform.eulerAngles = angles;
        }

        // ─────────────────────────────────────────────────────────────────────
        //  Editor Helpers
        // ─────────────────────────────────────────────────────────────────────

#if UNITY_EDITOR
        private void OnValidate()
        {
            _transitionDuration             = Mathf.Max(0.1f, _transitionDuration);
            _dayPreset.fogDensity            = Mathf.Max(0f,   _dayPreset.fogDensity);
            _nightPreset.fogDensity          = Mathf.Max(0f,   _nightPreset.fogDensity);
            _dayPreset.directionalLightIntensity  = Mathf.Max(0f, _dayPreset.directionalLightIntensity);
            _nightPreset.directionalLightIntensity = Mathf.Max(0f, _nightPreset.directionalLightIntensity);
        }
#endif
    }
}
