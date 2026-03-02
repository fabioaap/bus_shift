using UnityEngine;

namespace BusShift.Interventions
{
    /// <summary>
    /// Panel Lock countermeasure — press SHIFT to instantly lock the driver's panel for 3 seconds.
    /// CRITICAL: Response must be instantaneous because Emma (CRIANÇA 2) attacks within 2 seconds.
    /// Effective against CRIANÇA 2 (Emma).
    /// Cooldown: 15 seconds after the lock expires.
    /// Issue #23.
    /// </summary>
    public class PanelLockSystem : InterventionBase
    {
        // ── Configuration ──────────────────────────────────────────────────────

        [Header("Panel Lock Settings")]
        [Tooltip("Duration the panel stays locked after activation.")]
        [SerializeField] private float _lockDuration = 3f;

        // ── State ───────────────────────────────────────────────────────────────

        private float _lockTimer = 0f;

        // ── Public API ──────────────────────────────────────────────────────────

        /// <summary>True while the panel lock is active and protecting controls.</summary>
        public bool IsLocked => _lockTimer > 0f;

        /// <summary>Remaining lock protection time in seconds (0 when unlocked).</summary>
        public float LockTimeRemaining => Mathf.Max(0f, _lockTimer);

        // ── Events ──────────────────────────────────────────────────────────────

        /// <summary>Fired the instant the panel lock activates.</summary>
        public static event System.Action OnPanelLocked;

        /// <summary>Fired when the panel lock expires (before cooldown begins).</summary>
        public static event System.Action OnPanelUnlocked;

        // ── Unity Lifecycle ─────────────────────────────────────────────────────

        private void Awake()
        {
            triggerKey       = KeyCode.LeftShift;
            cooldownDuration = 15f;
        }

        protected override void Update()
        {
            base.Update(); // tick cooldown

            HandleInput();
            TickLockTimer();
        }

        // ── InterventionBase Contract ────────────────────────────────────────────

        /// <inheritdoc/>
        public override bool CanActivate() => !IsLocked && !IsOnCooldown;

        /// <inheritdoc/>
        public override void Activate()
        {
            if (!CanActivate()) return;

            // CRITICAL: lock is applied immediately — no animation delay.
            _lockTimer = _lockDuration;
            OnPanelLocked?.Invoke();
        }

        // ── Helpers ─────────────────────────────────────────────────────────────

        private void HandleInput()
        {
            // GetKeyDown ensures a single-frame response — critical for sub-2s reaction window.
            if (Input.GetKeyDown(triggerKey))
            {
                Activate();
            }
        }

        private void TickLockTimer()
        {
            if (_lockTimer <= 0f) return;

            _lockTimer -= Time.deltaTime;

            if (_lockTimer <= 0f)
            {
                _lockTimer = 0f;
                OnPanelUnlocked?.Invoke();
                StartCooldown();
            }
        }
    }
}
