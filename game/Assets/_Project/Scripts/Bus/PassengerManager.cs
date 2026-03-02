using System;
using System.Collections.Generic;
using UnityEngine;

namespace BusShift.Bus
{
    // ─────────────────────────────────────────────────────────────────────────
    //  PassengerManager
    // ─────────────────────────────────────────────────────────────────────────

    /// <summary>
    /// Singleton <see cref="MonoBehaviour"/> that owns the authoritative list of
    /// <see cref="PassengerData"/> objects for the current route and drives all
    /// passenger lifecycle events.
    ///
    /// <para>
    /// <b>Boarding / alighting flow:</b>
    /// <list type="number">
    ///   <item>Call <see cref="InitializeRoute"/> at the start of each route to
    ///         register the full manifest of expected passengers.</item>
    ///   <item>When the bus doors open at a stop, <see cref="BusStopSystem"/>
    ///         fires <see cref="BusStopSystem.OnStopReached"/>.</item>
    ///   <item><see cref="PassengerManager"/> looks up the matching
    ///         <see cref="StopPassengerConfig"/> and calls
    ///         <see cref="AlightPassenger"/> then <see cref="BoardPassenger"/>
    ///         for each listed ID.</item>
    ///   <item>When every live (non-ghost) passenger has both boarded and
    ///         alighted, <see cref="OnAllPassengersDelivered"/> fires.</item>
    /// </list>
    /// </para>
    ///
    /// <para>
    /// <b>Ghost passengers</b> may be injected via <see cref="InitializeRoute"/>
    /// with <see cref="PassengerData.IsGhost"/> = <c>true</c>. They trigger the
    /// same board/alight events but are excluded from the delivery counter.
    /// </para>
    /// </summary>
    public class PassengerManager : MonoBehaviour
    {
        // ─────────────────────────────────────────────────────────────────────
        //  Singleton
        // ─────────────────────────────────────────────────────────────────────

        /// <summary>The singleton instance of <see cref="PassengerManager"/>.</summary>
        public static PassengerManager Instance { get; private set; }

        // ─────────────────────────────────────────────────────────────────────
        //  Inspector
        // ─────────────────────────────────────────────────────────────────────

        [Header("Stop Configurations")]
        [Tooltip("One StopPassengerConfig asset per stop that has passenger activity.")]
        [SerializeField] private StopPassengerConfig[] _stopConfigs;

        // ─────────────────────────────────────────────────────────────────────
        //  Constants
        // ─────────────────────────────────────────────────────────────────────

        /// <summary>Total number of available seats on the bus.</summary>
        private const int BusSeatCount = 12;

        // ─────────────────────────────────────────────────────────────────────
        //  Private state
        // ─────────────────────────────────────────────────────────────────────

        /// <summary>Full manifest for the current route.</summary>
        private List<PassengerData> _passengers = new List<PassengerData>();

        // ─────────────────────────────────────────────────────────────────────
        //  Public properties
        // ─────────────────────────────────────────────────────────────────────

        /// <summary>
        /// Number of living (non-ghost) passengers currently on board:
        /// <see cref="PassengerData.HasBoarded"/> is <c>true</c> and
        /// <see cref="PassengerData.HasAlighted"/> is <c>false</c>.
        /// </summary>
        public int LivePassengerCount
        {
            get
            {
                int count = 0;
                foreach (var p in _passengers)
                {
                    if (!p.IsGhost && p.HasBoarded && !p.HasAlighted)
                        count++;
                }
                return count;
            }
        }

        /// <summary>
        /// Total number of live (non-ghost) passengers registered for the
        /// current route via <see cref="InitializeRoute"/>. Set to zero
        /// between routes.
        /// </summary>
        public int TotalLivePassengers { get; private set; }

        // ─────────────────────────────────────────────────────────────────────
        //  Events
        // ─────────────────────────────────────────────────────────────────────

        /// <summary>
        /// Fired when a passenger successfully boards the bus.
        /// Parameter: the <see cref="PassengerData"/> that boarded.
        /// </summary>
        public static event Action<PassengerData> OnPassengerBoarded;

        /// <summary>
        /// Fired when a passenger successfully alights from the bus.
        /// Parameter: the <see cref="PassengerData"/> that alighted.
        /// </summary>
        public static event Action<PassengerData> OnPassengerAlighted;

        /// <summary>
        /// Fired once when every live (non-ghost) passenger in the current route
        /// manifest has both boarded and alighted — i.e. all children have been
        /// safely delivered.
        /// </summary>
        public static event Action OnAllPassengersDelivered;

        /// <summary>
        /// Fired by <see cref="InitializeRoute"/> immediately after the passenger
        /// manifest is loaded. Parameter: total number of live passengers for
        /// this route (ghost passengers excluded).
        /// </summary>
        public static event Action<int> OnRouteInitialized;

