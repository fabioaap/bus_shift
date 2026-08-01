using System;
using UnityEngine;

namespace BusShift.Core
{
    /// <summary>
    /// Owns Dale's normalized tension value.
    ///
    /// Zero means fully controlled. One means the maximum tension threshold and
    /// immediately triggers game over. RemainingSanity is derived as one minus tension.
    /// </summary>
    [DisallowMultipleComponent]
    public class SanitySystem : MonoBehaviour
    {
        private static readonly float[,] InitialTensionValues =
        {
            { 0.05f, 0.10f },
            { 0.10f, 0.15f },
            { 0.15f, 0.20f },
            { 0.25f, 0.30f },
            { 0.30f, 0.35f }
        };

        [SerializeField] [Range(0f, 1f)] private float _currentTension;

        public float CurrentTension => _currentTension;
        public float RemainingSanity => 1f - _currentTension;
        public bool HasReachedGameOverThreshold { get; private set; }

        /// <summary>
        /// Legacy alias retained for existing scripts and serialized integrations.
        /// The value has always behaved as tension, not remaining sanity.
        /// New code must use CurrentTension or RemainingSanity explicitly.
        /// </summary>
        [Obsolete("CurrentSanity stores tension. Use CurrentTension or RemainingSanity explicitly.")]
        public float CurrentSanity => CurrentTension;

        public static event Action<float> OnTensionChanged;
        public static event Action<float> OnRemainingSanityChanged;

        /// <summary>Legacy event retained for existing HUD integrations. Argument is tension.</summary>
        public static event Action<float> OnSanityChanged;

        public static event Action OnGameOver;

        public void InitializeForPeriod(int dayIndex, int period)
        {
            int safeDayIndex = Mathf.Clamp(dayIndex, 0, InitialTensionValues.GetLength(0) - 1);
            int safePeriod = Mathf.Clamp(period, 0, InitialTensionValues.GetLength(1) - 1);

            HasReachedGameOverThreshold = false;
            SetTension(InitialTensionValues[safeDayIndex, safePeriod]);
        }

        public void AddTension(float amount)
        {
            if (amount <= 0f || HasReachedGameOverThreshold)
            {
                return;
            }

            SetTension(_currentTension + amount);

            if (_currentTension >= 1f)
            {
                TriggerTensionGameOver();
            }
        }

        public void ReduceTension(float amount)
        {
            if (amount <= 0f || HasReachedGameOverThreshold)
            {
                return;
            }

            SetTension(_currentTension - amount);
        }

        /// <summary>
        /// Restores a saved tension value without applying an additional delta.
        /// A value at the maximum threshold triggers game over exactly once.
        /// </summary>
        public void RestoreTension(float tension)
        {
            HasReachedGameOverThreshold = false;
            SetTension(tension);

            if (_currentTension >= 1f)
            {
                TriggerTensionGameOver();
            }
        }

        private void SetTension(float tension)
        {
            float next = Mathf.Clamp01(tension);

            if (Mathf.Approximately(next, _currentTension))
            {
                return;
            }

            _currentTension = next;

            OnTensionChanged?.Invoke(_currentTension);
            OnRemainingSanityChanged?.Invoke(RemainingSanity);
            OnSanityChanged?.Invoke(_currentTension);
        }

        private void TriggerTensionGameOver()
        {
            if (HasReachedGameOverThreshold)
            {
                return;
            }

            HasReachedGameOverThreshold = true;
            OnGameOver?.Invoke();
            GameManager.Instance?.TriggerGameOver("Tension reached the maximum threshold.");
        }
    }
}
