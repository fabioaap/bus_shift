using UnityEngine;
using BusShift.Bus;
using BusShift.Interventions;
using BusShift.Ghosts;

namespace BusShift.Core
{
    /// <summary>
    /// Bridges ghost state machines with their runtime integration points.
    ///
    /// Resolves four cross-system dependencies without coupling the
    /// Interventions or Bus namespaces directly to the Ghosts namespace:
    ///
    ///   1. RadioSystem.IsPlaying          → ThomasGhost.RadioIsPlaying
    ///   2. TimerSystem.IsLate             → OliverGhost.BusIsLate
    ///   3. Bus collision events           → GraceGhost.OnBusCollided
    ///   4. DayManager.OnDayDifficultyChanged → all GhostBase.SetDayDifficulty
    ///
    /// Attach to the same GameObject as GameManager (or any persistent object in the scene).
    /// </summary>
    public class GhostIntegrationBridge : MonoBehaviour
    {
        [Header("References")]
        [Tooltip("RadioSystem component in the scene. Auto-found if not assigned.")]
        [SerializeField] private RadioSystem _radioSystem;

        [Tooltip("TimerSystem component in the scene. Auto-found if not assigned.")]
        [SerializeField] private TimerSystem _timerSystem;

        [Tooltip("BusController or any bus-root GameObject whose OnCollisionEnter should notify Grace.")]
        [SerializeField] private BusCollisionReporter _collisionReporter;

        // Kept as a reference; the event itself is static on DayManager.
        private DayManager _dayManager;

        private void Awake()
        {
            // Unity 6: use FindAnyObjectByType for non-deterministic single-result lookups.
            if (_radioSystem == null)
                _radioSystem = FindAnyObjectByType<RadioSystem>();

            if (_timerSystem == null)
                _timerSystem = FindAnyObjectByType<TimerSystem>();

            if (_collisionReporter == null)
                _collisionReporter = FindAnyObjectByType<BusCollisionReporter>();

            if (_collisionReporter != null)
                _collisionReporter.OnCollision += HandleBusCollision;

            WireDayDifficulty();
        }

        private void OnDestroy()
        {
            if (_collisionReporter != null)
                _collisionReporter.OnCollision -= HandleBusCollision;

            // DayManager.OnDayDifficultyChanged is static; unsubscribe regardless of instance.
            DayManager.OnDayDifficultyChanged -= HandleDayDifficultyChanged;
        }

        private void Update()
        {
            // 1. Radio → Thomas
            if (_radioSystem != null)
                ThomasGhost.RadioIsPlaying = _radioSystem.IsPlaying;

            // 2. Timer → Oliver
            if (_timerSystem != null)
                OliverGhost.BusIsLate = _timerSystem.IsLate;
        }

        // 3. Grace: collision forwarding
        private void HandleBusCollision()
        {
            GraceGhost.NotifyBusCollision();
        }

        // ─────────────────────────────────────────────────────────────────────
        //  Day difficulty wiring
        // ─────────────────────────────────────────────────────────────────────

        /// <summary>
        /// Locates the <see cref="DayManager"/> in the scene and subscribes to
        /// <see cref="DayManager.OnDayDifficultyChanged"/> so all active ghosts
        /// have their difficulty scaled whenever a new period begins.
        /// </summary>
        private void WireDayDifficulty()
        {
            _dayManager = FindAnyObjectByType<DayManager>();

            // The event is static; subscribing via type name avoids CS0176 warning.
            DayManager.OnDayDifficultyChanged += HandleDayDifficultyChanged;

#if UNITY_EDITOR || DEVELOPMENT_BUILD
            if (_dayManager == null)
                Debug.LogWarning("[GhostIntegrationBridge] DayManager not found in scene — " +
                                 "day-difficulty events will be ignored.");
#endif
        }

        /// <summary>
        /// Propagates the current day number to every active ghost so each can
        /// scale its own attack parameters accordingly.
        /// </summary>
        /// <param name="day">Current day (1–5).</param>
        private void HandleDayDifficultyChanged(int day)
        {
            var ghosts = FindObjectsByType<GhostBase>(FindObjectsSortMode.None);
            foreach (var ghost in ghosts)
                DispatchSetDayDifficulty(ghost, day);
        }

        /// <summary>
        /// Type-safe dispatch of <c>SetDayDifficulty</c> to concrete ghost types.
        /// GhostBase does not expose the method (each ghost scales uniquely),
        /// so pattern-matching is used instead of a virtual call.
        /// </summary>
        private static void DispatchSetDayDifficulty(GhostBase ghost, int day)
        {
            switch (ghost)
            {
                case ThomasGhost t: t.SetDayDifficulty(day); break;
                case OliverGhost o: o.SetDayDifficulty(day); break;
                case EmmaGhost   e: e.SetDayDifficulty(day); break;
                case GraceGhost  g: g.SetDayDifficulty(day); break;
                case MarcusGhost m: m.SetDayDifficulty(day); break;
            }
        }
    }
}