        // ─────────────────────────────────────────────────────────────────────
        //  Unity lifecycle
        // ─────────────────────────────────────────────────────────────────────

        private void Awake()
        {
            // Enforce singleton — destroy duplicate instances.
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void OnEnable()
        {
            BusStopSystem.OnStopReached += HandleStopReached;
        }

        private void OnDisable()
        {
            BusStopSystem.OnStopReached -= HandleStopReached;
        }

        // ─────────────────────────────────────────────────────────────────────
        //  Public API
        // ─────────────────────────────────────────────────────────────────────

        /// <summary>
        /// Loads the passenger manifest for a new route, resetting all
        /// boarding / alighting state.
        ///
        /// <para>Call this at the beginning of each route or game day before
        /// the bus departs.</para>
        /// </summary>
        /// <param name="passengers">
        /// The complete list of <see cref="PassengerData"/> for the route,
        /// including any ghost passengers. Pass <c>null</c> to clear the
        /// manifest.
        /// </param>
        public void InitializeRoute(List<PassengerData> passengers)
        {
            _passengers = passengers ?? new List<PassengerData>();

            TotalLivePassengers = 0;
            foreach (var p in _passengers)
            {
                if (!p.IsGhost) TotalLivePassengers++;
            }

            OnRouteInitialized?.Invoke(TotalLivePassengers);

#if UNITY_EDITOR || DEVELOPMENT_BUILD
            Debug.Log($"[PassengerManager] Route initialised — " +
                      $"{_passengers.Count} total passengers ({TotalLivePassengers} live).");
#endif
        }

        /// <summary>
        /// Marks a passenger as having boarded the bus and assigns them a seat.
        /// Fires <see cref="OnPassengerBoarded"/> on success.
        ///
        /// <para>No-op if the passenger is not found, has already boarded, or
        /// the provided seat index is out of range.</para>
        /// </summary>
        /// <param name="id">The <see cref="PassengerData.Id"/> of the passenger.</param>
        /// <param name="seatIndex">
        /// Zero-based seat index (<c>0–11</c>) to assign to this passenger.
        /// Use <see cref="FindAvailableSeat"/> to auto-select a free seat.
        /// </param>
        public void BoardPassenger(string id, int seatIndex)
        {
            PassengerData passenger = FindById(id);
            if (passenger == null)
            {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
                Debug.LogWarning($"[PassengerManager] BoardPassenger: id '{id}' not found.");
#endif
                return;
            }

            if (passenger.HasBoarded)
            {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
                Debug.LogWarning($"[PassengerManager] BoardPassenger: '{id}' has already boarded.");
#endif
                return;
            }

            passenger.HasBoarded = true;
            passenger.SeatIndex  = seatIndex;

            OnPassengerBoarded?.Invoke(passenger);

#if UNITY_EDITOR || DEVELOPMENT_BUILD
            Debug.Log($"[PassengerManager] Boarded: '{passenger.DisplayName}' → seat {seatIndex}" +
                      (passenger.IsGhost ? " [GHOST]" : string.Empty));
#endif
        }

        /// <summary>
        /// Marks a passenger as having alighted and checks whether all live
        /// passengers in the manifest have now been delivered.
        /// Fires <see cref="OnPassengerAlighted"/> on success, and potentially
        /// <see cref="OnAllPassengersDelivered"/> if delivery is complete.
        ///
        /// <para>No-op if the passenger is not found, has not yet boarded, or
        /// has already alighted.</para>
        /// </summary>
        /// <param name="id">The <see cref="PassengerData.Id"/> of the passenger.</param>
        public void AlightPassenger(string id)
        {
            PassengerData passenger = FindById(id);
            if (passenger == null)
            {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
                Debug.LogWarning($"[PassengerManager] AlightPassenger: id '{id}' not found.");
#endif
                return;
            }

            if (!passenger.HasBoarded)
            {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
                Debug.LogWarning($"[PassengerManager] AlightPassenger: '{id}' has not boarded yet.");
#endif
                return;
            }

            if (passenger.HasAlighted)
            {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
                Debug.LogWarning($"[PassengerManager] AlightPassenger: '{id}' has already alighted.");
#endif
                return;
            }

            passenger.HasAlighted = true;

            OnPassengerAlighted?.Invoke(passenger);

#if UNITY_EDITOR || DEVELOPMENT_BUILD
            Debug.Log($"[PassengerManager] Alighted: '{passenger.DisplayName}'" +
                      (passenger.IsGhost ? " [GHOST]" : string.Empty));
#endif

            CheckAllDelivered();
        }

        /// <summary>
        /// Returns the <see cref="PassengerData"/> of the passenger currently
        /// occupying a given seat, or <c>null</c> if the seat is empty.
        ///
        /// <para>A passenger "occupies" a seat when they have boarded but have
        /// not yet alighted.</para>
        /// </summary>
        /// <param name="seatIndex">Zero-based seat index (<c>0–11</c>).</param>
        /// <returns>
        /// The <see cref="PassengerData"/> at that seat, or <c>null</c>.
        /// </returns>
        public PassengerData GetPassengerAtSeat(int seatIndex)
        {
            foreach (var p in _passengers)
            {
                if (p.HasBoarded && !p.HasAlighted && p.SeatIndex == seatIndex)
                    return p;
            }
            return null;
        }

        // ─────────────────────────────────────────────────────────────────────
        //  Private — stop event handling
        // ─────────────────────────────────────────────────────────────────────

        /// <summary>
        /// Subscribed to <see cref="BusStopSystem.OnStopReached"/>.
        /// Delegates to <see cref="TriggerStopPassengerEvents"/>.
        /// </summary>
        private void HandleStopReached(int stopIndex)
        {
            TriggerStopPassengerEvents(stopIndex);
        }

        /// <summary>
        /// Looks up the <see cref="StopPassengerConfig"/> for <paramref name="stopIndex"/>
        /// and processes alighting first, then boarding, so that vacated seats
        /// can be reused within the same stop visit.
        /// </summary>
        /// <param name="stopIndex">
        /// Waypoint index identifying the current stop, as provided by
        /// <see cref="BusStopSystem.OnStopReached"/>.
        /// </param>
        private void TriggerStopPassengerEvents(int stopIndex)
        {
            if (_stopConfigs == null || _stopConfigs.Length == 0) return;

            foreach (var config in _stopConfigs)
            {
                if (config == null || config.StopIndex != stopIndex) continue;

                // 1. Alighting — free seats before new passengers board.
                foreach (string id in config.AlightingPassengerIds)
                    AlightPassenger(id);

                // 2. Boarding — respect pre-configured seat or auto-assign.
                foreach (string id in config.BoardingPassengerIds)
                {
                    PassengerData p = FindById(id);

                    // Prefer the passenger's pre-assigned seat; fall back to auto.
                    int seat = (p != null && p.SeatIndex >= 0 && p.SeatIndex < BusSeatCount)
                               ? p.SeatIndex
                               : FindAvailableSeat();

                    if (seat < 0)
                    {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
                        Debug.LogWarning($"[PassengerManager] No seat available for '{id}' at stop {stopIndex}.");
#endif
                        continue;
                    }

                    BoardPassenger(id, seat);
                }

                // Only one config per stop index is expected.
                break;
            }
        }

        // ─────────────────────────────────────────────────────────────────────
        //  Private — delivery check
        // ─────────────────────────────────────────────────────────────────────

        /// <summary>
        /// Fires <see cref="OnAllPassengersDelivered"/> when every live
        /// (non-ghost) passenger in the manifest has both boarded and alighted.
        /// Ignores ghost passengers and no-ops on an empty or ghost-only manifest.
        /// </summary>
        private void CheckAllDelivered()
        {
            // Guard: need at least one live passenger in the manifest.
            if (TotalLivePassengers == 0) return;

            foreach (var p in _passengers)
            {
                if (p.IsGhost) continue;

                // A live passenger who has not yet boarded means the route is
                // still in progress.
                if (!p.HasBoarded) return;

                // A live passenger still on board — not delivered yet.
                if (!p.HasAlighted) return;
            }

            OnAllPassengersDelivered?.Invoke();

#if UNITY_EDITOR || DEVELOPMENT_BUILD
            Debug.Log("[PassengerManager] All live passengers delivered! 🎉");
#endif
        }

        // ─────────────────────────────────────────────────────────────────────
        //  Private — helpers
        // ─────────────────────────────────────────────────────────────────────

        /// <summary>
        /// Searches the manifest for a passenger with the given
        /// <see cref="PassengerData.Id"/>.
        /// </summary>
        /// <param name="id">Passenger identifier to look up.</param>
        /// <returns>
        /// The matching <see cref="PassengerData"/>, or <c>null</c> if not found.
        /// </returns>
        private PassengerData FindById(string id)
        {
            foreach (var p in _passengers)
            {
                if (p.Id == id) return p;
            }
            return null;
        }

        /// <summary>
        /// Finds the lowest-indexed seat that is not currently occupied by a
        /// boarded, non-alighted passenger.
        /// </summary>
        /// <returns>
        /// Zero-based seat index, or <c>-1</c> if the bus is full.
        /// </returns>
        private int FindAvailableSeat()
        {
            bool[] occupied = new bool[BusSeatCount];

            foreach (var p in _passengers)
            {
                if (p.HasBoarded && !p.HasAlighted &&
                    p.SeatIndex >= 0 && p.SeatIndex < BusSeatCount)
                {
                    occupied[p.SeatIndex] = true;
                }
            }

            for (int i = 0; i < BusSeatCount; i++)
            {
                if (!occupied[i]) return i;
            }

            return -1; // Bus is full.
        }
    }
}
