using UnityEngine;

namespace BusShift.Core
{
    public enum Period { Morning, Night }

    public class DayManager : MonoBehaviour
    {
        public int CurrentDay { get; private set; } = 1;
        public Period CurrentPeriod { get; private set; } = Period.Morning;

        public static event System.Action<int, Period> OnPeriodStarted;
        public static event System.Action<int, Period> OnPeriodCompleted;
        public static event System.Action<int> OnDayStarted;

        public void StartPeriod(int day, Period period)
        {
            CurrentDay = day;
            CurrentPeriod = period;
            OnDayStarted?.Invoke(day);
            OnPeriodStarted?.Invoke(day, period);
        }

        public void CompletePeriod()
        {
            OnPeriodCompleted?.Invoke(CurrentDay, CurrentPeriod);
        }
    }
}
