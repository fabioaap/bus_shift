using UnityEngine;
using BusShift.Interventions;
using BusShift.Ghosts;

namespace BusShift.Core
{
    /// <summary>
    /// Bridges ghost state machines with their runtime integration points.
    ///
    /// Resolves three cross-system dependencies without coupling the
    /// Interventions or Bus namespaces directly to the Ghosts namespace:
    ///
    ///   1. RadioSystem.IsPlaying  → ThomasGhost.RadioIsPlaying
    ///   2. TimerSystem.IsLate     → OliverGhost.BusIsLate
    ///   3. Bus collision events   → GraceGhost.OnBusCollided
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

        private void Awake()
        {
            if (_radioSystem == null)
                _radioSystem = FindObjectOfType<RadioSystem>();

            if (_timerSystem == null)
                _timerSystem = FindObjectOfType<TimerSystem>();

            if (_collisionReporter == null)
                _collisionReporter = FindObjectOfType<BusCollisionReporter>();

            if (_collisionReporter != null)
                _collisionReporter.OnCollision += HandleBusCollision;
        }

        private void OnDestroy()
        {
            if (_collisionReporter != null)
                _collisionReporter.OnCollision -= HandleBusCollision;
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
            GraceGhost.OnBusCollided?.Invoke();
        }
    }
}
