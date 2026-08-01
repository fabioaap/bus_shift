using System;
using UnityEngine;

namespace BusShift.Core
{
    public enum Period
    {
        Morning = 0,
        Night = 1
    }

    /// <summary>
    /// Owns progression through five days and two periods per day.
    ///
    /// Build 0.1.0 uses a dedicated slice flow and only validates Day 1. This manager
    /// remains the canonical progression owner for the complete game.
    /// </summary>
    [DisallowMultipleComponent]
    public class DayManager : MonoBehaviour
    {
        public const int TotalDays = 5;
        public const int PeriodsPerDay = 2;
        public const int TotalPeriods = TotalDays * PeriodsPerDay;

        [Header("System References")]
        [SerializeField] private SanitySystem _sanitySystem;
        [SerializeField] private TimerSystem _timerSystem;

        public int CurrentDay { get; private set; } = 1;
        public int CurrentPeriod { get; private set; }
        public int TotalPeriodsCompleted { get; private set; }
        public bool IsGameComplete { get; private set; }

        public static event Action<int, int> OnPeriodChanged;
        public static event Action OnDayCompleted;
        public static event Action OnGameCompleted;
        public static event Action<int> OnDayDifficultyChanged;

        public static event Action<int, Period> OnPeriodStarted;
        public static event Action<int, Period> OnPeriodEnded;
        public static event Action<int> OnDayStarted;

        private void Awake()
        {
            if (_sanitySystem == null)
            {
                _sanitySystem = FindAnyObjectByType<SanitySystem>();
            }

            if (_timerSystem == null)
            {
                _timerSystem = FindAnyObjectByType<TimerSystem>();
            }
        }

        public void InitializeFromSave(int day, int period)
        {
            CurrentDay = Mathf.Clamp(day, 1, TotalDays);
            CurrentPeriod = Mathf.Clamp(period, 0, PeriodsPerDay - 1);
            TotalPeriodsCompleted =
                ((CurrentDay - 1) * PeriodsPerDay) + CurrentPeriod;
            IsGameComplete = false;

            Debug.Log(
                $"[DayManager] Loaded Day {CurrentDay} {PeriodLabel(CurrentPeriod)} " +
                $"with {TotalPeriodsCompleted} completed periods.");
        }

        public void OnPeriodStart()
        {
            if (IsGameComplete)
            {
                return;
            }

            _sanitySystem?.InitializeForPeriod(CurrentDay - 1, CurrentPeriod);
            _timerSystem?.ResetTimer();

            OnDayDifficultyChanged?.Invoke(CurrentDay);
            OnPeriodChanged?.Invoke(CurrentDay, CurrentPeriod);

            if (CurrentPeriod == (int)Period.Morning)
            {
                OnDayStarted?.Invoke(CurrentDay);
            }

            OnPeriodStarted?.Invoke(CurrentDay, (Period)CurrentPeriod);

            Debug.Log(
                $"[DayManager] Started Day {CurrentDay} {PeriodLabel(CurrentPeriod)}.");
        }

        public void OnPeriodEnd()
        {
            if (IsGameComplete)
            {
                return;
            }

            PerformAutoSave();
            OnPeriodEnded?.Invoke(CurrentDay, (Period)CurrentPeriod);

            Debug.Log(
                $"[DayManager] Ended Day {CurrentDay} {PeriodLabel(CurrentPeriod)}.");
        }

        public void CompleteCurrentPeriod()
        {
            if (IsGameComplete)
            {
                return;
            }

            TotalPeriodsCompleted++;

            if (CurrentPeriod == (int)Period.Morning)
            {
                CurrentPeriod = (int)Period.Night;
                Debug.Log($"[DayManager] Advanced to Day {CurrentDay} Night.");
                return;
            }

            OnDayCompleted?.Invoke();
            Debug.Log($"[DayManager] Completed Day {CurrentDay}.");

            if (CurrentDay >= TotalDays)
            {
                CompleteGame();
                return;
            }

            CurrentDay++;
            CurrentPeriod = (int)Period.Morning;
            Debug.Log($"[DayManager] Advanced to Day {CurrentDay} Morning.");
        }

        private void CompleteGame()
        {
            IsGameComplete = true;

            GameManager.Instance?.TriggerVictory();
            OnGameCompleted?.Invoke();

            Debug.Log("[DayManager] Game completed after Day 5 Night.");
        }

        private void PerformAutoSave()
        {
            float currentTension =
                _sanitySystem != null ? _sanitySystem.CurrentTension : 0f;

            SaveData data = new SaveData
            {
                Day = CurrentDay,
                Period = CurrentPeriod,
                TotalPeriods = TotalPeriodsCompleted,
                Sanity = currentTension,
                SaveDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
            };

            SaveSystem.SaveGame(SaveSystem.AutoSaveSlot, data);
        }

        private static string PeriodLabel(int period)
        {
            return period == (int)Period.Morning ? "Morning" : "Night";
        }
    }
}
