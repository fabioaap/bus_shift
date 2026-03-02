using System;
using UnityEngine;

namespace BusShift.Core
{
    // ─────────────────────────────────────────────────────────────────────────────
    //  NarrativeEvent struct
    // ─────────────────────────────────────────────────────────────────────────────

    /// <summary>
    /// Defines a single timed narrative event that plays a radio dialogue sequence
    /// during a specific day and period.
    ///
    /// <para>
    /// <b>Fields:</b>
    /// <list type="bullet">
    ///   <item><see cref="Day"/> – In-game day (1–5) when this event is eligible.</item>
    ///   <item><see cref="Period"/> – Period index (0 = Morning, 1 = Night).</item>
    ///   <item><see cref="TriggerTime"/> – Seconds elapsed since period start before firing.</item>
    ///   <item><see cref="DialogueSequenceId"/> – ID forwarded to the DialogueSystem.</item>
    ///   <item><see cref="HasTriggered"/> – Runtime flag; cleared by <see cref="NarrativeEventManager.ResetEvents"/>.</item>
    /// </list>
    /// </para>
    /// </summary>
    [Serializable]
    public struct NarrativeEvent
    {
        [Tooltip("Day this event belongs to (1–5).")]
        public int Day;

        [Tooltip("Period: 0 = Morning, 1 = Night.")]
        public int Period;

        [Tooltip("Seconds elapsed since period start before this event fires.")]
        public float TriggerTime;

        [Tooltip("Dialogue sequence identifier forwarded to the DialogueSystem " +
                 "(or the OnRadioDialogueRequested event until DialogueSystem lands).")]
        public string DialogueSequenceId;

        /// <summary>
        /// Runtime flag set to <c>true</c> after this event fires.
        /// Reset by <see cref="NarrativeEventManager.ResetEvents"/> at period start.
        /// Not serialised to avoid polluting saved data.
        /// </summary>
        [NonSerialized] public bool HasTriggered;
    }

    // ─────────────────────────────────────────────────────────────────────────────
    //  NarrativeEventManager
    // ─────────────────────────────────────────────────────────────────────────────

    /// <summary>
    /// Singleton that fires time-triggered narrative events (radio dialogues) during
    /// each day/period shift.
    ///
    /// <para>
    /// <b>Event pipeline:</b>
    /// <list type="number">
    ///   <item>Subscribes to <see cref="DayManager.OnPeriodChanged"/> to track the active period.</item>
    ///   <item>
    ///     Each <c>Update</c> tick advances an internal elapsed-time counter and
    ///     checks every pending <see cref="NarrativeEvent"/> in <see cref="_events"/>.
    ///   </item>
    ///   <item>
    ///     When <c>TriggerTime</c> is reached, fires
    ///     <see cref="OnRadioDialogueRequested"/> (forward-compatible hook until
    ///     <c>DialogueSystem</c> is implemented) and <see cref="OnEventTriggered"/>.
    ///   </item>
    ///   <item>Marks the event as <c>HasTriggered = true</c> — it will not repeat.</item>
    /// </list>
    /// </para>
    ///
    /// <para>
    /// <b>DialogueSystem integration (not yet implemented):</b>
    /// Subscribe <see cref="OnRadioDialogueRequested"/> from DialogueSystem:
    /// <c>NarrativeEventManager.OnRadioDialogueRequested += seqId => PlayRadioDialogue(seqId);</c>
    /// </para>
    ///
    /// <para>
    /// <b>Setup:</b>
    /// <list type="bullet">
    ///   <item>Attach to a persistent GameObject alongside <see cref="GameManager"/>.</item>
    ///   <item>Populate <see cref="_events"/> in the Inspector with your narrative schedule.</item>
    /// </list>
    /// </para>
    /// </summary>
    public class NarrativeEventManager : MonoBehaviour
    {
        // ── Singleton ─────────────────────────────────────────────────────────────

        /// <summary>Singleton instance. Persists across scenes.</summary>
        public static NarrativeEventManager Instance { get; private set; }

        // ── Inspector ─────────────────────────────────────────────────────────────

        [Header("Narrative Events Schedule")]
        [Tooltip("All timed narrative events across all days/periods. " +
                 "Assign in the Inspector; HasTriggered is cleared automatically each period.")]
        [SerializeField] private NarrativeEvent[] _events = Array.Empty<NarrativeEvent>();

