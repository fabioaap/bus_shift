using UnityEngine;

namespace BusShift.Core
{
    public class SanitySystem : MonoBehaviour
    {
        // Initial sanity values [day 0..4][period: 0=morning, 1=night]
        private static readonly float[,] InitialValues =
        {
            { 0.05f, 0.10f },
            { 0.10f, 0.15f },
            { 0.15f, 0.20f },
            { 0.25f, 0.30f },
            { 0.30f, 0.35f }
        };

        [Range(0f, 1f)] public float CurrentSanity { get; private set; }

        public static event System.Action<float> OnSanityChanged;
        public static event System.Action OnGameOver;

        public void InitializeForPeriod(int day, int period)
        {
            CurrentSanity = InitialValues[Mathf.Clamp(day, 0, 4), Mathf.Clamp(period, 0, 1)];
            OnSanityChanged?.Invoke(CurrentSanity);
        }

        public void AddTension(float amount)
        {
            CurrentSanity = Mathf.Clamp01(CurrentSanity + amount);
            OnSanityChanged?.Invoke(CurrentSanity);
            if (CurrentSanity >= 1f) OnGameOver?.Invoke();
        }

        public void ReduceTension(float amount)
        {
            CurrentSanity = Mathf.Clamp01(CurrentSanity - amount);
            OnSanityChanged?.Invoke(CurrentSanity);
        }
    }
}
