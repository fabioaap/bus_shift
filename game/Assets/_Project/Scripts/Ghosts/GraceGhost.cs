using System;
using UnityEngine;

namespace BusShift.Ghosts
{
    /// <summary>
    /// Grace – "The Observer"
    ///
    /// Grace appears in front of the driver's rearview mirror / security camera,
    /// blocking their view for a fixed duration. She has no direct countermeasure —
    /// the only strategy is to avoid collisions while she is present.
    ///
    /// If the bus collides with anything while Grace is active, tension increases.
    /// Grace automatically disappears after <see cref="_viewBlockDuration"/> seconds.
    ///
    /// Grace does NOT use the standard <see cref="GhostBase.BeginAttack"/> flow.
    /// She uses the Active state for her entire presence, then self-defeats on timeout.
    ///
    /// Wire-up:
    ///   • Subscribe to <see cref="OnViewBlocked"/> / <see cref="OnViewCleared"/> to
    ///     show/hide the camera-obstruction overlay in the UI.
    ///   • Notify Grace of bus collisions by calling <see cref="OnBusCollided"/>:
    ///     <code>
    ///     // In BusController.OnCollisionEnter (or a companion BusCollisionReporter):
    ///     void OnCollisionEnter(Collision col)
    ///     {
    ///         GraceGhost.OnBusCollided?.Invoke();
    ///     }
    ///     </code>
    ///   • Call <see cref="SetDayDifficulty"/> once per day from DayManager.
    /// </summary>
    public class GraceGhost : GhostBase
    {
        // ── Inspector ─────────────────────────────────────────────────────────
        [Header("Grace – The Observer")]
        [Tooltip("How long (seconds) Grace blocks the camera before auto-disappearing.")]
        [SerializeField] private float _viewBlockDuration = 15f;

        [Tooltip("Tension added when the bus collides while Grace is active.")]
        [SerializeField] private float _collisionTensionAmount = 0.3f;

        [Tooltip("Cooldown between appearances (seconds). Total gap = this + 2 s base delay.")]
        [SerializeField] private float _baseCooldown = 25f;

        // ── Static collision event ────────────────────────────────────────────
        /// <summary>
        /// Fire this event from BusController (or a companion BusCollisionReporter) whenever
        /// the bus collides with an obstacle. GraceGhost subscribes automatically.
        ///
        /// <code>
        /// // BusController.cs or BusCollisionReporter.cs:
        /// void OnCollisionEnter(Collision col)
        /// {
        ///     GraceGhost.OnBusCollided?.Invoke();
        /// }
        /// </code>
        /// </summary>
        public static event Action OnBusCollided;

        /// <summary>
        /// Raises <see cref="OnBusCollided"/> from outside this type without exposing
        /// direct event invocation to other classes.
        /// </summary>
        public static void NotifyBusCollision()
        {
            OnBusCollided?.Invoke();
        }

        // ── Presence events ───────────────────────────────────────────────────
        /// <summary>Fired when Grace appears and starts blocking the view.</summary>
        public static event Action OnViewBlocked;

        /// <summary>Fired when Grace disappears and the view is restored.</summary>
        public static event Action OnViewCleared;

        // ── Runtime state ─────────────────────────────────────────────────────
        private float _blockTimer;
        private bool  _isBlockingView;

        /// <summary>True while Grace is actively blocking the driver's camera / mirror.</summary>
        public bool IsBlockingView => _isBlockingView;

        // ── Lifecycle ─────────────────────────────────────────────────────────
        private void Awake()
        {
            GhostType      = GhostType.Grace;
            AttackInterval = _baseCooldown;
        }

        private void OnEnable()  => OnBusCollided += HandleBusCollision;
        private void OnDisable() => OnBusCollided -= HandleBusCollision;

        protected override void Update()
        {
            base.Update(); // handles Idle countdown → Activate

            if (CurrentState == GhostState.Active)
            {
                _blockTimer -= Time.deltaTime;
                if (_blockTimer <= 0f)
                    AutoDisappear();
            }
        }

        // ── GhostBase abstract methods ────────────────────────────────────────
        protected override void OnActivate()
        {
            _isBlockingView = true;
            _blockTimer     = _viewBlockDuration;
            OnViewBlocked?.Invoke();
        }

        /// <summary>Grace does not use the standard attack window.</summary>
        protected override void OnAttack() { /* unused — Grace has no directed attack */ }

        protected override void OnDefeated()
        {
            if (_isBlockingView)
            {
                _isBlockingView = false;
                OnViewCleared?.Invoke();
            }
        }

        // ── Collision handling ────────────────────────────────────────────────
        private void HandleBusCollision()
        {
            if (CurrentState == GhostState.Active && _isBlockingView)
                BusShift.Core.GameManager.Instance?.SanitySystem?.AddTension(_collisionTensionAmount);
        }

        // ── Auto-disappear ────────────────────────────────────────────────────
        private void AutoDisappear()
        {
            // Defeat() resets state, fires OnAttackDefeated, and schedules the cooldown.
            // This is an automatic timeout — not a player-driven defeat.
            Defeat();
        }

        // ── Difficulty scaling ────────────────────────────────────────────────
        /// <summary>
        /// Adjusts difficulty based on the current day (1-based).
        /// Grace appears more frequently; view block duration becomes slightly longer.
        /// </summary>
        public void SetDayDifficulty(int day)
        {
            int dayIndex       = Mathf.Max(0, day - 1);
            AttackInterval     = Mathf.Max(12f, _baseCooldown - (dayIndex * 2f));
            _viewBlockDuration = Mathf.Min(25f, 15f + (dayIndex * 1f)); // longer blockage each day
        }
    }
}
