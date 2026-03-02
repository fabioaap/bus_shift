using System;
using System.Collections.Generic;
using UnityEngine;

namespace BusShift.Bus
{
    // ─────────────────────────────────────────────────────────────────────────
    //  Data
    // ─────────────────────────────────────────────────────────────────────────

    /// <summary>
    /// Represents a single point on the bus route.
    /// </summary>
    [Serializable]
    public class WaypointData
    {
        /// <summary>World-space position of this waypoint.</summary>
        public Vector3 Position;

        /// <summary>Display name shown in the UI and events.</summary>
        public string Name = "Waypoint";

        /// <summary>When true, this waypoint is a passenger stop.</summary>
        public bool IsStop;
    }

    // ─────────────────────────────────────────────────────────────────────────
    //  RouteManager
    // ─────────────────────────────────────────────────────────────────────────

    /// <summary>
    /// Manages the bus route as an ordered list of <see cref="WaypointData"/>.
    /// Tracks which waypoint is next, fires approach/arrival events, and
    /// exposes proximity helpers used by <see cref="BusStopSystem"/>.
    /// </summary>
    public class RouteManager : MonoBehaviour
    {
        // ── Inspector ────────────────────────────────────────────────────────

        [Header("Bus Reference")]
        [Tooltip("Transform of the bus. Leave null to use this GameObject.")]
        public Transform BusTransform;

        [Header("Route")]
        [Tooltip("Ordered list of waypoints that form the route.")]
        public List<WaypointData> Waypoints = new List<WaypointData>();

        [Header("Thresholds")]
        [Tooltip("Distance (m) at which the bus is considered to have reached a waypoint.")]
        public float WaypointReachDistance = 5f;

        [Tooltip("Distance (m) at which the bus is considered to be approaching a stop.")]
        public float StopApproachDistance = 10f;

        // ── Public state ─────────────────────────────────────────────────────

        /// <summary>Index of the waypoint the bus is currently heading toward.</summary>
        public int CurrentWaypointIndex { get; private set; }

        /// <summary>True when the bus is within <see cref="StopApproachDistance"/> of the next stop.</summary>
        public bool IsNearStop { get; private set; }

        /// <summary>Straight-line distance to the next waypoint marked as a stop.</summary>
        public float DistanceToNextStop { get; private set; } = float.MaxValue;

        // ── Events ────────────────────────────────────────────────────────────

        /// <summary>
        /// Fired when the bus enters <see cref="StopApproachDistance"/> of a stop.
        /// Parameters: stop waypoint index, stop name.
        /// </summary>
        public static event Action<int, string> OnApproachingStop;

        /// <summary>
        /// Fired when the bus reaches (within <see cref="WaypointReachDistance"/> of) a waypoint.
        /// Parameter: waypoint index.
        /// </summary>
        public static event Action<int> OnWaypointReached;

        // ── Private ───────────────────────────────────────────────────────────

        private bool _approachingStopFired;

        // ── Unity lifecycle ───────────────────────────────────────────────────

        private void Awake()
        {
            if (BusTransform == null)
                BusTransform = transform;
        }

        private void Update()
        {
            if (Waypoints == null || Waypoints.Count == 0) return;

            UpdateDistanceToNextStop();
            CheckWaypointProximity();
            CheckStopProximity();
        }

        // ── Private helpers ───────────────────────────────────────────────────

        /// <summary>Advances <see cref="CurrentWaypointIndex"/> when the bus arrives at the current target.</summary>
        private void CheckWaypointProximity()
        {
            if (CurrentWaypointIndex >= Waypoints.Count) return;

            float dist = Vector3.Distance(BusTransform.position, Waypoints[CurrentWaypointIndex].Position);
            if (dist > WaypointReachDistance) return;

            int reached = CurrentWaypointIndex;
            CurrentWaypointIndex++;
            _approachingStopFired = false;          // ready to fire for the next stop

            OnWaypointReached?.Invoke(reached);
        }

        /// <summary>
        /// Fires <see cref="OnApproachingStop"/> once per stop when the bus
        /// comes within <see cref="StopApproachDistance"/>.
        /// </summary>
        private void CheckStopProximity()
        {
            int nextStopIdx = FindNextStopIndex();

            if (nextStopIdx < 0)
            {
                IsNearStop = false;
                return;
            }

            float dist = Vector3.Distance(BusTransform.position, Waypoints[nextStopIdx].Position);
            IsNearStop = dist <= StopApproachDistance;

            if (IsNearStop && !_approachingStopFired)
            {
                _approachingStopFired = true;
                OnApproachingStop?.Invoke(nextStopIdx, Waypoints[nextStopIdx].Name);
            }
        }

        /// <summary>Recalculates <see cref="DistanceToNextStop"/> every frame.</summary>
        private void UpdateDistanceToNextStop()
        {
            int nextStopIdx = FindNextStopIndex();

            DistanceToNextStop = nextStopIdx >= 0
                ? Vector3.Distance(BusTransform.position, Waypoints[nextStopIdx].Position)
                : float.MaxValue;
        }

        /// <summary>
        /// Returns the index of the first stop waypoint at or after
        /// <see cref="CurrentWaypointIndex"/>, or -1 if none remain.
        /// </summary>
        private int FindNextStopIndex()
        {
            for (int i = CurrentWaypointIndex; i < Waypoints.Count; i++)
            {
                if (Waypoints[i].IsStop) return i;
            }
            return -1;
        }

        // ── Gizmos ────────────────────────────────────────────────────────────

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (Waypoints == null) return;

            for (int i = 0; i < Waypoints.Count; i++)
            {
                WaypointData wp = Waypoints[i];
                Gizmos.color = wp.IsStop ? Color.red : Color.yellow;
                Gizmos.DrawWireSphere(wp.Position, 1f);

                if (i < Waypoints.Count - 1)
                {
                    Gizmos.color = Color.white;
                    Gizmos.DrawLine(wp.Position, Waypoints[i + 1].Position);
                }

                // Draw approach radius for stops
                if (wp.IsStop)
                {
                    Gizmos.color = new Color(1f, 0f, 0f, 0.15f);
                    Gizmos.DrawWireSphere(wp.Position, StopApproachDistance);
                }
            }
        }
#endif
    }
}
