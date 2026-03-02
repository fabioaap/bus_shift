using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BusShift.Bus;

namespace BusShift.UI
{
    /// <summary>
    /// Displays the current fuel level as a filled Image bar with a colour
    /// gradient and a TextMeshPro percentage label.
    ///
    /// <para>
    /// <b>Colour zones:</b>
    /// <list type="bullet">
    ///   <item>&gt; 50 %  — green (safe).</item>
    ///   <item>25–50 % — yellow (caution).</item>
    ///   <item>&lt; 25 % — red (low fuel).</item>
    /// </list>
    /// </para>
    ///
    /// <para>
    /// When fuel drops below <see cref="LowFuelThreshold"/> (default 15 %) the
    /// bar begins a smooth sine-wave blink to draw the driver's attention.
    /// </para>
    ///
    /// <para>
    /// Listens to the static events <see cref="FuelSystem.OnFuelChanged"/> and
    /// <see cref="FuelSystem.OnFuelEmpty"/>; no direct reference to
    /// <see cref="FuelSystem"/> is required.
    /// </para>
    /// </summary>
    public class FuelUI : MonoBehaviour
    {
        // ─────────────────────────────────────────────────────────────────────
        //  Inspector — References
        // ─────────────────────────────────────────────────────────────────────

        [Header("Fuel Bar")]
        [Tooltip("Image set to 'Filled' mode. fillAmount is driven by fuel percent.")]
        [SerializeField] private Image _fuelBarFill;

        [Tooltip("TMP label that displays the fuel percentage as an integer (e.g. '73%').")]
        [SerializeField] private TextMeshProUGUI _fuelPercentText;

        // ─────────────────────────────────────────────────────────────────────
        //  Inspector — Colour thresholds
        // ─────────────────────────────────────────────────────────────────────

        [Header("Colour Zones")]
        [Tooltip("Bar colour when fuel is above 50%.")]
        [SerializeField] private Color _colorHigh   = new Color(0.18f, 0.80f, 0.44f, 1f);  // emerald green

        [Tooltip("Bar colour when fuel is between 25% and 50%.")]
        [SerializeField] private Color _colorMedium = new Color(1.00f, 0.80f, 0.00f, 1f);  // amber yellow

        [Tooltip("Bar colour when fuel is below 25%.")]
        [SerializeField] private Color _colorLow    = new Color(0.90f, 0.20f, 0.20f, 1f);  // danger red

        // ─────────────────────────────────────────────────────────────────────
        //  Inspector — Low-fuel blink
        // ─────────────────────────────────────────────────────────────────────

        [Header("Low-Fuel Warning Blink")]
        [Tooltip("Fuel percent (0–1) below which the bar starts blinking. Default 0.15 (15%).")]
        [SerializeField] [Range(0f, 1f)] private float _lowFuelThreshold = 0.15f;

        [Tooltip("Blink frequency in cycles per second.")]
        [SerializeField] [Range(0.5f, 8f)] private float _blinkFrequency = 2f;

        [Tooltip("Minimum alpha during a blink pulse. 0 = fully invisible at the trough.")]
        [SerializeField] [Range(0f, 1f)] private float _blinkAlphaMin = 0.15f;

        // ─────────────────────────────────────────────────────────────────────
        //  Private state
        // ─────────────────────────────────────────────────────────────────────

        private bool  _blinking;
        private float _blinkTimer;
        private Color _baseBarColor;   // RGB without alpha, set by UpdateColor()

        // ─────────────────────────────────────────────────────────────────────
        //  Lifecycle
        // ─────────────────────────────────────────────────────────────────────

        private void Awake()
        {
            // Cache initial colour so the blink only touches alpha, not RGB.
            if (_fuelBarFill != null)
                _baseBarColor = _fuelBarFill.color;
        }

        private void OnEnable()
        {
            FuelSystem.OnFuelChanged += HandleFuelChanged;
            FuelSystem.OnFuelEmpty   += HandleFuelEmpty;
        }

        private void OnDisable()
        {
            FuelSystem.OnFuelChanged -= HandleFuelChanged;
            FuelSystem.OnFuelEmpty   -= HandleFuelEmpty;
        }

        private void Update()
        {
            if (!_blinking) return;

            _blinkTimer += Time.deltaTime * _blinkFrequency * Mathf.PI * 2f;

            // Sine mapped from [-1,1] → [_blinkAlphaMin, 1]
            float t     = (Mathf.Sin(_blinkTimer) + 1f) * 0.5f;
            float alpha = Mathf.Lerp(_blinkAlphaMin, 1f, t);

            SetBarAlpha(alpha);
        }

        // ─────────────────────────────────────────────────────────────────────
        //  Event handlers
        // ─────────────────────────────────────────────────────────────────────

        private void HandleFuelChanged(float percent)
        {
            UpdateFillAmount(percent);
            UpdateLabel(percent);
            UpdateColor(percent);
            SetBlinking(percent < _lowFuelThreshold);
        }

        private void HandleFuelEmpty()
        {
            // Ensure the bar reads exactly 0% even if the last OnFuelChanged was ~0.01%.
            HandleFuelChanged(0f);
        }

        // ─────────────────────────────────────────────────────────────────────
        //  Private helpers
        // ─────────────────────────────────────────────────────────────────────

        private void UpdateFillAmount(float percent)
        {
            if (_fuelBarFill != null)
                _fuelBarFill.fillAmount = percent;
        }

        private void UpdateLabel(float percent)
        {
            if (_fuelPercentText != null)
                _fuelPercentText.text = $"{Mathf.RoundToInt(percent * 100f)}%";
        }

        /// <summary>
        /// Updates the RGB colour of the bar fill based on the three fuel zones.
        /// Alpha is left unchanged so the blink can control it independently.
        /// </summary>
        private void UpdateColor(float percent)
        {
            if (_fuelBarFill == null) return;

            Color target = percent > 0.5f  ? _colorHigh   :
                           percent > 0.25f ? _colorMedium :
                                             _colorLow;

            // Preserve current alpha; only change RGB.
            target.a      = _fuelBarFill.color.a;
            _baseBarColor  = target;

            // Apply immediately (alpha is managed by the blink logic in Update).
            _fuelBarFill.color = _baseBarColor;
        }

        /// <summary>
        /// Enables or disables the blink effect.
        /// When disabled, alpha is restored to fully opaque.
        /// </summary>
        private void SetBlinking(bool active)
        {
            if (_blinking == active) return;

            _blinking = active;

            if (!_blinking)
            {
                _blinkTimer = 0f;
                SetBarAlpha(1f);
            }
        }

        private void SetBarAlpha(float alpha)
        {
            if (_fuelBarFill == null) return;

            Color c = _baseBarColor;
            c.a                = alpha;
            _fuelBarFill.color = c;
        }

        // ─────────────────────────────────────────────────────────────────────
        //  Editor helpers
        // ─────────────────────────────────────────────────────────────────────

#if UNITY_EDITOR
        private void OnValidate()
        {
            _lowFuelThreshold = Mathf.Clamp01(_lowFuelThreshold);
            _blinkFrequency   = Mathf.Max(0.1f, _blinkFrequency);
            _blinkAlphaMin    = Mathf.Clamp01(_blinkAlphaMin);
        }
#endif
    }
}
