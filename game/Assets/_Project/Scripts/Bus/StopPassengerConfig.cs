using UnityEngine;
using System.Collections.Generic;

namespace BusShift.Bus
{
    // ─────────────────────────────────────────────────────────────────────────
    //  StopPassengerConfig
    // ─────────────────────────────────────────────────────────────────────────

    /// <summary>
    /// ScriptableObject that declares which passengers board and alight at a
    /// specific bus stop during a route.
    ///
    /// <para>
    /// Create an asset via the Unity menu:
    /// <b>Assets → Create → BusShift → Stop Passenger Config</b>.
    /// </para>
    ///
    /// <para>
    /// <b>Workflow:</b>
    /// <list type="number">
    ///   <item>Create one <see cref="StopPassengerConfig"/> asset per stop that
    ///         has passenger activity.</item>
    ///   <item>Set <see cref="StopIndex"/> to match the waypoint index used by
    ///         <see cref="RouteManager"/>.</item>
    ///   <item>Populate <see cref="BoardingPassengerIds"/> with the
    ///         <see cref="PassengerData.Id"/> values of children who board
    ///         here.</item>
    ///   <item>Populate <see cref="AlightingPassengerIds"/> with IDs of children
    ///         who leave here.</item>
    ///   <item>Assign all configs to the <see cref="PassengerManager"/> array in
    ///         the Inspector.</item>
    /// </list>
    /// </para>
    /// </summary>
    [CreateAssetMenu(fileName = "StopConfig", menuName = "BusShift/Stop Passenger Config")]
    public class StopPassengerConfig : ScriptableObject
    {
        // ── Inspector ─────────────────────────────────────────────────────────

        /// <summary>
        /// The waypoint index (as defined in <see cref="RouteManager.Waypoints"/>)
        /// that this configuration applies to.
        /// </summary>
        [Tooltip("Waypoint index (RouteManager.Waypoints) for this stop.")]
        public int StopIndex;

        /// <summary>
        /// <see cref="PassengerData.Id"/> values of passengers who board the bus
        /// at this stop. Processed in order; boarding is skipped when the bus is
        /// full (12 seats).
        /// </summary>
        [Tooltip("PassengerData.Id values of passengers who board at this stop.")]
        public List<string> BoardingPassengerIds = new List<string>();

        /// <summary>
        /// <see cref="PassengerData.Id"/> values of passengers who alight the bus
        /// at this stop. Alighting is processed before boarding so vacated seats
        /// can be reused in the same stop event.
        /// </summary>
        [Tooltip("PassengerData.Id values of passengers who alight at this stop.")]
        public List<string> AlightingPassengerIds = new List<string>();
    }
}