        [Tooltip("When true, fired events are logged to the Unity console.")]
        [SerializeField] private bool _debugLog = false;

        // ── State ─────────────────────────────────────────────────────────────────

        private int   _currentDay    = 1;
        private int   _currentPeriod = 0;
        private float _elapsedSeconds;
        private bool  _periodActive;

        // ── Events ────────────────────────────────────────────────────────────────

        /// <summary>
        /// Fired when a <see cref="NarrativeEvent"/> triggers.
        /// Passes the full event struct for subscribers that need context.
        /// </summary>
        public static event Action<NarrativeEvent> OnEventTriggered;

        /// <summary>
        /// Forward-compatible hook for <c>DialogueSystem</c> (not yet implemented).
        /// Passes the <see cref="NarrativeEvent.DialogueSequenceId"/> when an event fires.
        ///
        /// <para>
        /// When DialogueSystem is implemented, subscribe:
        /// <c>NarrativeEventManager.OnRadioDialogueRequested += id => DialogueSystem.Instance.PlayRadioDialogue(id);</c>
        /// </para>
        /// </summary>
        public static event Action<string> OnRadioDialogueRequested;

        // ── Unity Lifecycle ──────────────────────────────────────────────────────

        private void Awake()
        {
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
            DayManager.OnPeriodChanged += HandlePeriodChanged;
        }

        private void OnDisable()
        {
            DayManager.OnPeriodChanged -= HandlePeriodChanged;
        }

        private void Update()
        {
            if (!_periodActive) return;
            if (_events == null || _events.Length == 0) return;

            _elapsedSeconds += Time.deltaTime;

            for (int i = 0; i < _events.Length; i++)
            {
                ref NarrativeEvent ev = ref _events[i];

                if (ev.HasTriggered) continue;
                if (ev.Day != _currentDay || ev.Period != _currentPeriod) continue;
                if (_elapsedSeconds < ev.TriggerTime) continue;

                FireEvent(ref ev);
            }
        }

        // ── Public API ───────────────────────────────────────────────────────────

        /// <summary>
        /// Clears all <see cref="NarrativeEvent.HasTriggered"/> flags and resets
        /// the elapsed-time counter. Call at the start of each period to allow
        /// re-runs in the same session (e.g. after a reload or period restart).
        /// </summary>
        public void ResetEvents()
        {
            _elapsedSeconds = 0f;

            if (_events == null) return;

            for (int i = 0; i < _events.Length; i++)
                _events[i].HasTriggered = false;

#if UNITY_EDITOR || DEVELOPMENT_BUILD
            Debug.Log("[NarrativeEventManager] Events reset.");
#endif
        }

        // ── Private Helpers ───────────────────────────────────────────────────────

        private void HandlePeriodChanged(int day, int period)
        {
            _currentDay    = day;
            _currentPeriod = period;
            _periodActive  = true;

            ResetEvents();

#if UNITY_EDITOR || DEVELOPMENT_BUILD
            Debug.Log($"[NarrativeEventManager] Tracking Day {day} Period {period}.");
#endif
        }

        private void FireEvent(ref NarrativeEvent ev)
        {
            ev.HasTriggered = true;

            // Forward to DialogueSystem (or any other subscriber).
            if (!string.IsNullOrEmpty(ev.DialogueSequenceId))
                OnRadioDialogueRequested?.Invoke(ev.DialogueSequenceId);

            // Broadcast full event data.
            OnEventTriggered?.Invoke(ev);

            if (_debugLog)
            {
                Debug.Log($"[NarrativeEventManager] ▶ Fired: Day {ev.Day} Period {ev.Period} " +
                          $"@ {ev.TriggerTime:F1}s → \"{ev.DialogueSequenceId}\"");
            }
        }

        // ── Editor Validation ─────────────────────────────────────────────────────

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_events == null) return;

            for (int i = 0; i < _events.Length; i++)
            {
                _events[i].Day         = Mathf.Clamp(_events[i].Day, 1, DayManager.TotalDays);
                _events[i].Period      = Mathf.Clamp(_events[i].Period, 0, DayManager.PeriodsPerDay - 1);
                _events[i].TriggerTime = Mathf.Max(0f, _events[i].TriggerTime);
            }
        }
#endif
    }
}
