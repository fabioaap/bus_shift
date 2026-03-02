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
    /// <b>Observation mechanic:</b>
    /// <list type="bullet">
    ///   <item>
    ///     If the player observes Emma <i>before</i> she fully manifests
    ///     (<see cref="HasFullyManifested"/> == <c>false</c>) she vanishes but the
    ///     player is penalised (−0.10 sanity / −10 pts on a 0–100 scale).
    ///   </item>
    ///   <item>
    ///     Once fully manifested (<see cref="HasFullyManifested"/> == <c>true</c>),
    ///     sustained observation causes Emma to retreat (+0.05 sanity / +5 pts).
    ///   </item>
    /// </list>
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

        [Tooltip("Seconds after activation before Emma fully manifests and begins the attack window. " +
                 "During this window observing Emma carries a sanity penalty instead of a reward.")]
        [SerializeField] private float _manifestationDelay = 1.5f;

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

        /// <summary>Counts down from <see cref="_manifestationDelay"/> to zero before BeginAttack.</summary>
        private float _manifestationTimer;

        /// <summary>
        /// <c>true</c> once Emma has fully manifested (i.e. the attack window has started).
        /// Before this flag is set, observing Emma causes a sanity penalty; afterwards
        /// sustained observation triggers a retreat and sanity reward.
        /// </summary>
        public bool HasFullyManifested { get; private set; }

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

            // Manifestation delay: tick down in Active state before beginning the attack.
            if (CurrentState == GhostState.Active)
            {
                _manifestationTimer -= Time.deltaTime;
                if (_manifestationTimer <= 0f)
                {
                    HasFullyManifested = true;
                    BeginAttack();
                }
            }

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

            // Reset state for this appearance
            _laughIntensity    = 0;
            HasFullyManifested = false;
            _manifestationTimer = _manifestationDelay;
            OnLaugh?.Invoke(_laughIntensity);

            // Emma enters Active state with a short manifestation window before the attack fires.
            // (BeginAttack is called by Update once _manifestationTimer expires.)
        }

        protected override void OnAttack()
        {
            // The attack window (2 s) begins here.
            // GhostBase.OnAttackStarted event signals listeners to show Emma's sprite.
        }

        protected override void OnDefeated()
        {
            // Emma vanishes; laughter and manifestation state reset.
            _laughIntensity    = 0;
            HasFullyManifested = false;
            // GhostBase.Defeat() schedules ResetToIdle after 2 s → AttackInterval cooldown.
        }

        // ── Observation ───────────────────────────────────────────────────────
        /// <summary>
        /// Observation outcome depends on Emma's manifestation stage:
        /// <list type="bullet">
        ///   <item>
        ///     <b>Not yet fully manifested:</b> Emma vanishes but frightens the driver.
        ///     Sanity penalty: −0.10 (equivalent to −10 pts on a 0–100 normalised scale).
        ///   </item>
        ///   <item>
        ///     <b>Already fully manifested:</b> Sustained observation drives Emma back.
        ///     Sanity reward: +0.05 (equivalent to +5 pts on a 0–100 normalised scale).
        ///   </item>
        /// </list>
        /// </summary>
        protected override void OnObservationComplete()
        {
            _observationTimer = 0f;
            _isBeingObserved  = false;

            if (!HasFullyManifested)
            {
                // Player caught Emma materialising — she vanishes but causes a jumpscare penalty.
                Despawn();
                BusShift.Core.GameManager.Instance?.SanitySystem?.AddTension(0.10f);
            }
            else
            {
                // Player stared down fully-manifested Emma — she retreats.
                Despawn();
                BusShift.Core.GameManager.Instance?.SanitySystem?.ReduceTension(0.05f);
            }
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
