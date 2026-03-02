using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BusShift.Core;

namespace BusShift.UI
{
    /// <summary>
    /// Driver's wristwatch HUD element.
    ///
    /// Behaviour:
    ///   • Press SPACE   → reveals watch panel for <see cref="displayDuration"/> seconds
    ///   • After hiding  → starts a <see cref="cooldownDuration"/>-second cooldown
    ///   • During cooldown SPACE is ignored
    ///
    /// The panel shows:
    ///   • Current in-game time via <see cref="TimerSystem"/> ("HH:mm")
    ///   • A fill-bar representing the driver's current sanity/tension level
    ///
    /// Visibility is driven by a <see cref="CanvasGroup"/> alpha fade.
    /// </summary>
    public class WatchUI : MonoBehaviour
    {
        // ── Inspector ─────────────────────────────────────────────────────────

        [Header("References")]
        [Tooltip("CanvasGroup on the watch panel (controls fade in/out).")]
        [SerializeField] private CanvasGroup watchCanvasGroup;

        [Tooltip("TextMeshPro label that displays the current in-game time (e.g. '06:47').")]
        [SerializeField] private TextMeshProUGUI timeText;

        [Tooltip("Image used as a tension bar. Set Type to Filled / Horizontal in the Inspector.")]
        [SerializeField] private Image tensionBarImage;

        [Tooltip("TimerSystem in the scene. Provides FormattedTime and OnTimeChanged event.")]
        [SerializeField] private TimerSystem timerSystem;

        [Header("Behaviour")]
        [Tooltip("Seconds the watch stays visible after SPACE is pressed.")]
        [SerializeField] [Range(1f, 15f)] private float displayDuration = 5f;

        [Tooltip("Seconds the player must wait before using the watch again.")]
        [SerializeField] [Range(5f, 60f)] private float cooldownDuration = 20f;

        [Tooltip("Alpha fade speed (higher = faster fade).")]
        [SerializeField] [Range(1f, 20f)] private float fadeSpeed = 6f;

        [Header("Tension Bar Colors")]
        [Tooltip("Bar color at low tension (left side of gradient).")]
        [SerializeField] private Color lowTensionColor  = new Color(0.2f, 0.8f, 0.2f); // green

        [Tooltip("Bar color at high tension (right side of gradient).")]
        [SerializeField] private Color highTensionColor = new Color(0.9f, 0.2f, 0.2f); // red

        // ── Public API ────────────────────────────────────────────────────────

        /// <summary>Seconds remaining on the watch cooldown (0 = ready).</summary>
        public float CooldownRemaining { get; private set; }

        /// <summary>True while the watch panel is visible.</summary>
        public bool IsVisible { get; private set; }

        // ── Private state ─────────────────────────────────────────────────────

        private float _displayTimer;
        private float _targetAlpha;
        private float _currentSanity;

        // ── Lifecycle ─────────────────────────────────────────────────────────

        private void Awake()
        {
            if (watchCanvasGroup != null)
                watchCanvasGroup.alpha = 0f;
        }

        private void OnEnable()
        {
            SanitySystem.OnSanityChanged += HandleSanityChanged;

            // Subscribe to the TimerSystem event for zero-cost time updates
            TimerSystem.OnTimeChanged += HandleTimeChanged;
        }

        private void OnDisable()
        {
            SanitySystem.OnSanityChanged -= HandleSanityChanged;
            TimerSystem.OnTimeChanged   -= HandleTimeChanged;
        }

        private void Update()
        {
            TickCooldown();
            HandleInput();
            TickDisplay();
            FadePanel();
        }

        // ── Public control ────────────────────────────────────────────────────

        /// <summary>Force-reveals the watch without consuming the cooldown (e.g. tutorial).</summary>
        public void ForceShow()
        {
            OpenWatch();
        }

        // ── Private logic ─────────────────────────────────────────────────────

        private void HandleInput()
        {
            if (Input.GetKeyDown(KeyCode.Space) && CooldownRemaining <= 0f)
                OpenWatch();
        }

        private void OpenWatch()
        {
            IsVisible       = true;
            _displayTimer   = displayDuration;
            _targetAlpha    = 1f;

            SetInteractable(true);
            RefreshTimeDisplay();
            RefreshTensionBar();
        }

        private void CloseWatch()
        {
            IsVisible      = false;
            _targetAlpha   = 0f;
            CooldownRemaining = cooldownDuration; // begin cooldown after hiding
            SetInteractable(false);
        }

        private void TickCooldown()
        {
            if (CooldownRemaining > 0f)
                CooldownRemaining = Mathf.Max(0f, CooldownRemaining - Time.deltaTime);
        }

        private void TickDisplay()
        {
            if (!IsVisible) return;

            _displayTimer -= Time.deltaTime;

            if (_displayTimer <= 0f)
                CloseWatch();
        }

        private void FadePanel()
        {
            if (watchCanvasGroup == null) return;
            watchCanvasGroup.alpha = Mathf.Lerp(
                watchCanvasGroup.alpha, _targetAlpha, Time.deltaTime * fadeSpeed);
        }

        // ── Data refresh ──────────────────────────────────────────────────────

        /// <summary>
        /// Called by TimerSystem.OnTimeChanged every in-game tick.
        /// Only updates the label when the watch is visible (perf guard).
        /// </summary>
        private void HandleTimeChanged(string formattedTime)
        {
            if (!IsVisible || timeText == null) return;
            timeText.text = formattedTime;
        }

        /// <summary>Refresh the time display on demand (e.g. when watch first opens).</summary>
        private void RefreshTimeDisplay()
        {
            if (timeText == null) return;

            // Prefer TimerSystem; fall back to real wall-clock time so the UI always shows something
            string time = timerSystem != null
                ? timerSystem.FormattedTime
                : System.DateTime.Now.ToString("HH:mm");

            timeText.text = time;
        }

        private void RefreshTensionBar()
        {
            if (tensionBarImage == null) return;

            tensionBarImage.fillAmount = _currentSanity;

            // Interpolate bar colour from green → red as tension grows
            tensionBarImage.color = Color.Lerp(lowTensionColor, highTensionColor, _currentSanity);
        }

        private void HandleSanityChanged(float sanity)
        {
            _currentSanity = sanity;

            // Keep bar in sync while the watch is open
            if (IsVisible)
                RefreshTensionBar();
        }

        private void SetInteractable(bool value)
        {
            if (watchCanvasGroup == null) return;
            watchCanvasGroup.interactable   = value;
            watchCanvasGroup.blocksRaycasts = value;
        }
    }
}
