using System;
using UnityEngine;

namespace BusShift.Bus
{
    // ─────────────────────────────────────────────────────────────────────────
    //  PassengerData
    // ─────────────────────────────────────────────────────────────────────────

    /// <summary>
    /// Serialisable data-transfer object that stores the complete runtime state
    /// of a single passenger (child NPC) for the current bus route.
    ///
    /// <para>
    /// <b>Ghost flag:</b> when <see cref="IsGhost"/> is <c>true</c> the passenger
    /// is a supernatural entity that appears mysteriously in a seat and is NOT
    /// counted toward the delivery total.
    /// </para>
    ///
    /// <para>
    /// Seat indices range from <c>0</c> to <c>11</c> (12 seats per bus).
    /// A value of <c>-1</c> means no seat has been assigned yet.
    /// </para>
    /// </summary>
    [Serializable]
    public class PassengerData
    {
        // ── Identity ──────────────────────────────────────────────────────────

        /// <summary>
        /// Unique identifier for this passenger (e.g. <c>"child_01"</c>).
        /// Used as the primary key for lookup and event dispatch.
        /// </summary>
        public string Id;

        /// <summary>
        /// Human-readable name shown in the UI and debug logs
        /// (e.g. <c>"Ana"</c>).
        /// </summary>
        public string DisplayName;

        // ── Seat ──────────────────────────────────────────────────────────────

        /// <summary>
        /// Index of the seat currently occupied by this passenger.
        /// Range: <c>0–11</c>. A value of <c>-1</c> means unassigned.
        /// </summary>
        public int SeatIndex = -1;

        // ── Classification ────────────────────────────────────────────────────

        /// <summary>
        /// When <c>false</c>, this is a real living child who must be collected
        /// and delivered. When <c>true</c>, the passenger is a ghost NPC that
        /// appears in seats without a manifest entry and is excluded from the
        /// delivery counter.
        /// </summary>
        public bool IsGhost;

        // ── Route state ───────────────────────────────────────────────────────

        /// <summary>
        /// Set to <c>true</c> by <see cref="PassengerManager.BoardPassenger"/>
        /// once the passenger has boarded the bus at their stop.
        /// </summary>
        public bool HasBoarded;

        /// <summary>
        /// Set to <c>true</c> by <see cref="PassengerManager.AlightPassenger"/>
        /// once the passenger has left the bus at their destination.
        /// </summary>
        public bool HasAlighted;
    }
}
