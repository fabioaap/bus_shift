using System;
using UnityEngine;

namespace BusShift.Ghosts
{
    /// <summary>
    /// Emma – "The Trickster"  ⚠️ CRITICAL — only 2 seconds to react!
    ///
    /// Emma appears beside the driver (right side) and immediately triggers the attack
    /// with a 2-second window. The player must activate PanelLock (SHIFT) before
    /// the window expires or it is Game Over.
    ///
    /// Laugh intensity progresses automatically across the 2-second window:
    ///   0 = soft "hihihi"  |  1 = medium  |  2 = full shriek
    ///
    /// Wire-up:
    ///   • Assign <see cref="_driverSeatRightPosition"/> in the Inspector (Transform near driver's right).
    ///   • Call <see cref="Defeat"/> from your InputHandler when the player uses PanelLock / SHIFT.
    ///   • Subscribe to <see cref="OnLaugh"/> to play audio clips per intensity level.
    ///   • Subscribe to <see cref="GhostBase.OnAttackStarted"/> to show Emma's sprite/model.
    ///   • Call <see cref="SetDayDifficulty"/> once per day from DayManager.
    /// </summary>
    public class EmmaGhost : GhostBase
    {
        // ── Inspector ─────────────────────────────────────────────────────────
        [Header("Emma – The Trickster  ⚠️ CRITICAL: 2-second window")]
        [Tooltip("Transform positioned to Emma's spawn point (driver's right side).")]
        [SerializeField] private Transform _driverSeatRightPosition;

        [Tooltip("Cooldown between appearances (seconds). Total gap = this + 2 s base delay.")]
        [SerializeField] private float _baseCooldown = 18f; // ~20 s total with 2 s defeat delay

        // ── Events ────────────────────────────────────────────────────────────
        /// <summary>
        /// Fired whenever Emma's laugh intensity steps up.
        /// Values: 0 (soft "hihihi"), 1 (medium), 2 (intense shriek).
        /// </summary>
        public static event Action<int> OnLaugh;

        // ── Runtime state ─────────────────────────────────────────────────────
        private int _laughIntensity;

        /// <summary>Current laugh intensity: 0 (soft), 1 (medium), 2 (intense).</summary>
        public int LaughIntensity => _laughIntensity;

        // ── Lifecycle ─────────────────────────────────────────────────────────
        private void Awake()
        {
            GhostType      = GhostType.Emma;
            AttackWindow   = 2f;          // ⚠️ Only 2 seconds!
            AttackInterval = _baseCooldown;
        }

        protected override void Update()
        {
            base.Update(); // handles Idle countdown → Activate, Attacking countdown → GameOver

            // Track laugh intensity progression during the attack window
            if (CurrentState == GhostState.Attacking)
                UpdateLaughIntensity();
        }

        // ── GhostBase abstract methods ────────────────────────────────────────
        protected override void OnActivate()
        {
            // Snap Emma to the driver's right side
            if (_driverSeatRightPosition != null)
                transform.position = _driverSeatRightPosition.position;

            // Reset laugh to silence before the attack fires
            _laughIntensity = 0;
            OnLaugh?.Invoke(_laughIntensity);

            // Emma skips the Active grace period — attack starts the instant she appears
            BeginAttack();
        }

        protected override void OnAttack()
        {
            // The attack window (2 s) begins here.
            // GhostBase.OnAttackStarted event signals listeners to show Emma's sprite.
        }

        protected override void OnDefeated()
        {
            // Emma vanishes; laughter resets.
            _laughIntensity = 0;
            // GhostBase.Defeat() schedules ResetToIdle after 2 s → AttackInterval cooldown.
        }

        // ── Laugh intensity ───────────────────────────────────────────────────
        private void UpdateLaughIntensity()
        {
            // Divide the 2-second window into 3 equal segments (≈ 0.67 s each).
            // _windowTimer counts down from AttackWindow → 0, so elapsed = AttackWindow - _windowTimer.
            float elapsed      = AttackWindow - _windowTimer;
            float segmentSize  = AttackWindow / 3f;
            int   newIntensity = Mathf.Clamp(Mathf.FloorToInt(elapsed / segmentSize), 0, 2);

            if (newIntensity != _laughIntensity)
            {
                _laughIntensity = newIntensity;
                OnLaugh?.Invoke(_laughIntensity);
            }
        }

        // ── Difficulty scaling ────────────────────────────────────────────────
        /// <summary>
        /// Adjusts difficulty based on the current day (1-based).
        /// Emma appears more frequently with each passing day.
        /// </summary>
        public void SetDayDifficulty(int day)
        {
            int dayIndex   = Mathf.Max(0, day - 1);
            AttackInterval = Mathf.Max(8f, _baseCooldown - (dayIndex * 2f));
        }
    }
}
