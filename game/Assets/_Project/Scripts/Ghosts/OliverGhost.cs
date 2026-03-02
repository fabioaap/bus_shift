using System;
using UnityEngine;

namespace BusShift.Ghosts
{
    /// <summary>
    /// Oliver – "The Artist"
    ///
    /// Oliver settles in a bus seat and begins drawing. His frightening sketches
    /// continuously scare the living children, draining sanity every second.
    /// If the bus is running late, the tension rate doubles.
    ///
    /// Oliver automatically disappears after <see cref="_drawingDuration"/> seconds,
    /// or can be defeated early by the Microphone (Q).
    ///
    /// Oliver does NOT use the standard <see cref="GhostBase.BeginAttack"/> flow.
    /// He uses the Active state for his entire presence, then self-defeats on timeout.
    ///
    /// Wire-up:
    ///   • Call <see cref="Defeat"/> from your InputHandler when the player uses the Microphone (Q).
    ///   • Keep <see cref="BusIsLate"/> in sync with your TimerSystem each frame:
    ///     <code>
    ///     // Inside TimerSystem.Update() or wherever IsLate is evaluated:
    ///     OliverGhost.BusIsLate = IsLate;
    ///     </code>
    ///   • Subscribe to <see cref="OnDrawingStarted"/> / <see cref="OnDrawingStopped"/>
    ///     to trigger Oliver's drawing animation.
    ///   • Call <see cref="SetDayDifficulty"/> once per day from DayManager.
    /// </summary>
    public class OliverGhost : GhostBase
    {
        // ── Inspector ─────────────────────────────────────────────────────────
        [Header("Oliver – The Artist")]
        [Tooltip("Base tension added per second while Oliver is drawing.")]
        [SerializeField] private float _baseTensionRate = 0.05f;

        [Tooltip("Multiplier applied to tension rate when the bus is running late.")]
        [SerializeField] private float _lateTensionMultiplier = 2f;

        [Tooltip("Seconds Oliver stays before auto-disappearing if not defeated.")]
        [SerializeField] private float _drawingDuration = 20f;

        [Tooltip("Cooldown between appearances (seconds). Total gap = this + 2 s base delay.")]
        [SerializeField] private float _baseCooldown = 30f;

        // ── Static integration point ──────────────────────────────────────────
        /// <summary>
        /// Set this to <c>true</c> while the bus is running behind schedule (TimerSystem.IsLate).
        /// Oliver's tension rate doubles when this flag is active.
        ///
        /// <code>
        /// // Inside TimerSystem.Update() or wherever schedule lateness is evaluated:
        /// OliverGhost.BusIsLate = IsLate;
        /// </code>
        /// </summary>
        public static bool BusIsLate { get; set; }

        // ── Events ────────────────────────────────────────────────────────────
        /// <summary>Fired when Oliver begins drawing (ghost becomes active).</summary>
        public static event Action OnDrawingStarted;

        /// <summary>Fired when Oliver stops drawing (defeated by Microphone or timer expired).</summary>
        public static event Action OnDrawingStopped;

        // ── Runtime state ─────────────────────────────────────────────────────
        private float _drawingTimer;
        private bool  _isDrawing;
        private float _currentBaseTensionRate;

        /// <summary>True while Oliver is actively drawing in the bus seats.</summary>
        public bool IsDrawing => _isDrawing;

        /// <summary>
        /// Current tension added per second.
        /// Doubles when <see cref="BusIsLate"/> is true.
        /// Returns 0 when Oliver is not drawing.
        /// </summary>
        public float TensionRate
        {
            get
            {
                if (!_isDrawing) return 0f;
                return _currentBaseTensionRate * (BusIsLate ? _lateTensionMultiplier : 1f);
            }
        }

        // ── Lifecycle ─────────────────────────────────────────────────────────
        private void Awake()
        {
            GhostType              = GhostType.Oliver;
            AttackInterval         = _baseCooldown;
            _currentBaseTensionRate = _baseTensionRate;
        }

        protected override void Update()
        {
            base.Update(); // handles Idle countdown → Activate

            if (CurrentState == GhostState.Active && _isDrawing)
            {
                // Continuous sanity drain
                BusShift.Core.GameManager.Instance?.SanitySystem?.AddTension(TensionRate * Time.deltaTime);

                // Auto-disappear after drawing duration elapses
                _drawingTimer -= Time.deltaTime;
                if (_drawingTimer <= 0f)
                    Defeat();
            }
        }

        // ── GhostBase abstract methods ────────────────────────────────────────
        protected override void OnActivate()
        {
            _isDrawing    = true;
            _drawingTimer = _drawingDuration;
            IsVulnerable  = true;
            OnDrawingStarted?.Invoke();
        }

        /// <summary>Oliver does not use the standard attack window.</summary>
        protected override void OnAttack() { /* unused — Oliver has no directed attack */ }

        protected override void OnDefeated()
        {
            _isDrawing   = false;
            IsVulnerable = false;
            OnDrawingStopped?.Invoke();
            // GhostBase.Defeat() schedules ResetToIdle after 2 s → AttackInterval cooldown.
        }

        // ── Difficulty scaling ────────────────────────────────────────────────
        /// <summary>
        /// Adjusts difficulty based on the current day (1-based).
        /// Oliver appears more frequently and draws at a slightly higher tension rate.
        /// </summary>
        public void SetDayDifficulty(int day)
        {
            int dayIndex           = Mathf.Max(0, day - 1);
            AttackInterval         = Mathf.Max(12f, _baseCooldown - (dayIndex * 3f));
            _currentBaseTensionRate = _baseTensionRate + (dayIndex * 0.01f); // small per-day increase
        }
    }
}
