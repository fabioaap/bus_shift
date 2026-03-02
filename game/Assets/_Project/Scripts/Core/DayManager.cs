using System;
using UnityEngine;

namespace BusShift.Core
{
    /// <summary>
    /// Period helper enum. 0 = Morning, 1 = Night.
    /// Kept for backward compatibility with existing event subscribers.
    /// </summary>
    public enum Period { Morning = 0, Night = 1 }

    /// <summary>
    /// Manages progression through 5 days × 2 periods (10 total shifts).
    ///
    /// Flow: Day1 Morning → Day1 Night → Day2 Morning → … → Day5 Night → Game Complete.
    ///
    /// Responsibilities:
    ///   • Track current day (1-5) and period (0=Morning / 1=Night).
    ///   • Initialize SanitySystem and TimerSystem at the start of each period.
    ///   • Auto-save via SaveSystem at period end.
    ///   • Fire events so ghost systems can react to day/difficulty changes.
    ///
    /// Setup: Attach to a persistent GameObject in the first scene.
    ///        Assign SanitySystem and TimerSystem in the Inspector (or they are
    ///        located automatically via FindAnyObjectByType on Awake).
    /// </summary>
    public class DayManager : MonoBehaviour
    {
        // ── Constants ────────────────────────────────────────────────────────────

        /// <summary>Total number of in-game days.</summary>
        public const int TotalDays = 5;

        /// <summary>Number of periods per day (Morning + Night).</summary>
        public const int PeriodsPerDay = 2;

        /// <summary>Total periods across all days (10).</summary>
        public const int TotalPeriods = TotalDays * PeriodsPerDay;

        // ── Inspector References ─────────────────────────────────────────────────

        [Header("System References")]
        [Tooltip("SanitySystem in the scene. Auto-found on Awake if not assigned.")]
        [SerializeField] private SanitySystem _sanitySystem;

        [Tooltip("TimerSystem in the scene. Auto-found on Awake if not assigned.")]
        [SerializeField] private TimerSystem _timerSystem;

        // ── State ─────────────────────────────────────────────────────────────────

        /// <summary>Current day, 1-5.</summary>
        public int CurrentDay { get; private set; } = 1;

        /// <summary>Current period. 0 = Morning, 1 = Night.</summary>
        public int CurrentPeriod { get; private set; } = 0;

        /// <summary>Number of periods fully completed so far (0-9).</summary>
        public int TotalPeriodsCompleted { get; private set; } = 0;

        /// <summary>True after Day 5 Night is completed.</summary>
        public bool IsGameComplete { get; private set; } = false;

        // ── Events ────────────────────────────────────────────────────────────────

        /// <summary>
        /// Fired when a new period begins.
        /// Parameters: (day 1-5, period 0=Morning/1=Night).
        /// </summary>
        public static event Action<int, int> OnPeriodChanged;

        /// <summary>Fired after both periods of a day are completed (at end of Night).</summary>
        public static event Action OnDayCompleted;

        /// <summary>Fired after Day 5 Night is completed — the game is won.</summary>
        public static event Action OnGameCompleted;

        /// <summary>
        /// Fired at the start of each period so ghost systems can scale difficulty.
        /// Parameter: current day (1-5).
        /// </summary>
        public static event Action<int> OnDayDifficultyChanged;

        // ── Backward-compatible events (kept for existing subscribers) ────────────

        /// <summary>Fires when a period starts, using the Period enum. Kept for legacy subscribers.</summary>
        public static event Action<int, Period> OnPeriodStarted;

        /// <summary>Fires when a period ends, using the Period enum. Kept for legacy subscribers.</summary>
        public static event Action<int, Period> OnPeriodEnded;

        /// <summary>Fires when a day begins (first period start of that day). Kept for legacy subscribers.</summary>
        public static event Action<int> OnDayStarted;

        // ── Unity Lifecycle ──────────────────────────────────────────────────────

        private void Awake()
        {
            if (_sanitySystem == null)
                _sanitySystem = FindAnyObjectByType<SanitySystem>();

            if (_timerSystem == null)
                _timerSystem = FindAnyObjectByType<TimerSystem>();
        }

        // ── Public API ───────────────────────────────────────────────────────────

