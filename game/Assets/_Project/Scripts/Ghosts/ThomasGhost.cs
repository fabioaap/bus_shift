using System;
using UnityEngine;

namespace BusShift.Ghosts
{
    /// <summary>
    /// Thomas – "The Narrator"
    ///
    /// Whispers from the back of the bus, progressing through three escalating phases.
    /// If not neutralized by the Radio before Phase 2 ends, Game Over is triggered.
    ///
    /// Phase timeline (configurable in Inspector):
    ///   Phase 0 (0 → phase1Duration):    Soft whispers  → adds flat tension once
    ///   Phase 1 (phase1Duration → +phase2Duration):  Loud whispers  → adds tension per second
    ///   Phase 2 (after phase1+phase2 elapsed):        → Game Over
    ///
    /// Thomas does NOT use the standard <see cref="GhostBase.BeginAttack"/> flow.
    /// He manages his own timer and calls <see cref="GhostBase.TriggerGameOver"/> directly.
    ///
    /// Wire-up:
    ///   • Set <see cref="RadioIsPlaying"/> = true/false from your RadioSystem each frame.
    ///     e.g. in RadioSystem.Update: <c>ThomasGhost.RadioIsPlaying = IsPlaying;</c>
    ///   • Subscribe to <see cref="OnPhaseChanged"/> to trigger audio / visual feedback.
    ///   • Subscribe to <see cref="GhostBase.OnAttackDefeated"/> to confirm radio neutralisation.
    ///   • Call <see cref="SetDayDifficulty"/> once per day from DayManager.
    /// </summary>
    public class ThomasGhost : GhostBase
    {
        // ── Inspector ─────────────────────────────────────────────────────────
        [Header("Thomas – The Narrator")]
        [Tooltip("Duration of Phase 0 (soft whispers), in seconds.")]
        [SerializeField] private float _phase1Duration          = 4f;

        [Tooltip("Duration of Phase 1 (loud whispers), in seconds. Phase 2 = Game Over.")]
        [SerializeField] private float _phase2Duration          = 4f;

        [Tooltip("One-time tension added when Phase 0 begins.")]
        [SerializeField] private float _phase1TensionFlat       = 0.1f;

        [Tooltip("Tension added per second during Phase 1.")]
        [SerializeField] private float _phase2TensionPerSecond  = 0.2f;

        [Tooltip("Cooldown before Thomas reappears after being defeated (approx, adjusted for 2 s base delay).")]
        [SerializeField] private float _baseCooldown            = 23f; // 23 s + 2 s ≈ 25 s total

        // ── Static integration point ──────────────────────────────────────────
        /// <summary>
        /// Set this to <c>true</c> while the RadioSystem is actively playing.
        /// Thomas will be immediately defeated (neutralised) when this is true during his Active state.
        ///
        /// <code>
        /// // Inside RadioSystem.Update():
        /// ThomasGhost.RadioIsPlaying = IsPlaying;
        /// </code>
        /// </summary>
        public static bool RadioIsPlaying { get; set; }

        // ── Events ────────────────────────────────────────────────────────────
        /// <summary>
        /// Fired when Thomas transitions between whisper phases.
        /// Values: 0 (soft), 1 (loud), 2 (game-over threshold reached).
        /// </summary>
        public static event Action<int> OnPhaseChanged;

        // ── Runtime state ─────────────────────────────────────────────────────
        private float _phaseTimer;
        private int   _whisperPhase;
        private bool  _phase1TensionApplied;

        /// <summary>Current whisper phase: 0 (soft), 1 (loud), 2 (game-over).</summary>
        public int WhisperPhase => _whisperPhase;

        // ── Lifecycle ─────────────────────────────────────────────────────────
        private void Awake()
        {
            GhostType      = GhostType.Thomas;
            AttackInterval = _baseCooldown;
        }

        protected override void Update()
        {
            // base.Update handles: Idle countdown → Activate.
            // Thomas never enters GhostState.Attacking, so that branch is never reached.
            base.Update();

            if (CurrentState == GhostState.Active)
            {
                // The Radio neutralises Thomas immediately
                if (RadioIsPlaying)
                {
                    Defeat();
                    return;
                }

                _phaseTimer += Time.deltaTime;
                EvaluatePhase();
            }
        }

        // ── GhostBase abstract methods ────────────────────────────────────────
        protected override void OnActivate()
        {
            IsVulnerable          = true;
            _phaseTimer           = 0f;
            _whisperPhase         = 0;
            _phase1TensionApplied = false;
            OnPhaseChanged?.Invoke(0);
        }

        /// <summary>Thomas does not use the standard BeginAttack window.</summary>
        protected override void OnAttack() { /* unused — Thomas bypasses BeginAttack */ }

        protected override void OnDefeated()
        {
            IsVulnerable  = false;
            _phaseTimer   = 0f;
            _whisperPhase = 0;
            // GhostBase.Defeat() schedules ResetToIdle after 2 s → _baseCooldown before next appearance.
        }

        // ── Phase evaluation ──────────────────────────────────────────────────
        private void EvaluatePhase()
        {
            float phase2Start  = _phase1Duration;
            float gameOverTime = _phase1Duration + _phase2Duration;

            if (_phaseTimer < phase2Start)
            {
                // ── Phase 0: Soft whispers ────────────────────────────────────
                if (_whisperPhase != 0)
                    TransitionToPhase(0);

                // Apply one-time flat tension when this phase begins
                if (!_phase1TensionApplied)
                {
                    _phase1TensionApplied = true;
                    AddTension(_phase1TensionFlat);
                }
            }
            else if (_phaseTimer < gameOverTime)
            {
                // ── Phase 1: Loud whispers, continuous tension ─────────────────
                if (_whisperPhase != 1)
                    TransitionToPhase(1);

                AddTension(_phase2TensionPerSecond * Time.deltaTime);
            }
            else
            {
                // ── Phase 2: Game Over threshold reached ──────────────────────
                if (_whisperPhase != 2)
                {
                    TransitionToPhase(2);
                    TriggerGameOver();
                }
            }
        }

        private void TransitionToPhase(int phase)
        {
            _whisperPhase = phase;
            OnPhaseChanged?.Invoke(phase);
        }

        private static void AddTension(float amount)
        {
            BusShift.Core.GameManager.Instance?.SanitySystem?.AddTension(amount);
        }

        // ── Difficulty scaling ────────────────────────────────────────────────
        /// <summary>
        /// Adjusts difficulty based on the current day (1-based).
        /// Phase durations shrink and tension rates increase with each day.
        /// </summary>
        public void SetDayDifficulty(int day)
        {
            int dayIndex           = Mathf.Max(0, day - 1);
            AttackInterval         = Mathf.Max(10f, _baseCooldown - (dayIndex * 2f));
            _phase1Duration        = Mathf.Max(2f, 4f - (dayIndex * 0.3f));
            _phase2Duration        = Mathf.Max(2f, 4f - (dayIndex * 0.3f));
            _phase2TensionPerSecond = 0.2f + (dayIndex * 0.02f);
        }
    }
}
