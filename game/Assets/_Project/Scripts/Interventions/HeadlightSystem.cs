using UnityEngine;

namespace BusShift.Interventions
{
    /// <summary>
    /// Headlight countermeasure — press F to activate the bus headlight for 15 seconds.
    /// Cooldown: 20 seconds after the light turns off.
    /// Integrates with a Unity Light component (intensity 0 → 3 when active).
    /// Issue #16.
    /// </summary>
    [RequireComponent(typeof(Light))]
    public class HeadlightSystem : InterventionBase
    {
        // ── Configuration ──────────────────────────────────────────────────────

        [Header("Headlight Settings")]
        [Tooltip("Duration the headlight stays on after activation.")]
        [SerializeField] private float _activeDuration = 15f;

        [Tooltip("Light intensity when the headlight is ON.")]
        [SerializeField] private float _activeIntensity = 3f;

        // ── State ───────────────────────────────────────────────────────────────

        private float _activeTimer = 0f;
        private Light _light;

        // ── Public API ──────────────────────────────────────────────────────────

        /// <summary>True while the headlight is on.</summary>
        public bool IsActive => _activeTimer > 0f;

        /// <summary>Remaining seconds the light will stay on (0 when off).</summary>
        public float ActiveTimeRemaining => Mathf.Max(0f, _activeTimer);

        // ── Events ──────────────────────────────────────────────────────────────

        /// <summary>Fired when the headlight turns on.</summary>
        public static event System.Action OnHeadlightActivated;

        /// <summary>Fired when the headlight turns off (before cooldown begins).</summary>
        public static event System.Action OnHeadlightDeactivated;

        // ── Unity Lifecycle ─────────────────────────────────────────────────────

        private void Awake()
        {
            triggerKey       = KeyCode.F;
            cooldownDuration = 20f;

            _light           = GetComponent<Light>();
            _light.intensity = 0f;
            _light.enabled   = false;
        }

        protected override void Update()
        {
            base.Update(); // tick cooldown

            HandleInput();
            TickActiveTimer();
        }

        // ── InterventionBase Contract ────────────────────────────────────────────

        /// <inheritdoc/>
        public override bool CanActivate() => !IsActive && !IsOnCooldown;

        /// <inheritdoc/>
        public override void Activate()
        {
            if (!CanActivate()) return;

            _activeTimer     = _activeDuration;
            _light.intensity = _activeIntensity;
            _light.enabled   = true;

            OnHeadlightActivated?.Invoke();
        }

        // ── Helpers ─────────────────────────────────────────────────────────────

        private void HandleInput()
        {
            if (Input.GetKeyDown(triggerKey))
            {
                Activate();
            }
        }

        private void TickActiveTimer()
        {
            if (_activeTimer <= 0f) return;

            _activeTimer -= Time.deltaTime;

            if (_activeTimer <= 0f)
            {
                _activeTimer     = 0f;
                _light.intensity = 0f;
                _light.enabled   = false;

                OnHeadlightDeactivated?.Invoke();
                StartCooldown();
            }
        }
    }
}
