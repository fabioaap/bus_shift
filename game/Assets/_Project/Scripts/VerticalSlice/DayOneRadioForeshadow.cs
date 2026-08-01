using System;
using BusShift.Core;
using BusShift.Interventions;
using UnityEngine;

namespace BusShift.VerticalSlice
{
    /// <summary>
    /// Controls the safe Day 1 radio foreshadowing event.
    ///
    /// The event suggests Thomas through patterned static and an unidentified voice,
    /// but it does not spawn ThomasGhost, reveal his name, or trigger game over.
    /// Turning on the normal radio resolves the event.
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class DayOneRadioForeshadow : MonoBehaviour
    {
        [Header("References")]
        [Tooltip("The normal bus radio used as the player countermeasure.")]
        [SerializeField] private RadioSystem _radioSystem;

        [Tooltip("AudioSource that plays the channel two static and whisper clip.")]
        [SerializeField] private AudioSource _interferenceSource;

        [Header("Event timing")]
        [Tooltip("Start the first foreshadowing event automatically after this component is enabled.")]
        [SerializeField] private bool _autoStart;

        [Tooltip("Delay before an automatically started event begins.")]
        [SerializeField] [Min(0f)] private float _autoStartDelay = 1f;

        [Tooltip("Seconds before UI should display the contextual radio instruction.")]
        [SerializeField] [Min(0f)] private float _promptDelay = 4f;

        [Tooltip("Seconds before ignoring the event adds tension once.")]
        [SerializeField] [Min(0f)] private float _penaltyDelay = 8f;

        [Tooltip("Maximum duration before the foreshadowing fades without causing game over.")]
        [SerializeField] [Min(0.1f)] private float _maximumDuration = 14f;

        [Header("Tension")]
        [Tooltip("Tension added once when the player ignores the interference.")]
        [SerializeField] [Range(0f, 1f)] private float _ignoredTension = 0.05f;

        /// <summary>Raised when the channel two interference begins.</summary>
        public static event Action OnForeshadowStarted;

        /// <summary>Raised when UI should show the contextual radio instruction.</summary>
        public static event Action OnContextPromptRequested;

        /// <summary>Raised when the player resolves the event by turning on the normal radio.</summary>
        public static event Action OnForeshadowResolved;

        /// <summary>Raised when the one time ignored event tension is applied.</summary>
        public static event Action<float> OnIgnoredPenaltyApplied;

        /// <summary>Raised when the event ends without being resolved.</summary>
        public static event Action OnForeshadowExpired;

        private float _elapsed;
        private float _autoStartTimer;
        private bool _isActive;
        private bool _promptRaised;
        private bool _penaltyApplied;
        private bool _autoStartPending;

        public bool IsActive => _isActive;
        public bool PromptRaised => _promptRaised;
        public bool PenaltyApplied => _penaltyApplied;
        public float Elapsed => _elapsed;

        private void Awake()
        {
            if (_radioSystem == null)
            {
                _radioSystem = FindAnyObjectByType<RadioSystem>();
            }

            if (_interferenceSource != null)
            {
                _interferenceSource.loop = true;
                _interferenceSource.playOnAwake = false;
                _interferenceSource.Stop();
            }
        }

        private void OnEnable()
        {
            RadioSystem.OnRadioTurnedOn += HandleRadioTurnedOn;

            if (_autoStart)
            {
                _autoStartPending = true;
                _autoStartTimer = _autoStartDelay;
            }
        }

        private void OnDisable()
        {
            RadioSystem.OnRadioTurnedOn -= HandleRadioTurnedOn;
            StopInterferenceAudio();
            _isActive = false;
            _autoStartPending = false;
        }

        private void Update()
        {
            if (_autoStartPending)
            {
                _autoStartTimer -= Time.deltaTime;

                if (_autoStartTimer <= 0f)
                {
                    _autoStartPending = false;
                    BeginForeshadow();
                }
            }

            if (!_isActive)
            {
                return;
            }

            _elapsed += Time.deltaTime;

            if (!_promptRaised && _elapsed >= _promptDelay)
            {
                _promptRaised = true;
                OnContextPromptRequested?.Invoke();
            }

            if (!_penaltyApplied && _elapsed >= _penaltyDelay)
            {
                ApplyIgnoredPenalty();
            }

            if (_elapsed >= _maximumDuration)
            {
                ExpireForeshadow();
            }
        }

        /// <summary>
        /// Starts or restarts the safe Day 1 interference event.
        /// Route and stop logic can call this at the canonical trigger points.
        /// </summary>
        public void BeginForeshadow()
        {
            if (_isActive)
            {
                return;
            }

            _elapsed = 0f;
            _promptRaised = false;
            _penaltyApplied = false;
            _isActive = true;

            if (_interferenceSource != null && !_interferenceSource.isPlaying)
            {
                _interferenceSource.Play();
            }

            OnForeshadowStarted?.Invoke();
        }

        /// <summary>
        /// Cancels the event without treating it as a player success.
        /// Useful when a period ends or the scene unloads.
        /// </summary>
        public void CancelForeshadow()
        {
            if (!_isActive)
            {
                return;
            }

            _isActive = false;
            StopInterferenceAudio();
        }

        private void HandleRadioTurnedOn()
        {
            if (!_isActive)
            {
                return;
            }

            _isActive = false;
            StopInterferenceAudio();
            OnForeshadowResolved?.Invoke();
        }

        private void ApplyIgnoredPenalty()
        {
            _penaltyApplied = true;

            if (_ignoredTension > 0f)
            {
                GameManager.Instance?.SanitySystem?.AddTension(_ignoredTension);
            }

            OnIgnoredPenaltyApplied?.Invoke(_ignoredTension);
        }

        private void ExpireForeshadow()
        {
            _isActive = false;
            StopInterferenceAudio();
            OnForeshadowExpired?.Invoke();
        }

        private void StopInterferenceAudio()
        {
            if (_interferenceSource == null)
            {
                return;
            }

            _interferenceSource.Stop();
        }
    }
}
