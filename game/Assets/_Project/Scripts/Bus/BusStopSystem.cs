using System;
using UnityEngine;
using BusShift.Core;

namespace BusShift.Bus
{
    // ─────────────────────────────────────────────────────────────────────────
    //  Enum
    // ─────────────────────────────────────────────────────────────────────────

    /// <summary>Represents the current state of the bus doors.</summary>
    public enum DoorState
    {
        /// <summary>Doors are fully closed.</summary>
        Closed,

        /// <summary>Doors are in the process of opening (transition).</summary>
        Opening,

        /// <summary>Doors are fully open and passengers may board/alight.</summary>
        Open,

        /// <summary>Doors are in the process of closing (transition).</summary>
        Closing
    }

    // ─────────────────────────────────────────────────────────────────────────
    //  BusStopSystem
    // ─────────────────────────────────────────────────────────────────────────

    /// <summary>
    /// Handles bus-stop interactions: door open/close via the <b>T</b> key,
    /// automatic door closing, passenger boarding/alighting, and sanity-tension
    /// penalties for skipping stops.
    ///
    /// Requires a <see cref="RouteManager"/> reference assigned in the Inspector.
    /// </summary>
    public class BusStopSystem : MonoBehaviour
    {
        // ── Inspector ────────────────────────────────────────────────────────

        [Header("References")]
        [Tooltip("RouteManager that provides stop-proximity data.")]
        public RouteManager RouteManager;

        [Header("Door Settings")]
        [Tooltip("Key the driver presses to open or close the doors.")]
        public KeyCode DoorToggleKey = KeyCode.T;

        [Tooltip("Seconds the door stays open before auto-closing.")]
        public float AutoCloseDelay = 5f;

        [Header("Passenger Settings")]
        [Tooltip("Maximum number of passengers allowed on the bus.")]
        public int MaxPassengers = 10;

        [Tooltip("Sanity tension added when the bus passes a stop without opening the doors.")]
        public float SkipStopTensionPenalty = 0.05f;

        // ── Public state ─────────────────────────────────────────────────────

        /// <summary>The current state of the bus doors.</summary>
        public DoorState CurrentDoorState { get; private set; } = DoorState.Closed;

        /// <summary>Number of passengers currently on the bus (0 – <see cref="MaxPassengers"/>).</summary>
        public int CurrentPassengerCount { get; private set; }

        // ── Events ────────────────────────────────────────────────────────────

        /// <summary>Fired when the doors finish opening.</summary>
        public static event Action OnDoorOpened;

        /// <summary>Fired when the doors finish closing.</summary>
        public static event Action OnDoorClosed;

        /// <summary>
        /// Fired whenever the passenger count changes.
        /// Parameter: new passenger count.
        /// </summary>
        public static event Action<int> OnPassengerCountChanged;

        /// <summary>
        /// Fired when the bus doors open at a stop, signalling that passenger
        /// boarding and alighting should be processed.
        /// Parameter: the waypoint index of the current stop, as tracked by
        /// <see cref="RouteManager"/>.
        /// Consumed by <see cref="PassengerManager"/> to trigger config-driven
        /// boarding/alighting via <see cref="StopPassengerConfig"/>.
        /// </summary>
        public static event Action<int> OnStopReached;

        // ── Private ───────────────────────────────────────────────────────────

        /// <summary>Counts down to auto-close while doors are open.</summary>
        private float _autoCloseTimer;

        /// <summary>True if the doors were opened at the current stop approach.</summary>
        private bool _doorOpenedAtCurrentStop;

        /// <summary>The stop-waypoint index last penalised, to avoid double-penalising.</summary>
        private int _lastPenalisedStopIndex = -1;

        /// <summary>
        /// Waypoint index of the stop the bus is currently approaching,
        /// captured from <see cref="HandleApproachingStop"/> and forwarded to
        /// <see cref="OnStopReached"/> when the doors open.
        /// </summary>
        private int _currentApproachStopIndex = -1;

        private SanitySystem _sanitySystem;

        // ── Unity lifecycle ───────────────────────────────────────────────────

        private void Awake()
        {
            // Resolve SanitySystem from the singleton GameManager
            if (GameManager.Instance != null)
                _sanitySystem = GameManager.Instance.SanitySystem;
        }

        private void OnEnable()
        {
            RouteManager.OnApproachingStop += HandleApproachingStop;
            RouteManager.OnWaypointReached  += HandleWaypointReached;
        }

        private void OnDisable()
        {
            RouteManager.OnApproachingStop -= HandleApproachingStop;
            RouteManager.OnWaypointReached  -= HandleWaypointReached;
        }

        private void Update()
        {
            HandleDoorInput();
            TickAutoClose();
        }

        // ── Door input ────────────────────────────────────────────────────────

