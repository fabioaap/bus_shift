using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using BusShift.Core;

namespace BusShift.UI
{
    /// <summary>
    /// Reacts to <see cref="SanitySystem.OnSanityChanged"/> and drives URP post-processing
    /// effects through a <see cref="Volume"/> to give visual feedback on the driver's tension.
    ///
    /// Four escalating stages:
    ///   Stage 0 (0–25 %)  — light vignette only
    ///   Stage 1 (25–50 %) — vignette + chromatic aberration
    ///   Stage 2 (50–75 %) — vignette + chromatic + film grain
    ///   Stage 3 (75–100%) — all effects at max + subtle red colour filter
    ///
    /// All transitions are smoothly lerped in Update() so there are no jarring cuts.
    /// No numbers or labels are shown; the effect IS the feedback.
    /// </summary>
    public class TensionUI : MonoBehaviour
    {
        // ── Inspector ─────────────────────────────────────────────────────────

        [Header("Post-Processing Volume")]
        [Tooltip("URP Volume that owns the Vignette, ChromaticAberration, FilmGrain and " +
                 "ColorAdjustments overrides. Assign a runtime Volume (not the global one).")]
        [SerializeField] private Volume postProcessVolume;

        [Header("Transition")]
        [Tooltip("Speed at which post-processing values lerp toward their target. " +
                 "Higher = snappier transitions.")]
        [SerializeField] [Range(0.5f, 10f)] private float lerpSpeed = 2f;

        // ── Stage targets — set once per sanity change ────────────────────────

        private float _targetVignette;
        private float _targetChromatic;
        private float _targetGrain;
        private Color _targetColorFilter = Color.white;

        // ── Current smoothed values ───────────────────────────────────────────

        private float _currentVignette;
        private float _currentChromatic;
        private float _currentGrain;
        private Color _currentColorFilter = Color.white;

        // ── Cached post-processing component references ───────────────────────

        private Vignette          _vignette;
        private ChromaticAberration _chromatic;
        private FilmGrain          _filmGrain;
        private ColorAdjustments   _colorAdjustments;

        private bool _effectsReady;

        // ── Lifecycle ─────────────────────────────────────────────────────────

        private void Awake()
        {
            if (postProcessVolume == null)
            {
                Debug.LogWarning("[TensionUI] No Post-Process Volume assigned. " +
                                 "Tension VFX are disabled. Assign a Volume in the Inspector.");
                return;
            }

            // volume.profile creates a runtime clone of the shared profile so we never
            // dirty the project asset during Play mode.
            _effectsReady = TryCacheEffects(postProcessVolume.profile);
        }

        private void OnEnable()
        {
            SanitySystem.OnSanityChanged += HandleSanityChanged;
        }

        private void OnDisable()
        {
            SanitySystem.OnSanityChanged -= HandleSanityChanged;
        }

        private void Update()
        {
            if (!_effectsReady) return;

            float dt = Time.deltaTime * lerpSpeed;

            // Smoothly drive current values toward targets each frame
            _currentVignette     = Mathf.Lerp(_currentVignette,    _targetVignette,     dt);
            _currentChromatic    = Mathf.Lerp(_currentChromatic,   _targetChromatic,    dt);
            _currentGrain        = Mathf.Lerp(_currentGrain,       _targetGrain,        dt);
            _currentColorFilter  = Color.Lerp(_currentColorFilter, _targetColorFilter,  dt);

            ApplyToVolume();
        }

        // ── Event handler ─────────────────────────────────────────────────────

        private void HandleSanityChanged(float sanity)
        {
            // Map normalised sanity (0–1) to one of four effect stages.
            // NOTE: sanity represents *tension* here — higher = more effects.
            if (sanity < 0.25f)
            {
                // Stage 0 — barely noticeable
                _targetVignette     = 0.1f;
                _targetChromatic    = 0f;
                _targetGrain        = 0f;
                _targetColorFilter  = Color.white;
            }
            else if (sanity < 0.5f)
            {
                // Stage 1 — noticeable discomfort
                _targetVignette     = 0.3f;
                _targetChromatic    = 0.2f;
                _targetGrain        = 0f;
                _targetColorFilter  = Color.white;
            }
            else if (sanity < 0.75f)
            {
                // Stage 2 — heavy dread
                _targetVignette     = 0.5f;
                _targetChromatic    = 0.5f;
                _targetGrain        = 0.3f;
                _targetColorFilter  = Color.white;
            }
            else
            {
                // Stage 3 — critical / near game-over
                _targetVignette     = 0.8f;
                _targetChromatic    = 1.0f;
                _targetGrain        = 0.8f;
                // Subtle red tint: full red, green/blue at 75%
                _targetColorFilter  = new Color(1f, 0.75f, 0.75f, 1f);
            }
        }

        // ── Helpers ───────────────────────────────────────────────────────────

        /// <summary>
        /// Tries to retrieve each URP override component from the Volume's profile.
        /// Missing components generate a warning but don't break anything.
        /// </summary>
        private bool TryCacheEffects(VolumeProfile profile)
        {
            if (profile == null)
            {
                Debug.LogWarning("[TensionUI] Volume has no profile assigned.");
                return false;
            }

            bool ok = true;

            if (!profile.TryGet(out _vignette))
            {
                Debug.LogWarning("[TensionUI] Vignette override not found in Volume profile.");
                ok = false;
            }

            if (!profile.TryGet(out _chromatic))
            {
                Debug.LogWarning("[TensionUI] ChromaticAberration override not found in Volume profile.");
                ok = false;
            }

            if (!profile.TryGet(out _filmGrain))
            {
                Debug.LogWarning("[TensionUI] FilmGrain override not found in Volume profile.");
                ok = false;
            }

            if (!profile.TryGet(out _colorAdjustments))
            {
                Debug.LogWarning("[TensionUI] ColorAdjustments override not found in Volume profile.");
                ok = false;
            }

            // Ensure all retrieved components are actively overriding
            // (overrideState must be true or the component is ignored by the renderer)
            EnableOverrides();

            return ok; // returns false if any component was missing; effects still run partially
        }

        private void EnableOverrides()
        {
            if (_vignette != null)
            {
                _vignette.active = true;
                _vignette.intensity.overrideState = true;
            }

            if (_chromatic != null)
            {
                _chromatic.active = true;
                _chromatic.intensity.overrideState = true;
            }

            if (_filmGrain != null)
            {
                _filmGrain.active = true;
                _filmGrain.intensity.overrideState = true;
            }

            if (_colorAdjustments != null)
            {
                _colorAdjustments.active = true;
                _colorAdjustments.colorFilter.overrideState = true;
            }
        }

        private void ApplyToVolume()
        {
            if (_vignette != null)
                _vignette.intensity.value = _currentVignette;

            if (_chromatic != null)
                _chromatic.intensity.value = _currentChromatic;

            if (_filmGrain != null)
                _filmGrain.intensity.value = _currentGrain;

            if (_colorAdjustments != null)
                _colorAdjustments.colorFilter.value = _currentColorFilter;
        }
    }
}
