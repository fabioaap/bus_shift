using System;
using UnityEngine;

namespace BusShift.Ghosts
{
    /// <summary>
    /// Marcus – "The Invader"
    ///
    /// Spawns at a random rear row (4 or 5) and advances one row toward the driver
    /// every <see cref="_rowAdvanceInterval"/> seconds. When he reaches Row 1,
    /// <see cref="GhostBase.BeginAttack"/> is called and the player has 30 seconds
    /// to use the Microphone (Q) before Game Over.
    ///
    /// Row numbering: 1 = closest to driver  |  5 = back of bus.
    ///
    /// Wire-up:
    ///   • Call <see cref="Defeat"/> from your InputHandler when the player uses the Microphone.
    ///   • Subscribe to <see cref="OnRowChanged"/> to animate Marcus moving between seats.
    ///   • Call <see cref="SetDayDifficulty"/> once per day from DayManager.
    /// </summary>
    public class MarcusGhost : GhostBase
    {
        // ── Inspector ─────────────────────────────────────────────────────────
        [Header("Marcus – The Invader")]
        [SerializeField] private float _baseRowAdvanceInterval  = 15f;
        [SerializeField] private float _difficultyReductionPerDay = 2f;
        [SerializeField] private float _minRowAdvanceInterval   = 5f;
        [SerializeField] private float _baseAttackInterval      = 30f; // time between appearances
        [SerializeField] private int   _totalRows               = 5;   // row 5 = back of bus
        [SerializeField] private int   _spawnRowMin             = 4;   // spawns in rows 4-5

        // ── Events ────────────────────────────────────────────────────────────
        /// <summary>
        /// Fired each time Marcus moves to a new row (including initial spawn and defeat reset).
        /// Row 1 = near driver, Row 5 = back of bus.
        /// </summary>
        public static event Action<int> OnRowChanged;

        // ── Runtime state ─────────────────────────────────────────────────────
        private float _rowAdvanceInterval;
        private float _rowAdvanceTimer;
        private int   _currentRow;

        /// <summary>Current row: 1 = near driver, 5 = back of bus.</summary>
        public int CurrentRow => _currentRow;

        // ── Lifecycle ─────────────────────────────────────────────────────────
        private void Awake()
        {
            GhostType           = GhostType.Marcus;
            AttackWindow        = 30f;
            AttackInterval      = _baseAttackInterval;
            _rowAdvanceInterval = _baseRowAdvanceInterval;
        }

        protected override void Update()
        {
            base.Update(); // handles Idle countdown → Activate, Attacking countdown → GameOver

            if (CurrentState == GhostState.Active)
            {
                _rowAdvanceTimer -= Time.deltaTime;
                if (_rowAdvanceTimer <= 0f)
                    AdvanceRow();
            }
        }

        // ── GhostBase abstract methods ────────────────────────────────────────
        protected override void OnActivate()
        {
            // Spawn at a random rear row
            int spawnRow = UnityEngine.Random.Range(_spawnRowMin, _totalRows + 1);
            SetRow(spawnRow);
            _rowAdvanceTimer = _rowAdvanceInterval;
        }

        protected override void OnAttack()
        {
            // Marcus has reached Row 1 – the 30-second Microphone window is now active.
            // Visual feedback (e.g. Marcus looming over the driver) should be triggered here
            // by subscribing to GhostBase.OnAttackStarted.
        }

        protected override void OnDefeated()
        {
            // Immediately snap back to a random rear row for the visual feedback.
            // OnActivate will also set a fresh row when the ghost reactivates after cooldown.
            int row = UnityEngine.Random.Range(_spawnRowMin, _totalRows + 1);
            SetRow(row);
        }

        // ── Row advancement ───────────────────────────────────────────────────
        private void AdvanceRow()
        {
            _rowAdvanceTimer = _rowAdvanceInterval;
            SetRow(_currentRow - 1);

            if (_currentRow <= 1)
                BeginAttack();
        }

        private void SetRow(int row)
        {
            _currentRow = Mathf.Clamp(row, 1, _totalRows);
            OnRowChanged?.Invoke(_currentRow);
        }

        // ── Difficulty scaling ────────────────────────────────────────────────
        /// <summary>
        /// Adjusts Marcus's difficulty based on the current day (1-based).
        /// Row advance interval decreases by <see cref="_difficultyReductionPerDay"/> seconds per day.
        /// Attack interval also shrinks so he reappears more often.
        /// Observation time scales up progressively so higher-day players must stare longer:
        /// days 1–2 = 3 s  |  days 3–4 = 5 s  |  day 5+ = 6 s.
        /// </summary>
        public void SetDayDifficulty(int day)
        {
            int dayIndex = Mathf.Max(0, day - 1); // convert to 0-based
            _rowAdvanceInterval = Mathf.Max(
                _minRowAdvanceInterval,
                _baseRowAdvanceInterval - (dayIndex * _difficultyReductionPerDay));
            AttackInterval = Mathf.Max(10f, _baseAttackInterval - (dayIndex * 2f));

            // Observation scaling: looking at Marcus must be sustained longer on harder days.
            if (day >= 5)
                observationTimeToVanish = 6f;  // base 3 s + day-3 bonus 2 s + day-5 bonus 1 s
            else if (day >= 3)
                observationTimeToVanish = 5f;  // base 3 s + day-3 bonus 2 s
            else
                observationTimeToVanish = 3f;  // base
        }

        // ── Observation ───────────────────────────────────────────────────────
        /// <summary>
        /// Called by <see cref="GhostVisibilityChecker"/> once the player has sustained
        /// observation for <see cref="GhostBase.observationTimeToVanish"/> seconds.
        /// Marcus retreats immediately and awards a small sanity recovery.
        /// </summary>
        /// <remarks>
        /// Sanity gain: 0.05 (equivalent to +5 points on a 0–100 scale normalised to 0–1).
        /// Uses <see cref="BusShift.Core.SanitySystem.ReduceTension"/> so the value is
        /// safely clamped by the system.
        /// </remarks>
        protected override void OnObservationComplete()
        {
            _observationTimer = 0f;
            _isBeingObserved  = false;

            Despawn();

            BusShift.Core.GameManager.Instance?.SanitySystem?.ReduceTension(0.05f);
        }
    }
}