        /// <summary>
        /// Reads the door-toggle key. The door may only be operated when
        /// the bus is within approach range of a stop.
        /// </summary>
        private void HandleDoorInput()
        {
            if (!Input.GetKeyDown(DoorToggleKey)) return;
            if (RouteManager == null || !RouteManager.IsNearStop) return;

            switch (CurrentDoorState)
            {
                case DoorState.Closed:
                case DoorState.Closing:
                    OpenDoor();
                    break;

                case DoorState.Open:
                case DoorState.Opening:
                    CloseDoor();
                    break;
            }
        }

        // ── Auto-close ────────────────────────────────────────────────────────

        /// <summary>Decrements the auto-close timer and closes the doors when it expires.</summary>
        private void TickAutoClose()
        {
            if (CurrentDoorState != DoorState.Open) return;

            _autoCloseTimer -= Time.deltaTime;
            if (_autoCloseTimer <= 0f)
                CloseDoor();
        }

        // ── Door operations ───────────────────────────────────────────────────

        /// <summary>
        /// Opens the doors, resets the auto-close timer, marks that this stop
        /// was serviced, triggers passenger exchange, and fires
        /// <see cref="OnStopReached"/> so that <see cref="PassengerManager"/>
        /// can process config-driven boarding and alighting.
        /// </summary>
        private void OpenDoor()
        {
            CurrentDoorState = DoorState.Opening;
            // NOTE: Hook an Animator trigger here for a real door animation.
            //       For now the state transitions immediately to Open.
            CurrentDoorState = DoorState.Open;

            _autoCloseTimer = AutoCloseDelay;
            _doorOpenedAtCurrentStop = true;

            OnDoorOpened?.Invoke();

            // Notify PassengerManager (and any other subscriber) that the bus
            // has serviced this stop so config-driven boarding can be processed.
            if (_currentApproachStopIndex >= 0)
                OnStopReached?.Invoke(_currentApproachStopIndex);

            HandlePassengerExchange();
        }

        /// <summary>
        /// Closes the doors and fires <see cref="OnDoorClosed"/>.
        /// </summary>
        private void CloseDoor()
        {
            CurrentDoorState = DoorState.Closing;
            // NOTE: Hook an Animator trigger here for door-close animation.
            CurrentDoorState = DoorState.Closed;

            OnDoorClosed?.Invoke();
        }

        // ── Passengers ────────────────────────────────────────────────────────

        /// <summary>
        /// Randomly disembarks and boards passengers when the doors open.
        /// Both operations generate a random count of 1–3, clamped to capacity.
        /// </summary>
        private void HandlePassengerExchange()
        {
            // Alighting — 1-3 passengers, but can't exceed current count
            int alighting = CurrentPassengerCount > 0
                ? Mathf.Min(UnityEngine.Random.Range(1, 4), CurrentPassengerCount)
                : 0;

            CurrentPassengerCount -= alighting;

            // Boarding — 1-3 passengers, capped by remaining capacity
            int capacity  = MaxPassengers - CurrentPassengerCount;
            int boarding   = capacity > 0
                ? Mathf.Min(UnityEngine.Random.Range(1, 4), capacity)
                : 0;

            CurrentPassengerCount += boarding;

            OnPassengerCountChanged?.Invoke(CurrentPassengerCount);
        }

        // ── Route event handlers ──────────────────────────────────────────────

        /// <summary>
        /// Resets the door-opened flag and records the current stop index when
        /// the bus approaches a new stop, so we can detect whether it was
        /// skipped and forward the index to <see cref="OnStopReached"/>.
        /// </summary>
        private void HandleApproachingStop(int stopIndex, string stopName)
        {
            _doorOpenedAtCurrentStop   = false;
            _currentApproachStopIndex  = stopIndex;
        }

        /// <summary>
        /// Called when any waypoint is reached. If the waypoint was a stop
        /// that was not serviced, applies a sanity-tension penalty.
        /// </summary>
        private void HandleWaypointReached(int waypointIndex)
        {
            if (RouteManager == null) return;
            if (waypointIndex < 0 || waypointIndex >= RouteManager.Waypoints.Count) return;

            WaypointData wp = RouteManager.Waypoints[waypointIndex];
            bool isUnsercedStop = wp.IsStop
                                  && !_doorOpenedAtCurrentStop
                                  && waypointIndex != _lastPenalisedStopIndex;

            if (!isUnsercedStop) return;

            _lastPenalisedStopIndex = waypointIndex;
            _sanitySystem?.AddTension(SkipStopTensionPenalty);

#if UNITY_EDITOR || DEVELOPMENT_BUILD
            Debug.Log($"[BusStopSystem] Stop '{wp.Name}' skipped — tension +{SkipStopTensionPenalty}");
#endif
        }
    }
}