        /// <summary>
        /// Restores day and period state loaded from a save slot.
        /// Call this before <see cref="OnPeriodStart"/> when continuing a saved game.
        /// </summary>
        /// <param name="day">Saved day (1-5).</param>
        /// <param name="period">Saved period (0=Morning, 1=Night).</param>
        public void InitializeFromSave(int day, int period)
        {
            CurrentDay            = Mathf.Clamp(day,    1, TotalDays);
            CurrentPeriod         = Mathf.Clamp(period, 0, PeriodsPerDay - 1);
            TotalPeriodsCompleted = (CurrentDay - 1) * PeriodsPerDay + CurrentPeriod;
            IsGameComplete        = false;

            Debug.Log($"[DayManager] Loaded from save → Day {CurrentDay} {PeriodLabel(CurrentPeriod)} " +
                      $"(periods completed: {TotalPeriodsCompleted})");
        }

        /// <summary>
        /// Initialises all period-scoped systems and broadcasts period start events.
        /// Call once after the period's scene is loaded (or after InitializeFromSave).
        /// </summary>
        public void OnPeriodStart()
        {
            if (IsGameComplete) return;

            // Initialise systems
            _sanitySystem?.InitializeForPeriod(CurrentDay - 1, CurrentPeriod);
            _timerSystem?.ResetTimer();

            // Broadcast
            OnDayDifficultyChanged?.Invoke(CurrentDay);
            OnPeriodChanged?.Invoke(CurrentDay, CurrentPeriod);

            // Backward-compat events
            if (CurrentPeriod == 0)
                OnDayStarted?.Invoke(CurrentDay);

            OnPeriodStarted?.Invoke(CurrentDay, (Period)CurrentPeriod);

            Debug.Log($"[DayManager] Period started → Day {CurrentDay} {PeriodLabel(CurrentPeriod)}");
        }

        /// <summary>
        /// Persists current state and broadcasts period-end events.
        /// Call when the player's shift ends (bus arrives / time runs out).
        /// </summary>
        public void OnPeriodEnd()
        {
            PerformAutoSave();
            OnPeriodEnded?.Invoke(CurrentDay, (Period)CurrentPeriod);

            Debug.Log($"[DayManager] Period ended → Day {CurrentDay} {PeriodLabel(CurrentPeriod)}");
        }

        /// <summary>
        /// Advances the state machine to the next period.
        /// Fires <see cref="OnDayCompleted"/> when a full day finishes,
        /// and <see cref="OnGameCompleted"/> when Day 5 Night is cleared.
        /// </summary>
        public void CompleteCurrentPeriod()
        {
            if (IsGameComplete) return;

            TotalPeriodsCompleted++;

            if (CurrentPeriod == 0)
            {
                // Morning → Night (same day)
                CurrentPeriod = 1;
                Debug.Log($"[DayManager] Advanced to Day {CurrentDay} Night");
            }
            else
            {
                // Night → next day
                bool isFinalDay = CurrentDay >= TotalDays;

                OnDayCompleted?.Invoke();
                Debug.Log($"[DayManager] Day {CurrentDay} completed.");

                if (isFinalDay)
                {
                    IsGameComplete = true;
                    OnGameCompleted?.Invoke();
                    GameManager.Instance?.SetState(GameState.GameOver);
                    Debug.Log("[DayManager] ✅ Game completed — Day 5 Night cleared!");
                    return;
                }

                CurrentDay++;
                CurrentPeriod = 0;
                Debug.Log($"[DayManager] Advanced to Day {CurrentDay} Morning");
            }
        }

        // ── Private Helpers ──────────────────────────────────────────────────────

        private void PerformAutoSave()
        {
            float currentSanity = _sanitySystem != null ? _sanitySystem.CurrentSanity : 0f;

            var data = new SaveData
            {
                Day          = CurrentDay,
                Period       = CurrentPeriod,
                TotalPeriods = TotalPeriodsCompleted,
                Sanity       = currentSanity,
                SaveDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
            };

            SaveSystem.SaveGame(SaveSystem.AutoSaveSlot, data);
        }

        private static string PeriodLabel(int period) =>
            period == 0 ? "Morning" : "Night";
    }
}

