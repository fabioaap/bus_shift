using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BusShift.Interventions;

namespace BusShift.UI
{
    /// <summary>
    /// HUD icon representing a single driver intervention tool (Microphone, Panel Lock,
    /// Radio, or Headlights).
    ///
    /// Visual states:
    ///   • Available  — green background, fill at 100 %
    ///   • On cooldown — grey background, circular fill depletes from full → empty as
    ///                   the cooldown ticks down, then snaps back to green when ready.
    ///
    /// Setup in Unity Editor:
    ///   1. Place this component on a UI GameObject.
    ///   2. Assign the background Image, fill Image, and optional label references.
    ///   3. Call <see cref="Initialize"/> from code (or HUDController) passing the
    ///      relevant InterventionBase component.
    ///
    /// Alternatively, assign all fields in the Inspector and the icon will auto-search
    /// for an <see cref="InterventionBase"/> on the same GameObject when Initialize
    /// has not been called before the first Update.
    /// </summary>
    public class CooldownIconUI : MonoBehaviour
    {
        // ── Inspector ─────────────────────────────────────────────────────────

        [Header("Key & Label")]
        [Tooltip("Keyboard key shown on the icon (display only — does not handle input here).")]
        [SerializeField] private KeyCode displayKey = KeyCode.None;

        [Tooltip("Short tool label displayed on the icon, e.g. 'MIC', 'LOCK', 'RADIO', 'LIGHT'.")]
        [SerializeField] private string toolName = "TOOL";

        [Header("UI References")]
        [Tooltip("Background Image. Tinted green (available) or grey (on cooldown).")]
        [SerializeField] private Image backgroundImage;

        [Tooltip("Foreground Image used as a radial fill indicator. " +
                 "Set Image Type → Filled / Radial360 in the Inspector, " +
                 "or call Initialize() which configures it automatically.")]
        [SerializeField] private Image cooldownFillImage;

        [Tooltip("Optional label showing the trigger key (e.g. 'Q', 'E').")]
        [SerializeField] private TextMeshProUGUI keyLabel;

        [Tooltip("Optional label showing the short tool name (e.g. 'MIC').")]
        [SerializeField] private TextMeshProUGUI toolNameLabel;

        [Header("Colors")]
        [Tooltip("Background tint when the tool is ready.")]
        [SerializeField] private Color availableColor = new Color(0.2f, 0.75f, 0.2f); // green

        [Tooltip("Background tint while the tool is on cooldown.")]
        [SerializeField] private Color cooldownColor  = new Color(0.35f, 0.35f, 0.35f); // grey

        [Tooltip("Tint of the fill arc when on cooldown.")]
        [SerializeField] private Color fillCooldownColor  = new Color(1f, 1f, 1f, 0.6f);

        [Tooltip("Tint of the fill arc when available (fully filled).")]
        [SerializeField] private Color fillAvailableColor = new Color(0.2f, 0.9f, 0.2f, 0.8f);

        // ── Private state ─────────────────────────────────────────────────────

        private InterventionBase _intervention;

        // Tracks whether Initialize() has been called at least once
        private bool _initialised;

        // ── Public API ────────────────────────────────────────────────────────

        /// <summary>
        /// Binds this icon to the given <paramref name="intervention"/> and applies
        /// all label/fill configuration. Must be called before the icon becomes active.
        /// </summary>
        /// <param name="intervention">The intervention tool this icon represents.</param>
        public void Initialize(InterventionBase intervention)
        {
            _intervention = intervention;
            _initialised  = true;

            ConfigureLabels();
            ConfigureFillImage();
            Refresh(); // apply the initial visual state immediately
        }

        // ── Lifecycle ─────────────────────────────────────────────────────────

        private void Update()
        {
            // Lazy fallback: if nobody called Initialize, try to find a sibling component
            if (!_initialised)
            {
                InterventionBase sibling = GetComponent<InterventionBase>();
                if (sibling != null) Initialize(sibling);
                else return; // nothing to bind; skip Update until Initialize is called
            }

            Refresh();
        }

        // ── Helpers ───────────────────────────────────────────────────────────

        /// <summary>
        /// Reads the intervention's current state and applies visuals.
        /// Called every frame while the icon is active.
        /// </summary>
        private void Refresh()
        {
            if (_intervention == null) return;

            bool onCooldown = _intervention.IsOnCooldown;

            // ── Background colour ──
            if (backgroundImage != null)
                backgroundImage.color = onCooldown ? cooldownColor : availableColor;

            // ── Radial fill ──
            // fillAmount = 0   → cooldown just started (empty arc)
            // fillAmount = 1   → ready (full arc)
            if (cooldownFillImage != null)
            {
                float fill;

                if (onCooldown && _intervention.CooldownDuration > 0f)
                {
                    // Arc grows as cooldown ticks down: 0 (start) → 1 (ready)
                    fill = 1f - (_intervention.CooldownRemaining / _intervention.CooldownDuration);
                }
                else
                {
                    fill = 1f; // fully ready
                }

                cooldownFillImage.fillAmount = fill;
                cooldownFillImage.color = onCooldown ? fillCooldownColor : fillAvailableColor;
            }
        }

        /// <summary>
        /// Sets key and tool-name labels. Reads <see cref="displayKey"/> and
        /// <see cref="toolName"/> if the intervention doesn't override them,
        /// but prefers the intervention's own TriggerKey when available.
        /// </summary>
        private void ConfigureLabels()
        {
            // Key label: use the intervention's trigger key when possible
            if (keyLabel != null)
            {
                KeyCode key = (_intervention != null && _intervention.TriggerKey != KeyCode.None)
                    ? _intervention.TriggerKey
                    : displayKey;

                keyLabel.text = key != KeyCode.None ? key.ToString() : string.Empty;
            }

            if (toolNameLabel != null)
                toolNameLabel.text = toolName;
        }

        /// <summary>
        /// Programmatically configures the fill image for radial clock-style animation.
        /// This means the designer does not need to set these properties manually.
        /// </summary>
        private void ConfigureFillImage()
        {
            if (cooldownFillImage == null) return;

            cooldownFillImage.type          = Image.Type.Filled;
            cooldownFillImage.fillMethod    = Image.FillMethod.Radial360;

            // Fill starts from the top and drains clockwise (like a clock countdown)
            cooldownFillImage.fillOrigin    = (int)Image.Origin360.Top;
            cooldownFillImage.fillClockwise = true;
            cooldownFillImage.fillAmount    = 1f;
        }
    }
}
