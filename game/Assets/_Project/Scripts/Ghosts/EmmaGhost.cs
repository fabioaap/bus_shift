using System;
using UnityEngine;

namespace BusShift.Ghosts
{
    /// <summary>
    /// Emma, "The Trickster".
    ///
    /// Emma appears beside the driver and reaches for the control panel. The player
    /// must activate Panel Lock before the attack window expires.
    ///
    /// Day 1 uses a longer onboarding window so the first canonical encounter teaches
    /// the countermeasure before the two second advanced difficulty is introduced.
    ///
    /// Observation remains contextual:
    ///
    /// 1. Observing before full manifestation makes Emma vanish, but adds tension.
    /// 2. Observing after full manifestation drives Emma back and reduces tension.
    ///
    /// Wire up:
    ///
    /// 1. Assign <see cref="_driverSeatRightPosition"/> near the driver's right side.
    /// 2. Call <see cref="Defeat"/> when Panel Lock is activated.
    /// 3. Subscribe to <see cref="OnLaugh"/> for escalating audio feedback.
    /// 4. Subscribe to <see cref="GhostBase.OnAttackStarted"/> for manifestation feedback.
    /// 5. Call <see cref="SetDayDifficulty"/> when a period begins.
    /// </summary>
    public class EmmaGhost : GhostBase
    {
        [Header("Emma, The Trickster")]
        [Tooltip("Transform positioned at Emma's spawn point beside the driver.")]
        [SerializeField] private Transform _driverSeatRightPosition;

        [Tooltip("Base cooldown between appearances. The GhostBase reset delay is added separately.")]
        [SerializeField] private float _baseCooldown = 18f;

        [Header("Day 1 onboarding")]
        [Tooltip("Reaction time used by the first canonical encounter with Emma.")]
        [SerializeField] [Min(2f)] private float _dayOneAttackWindow = 6f;

        [Tooltip("Time Emma remains partially manifested before the Day 1 attack begins.")]
        [SerializeField] [Min(0f)] private float _dayOneManifestationDelay = 2.5f;

        [Header("Advanced difficulty")]
        [Tooltip("Reaction time used from Day 2 onward.")]
        [SerializeField] [Min(0.5f)] private float _standardAttackWindow = 2f;

        [Tooltip("Manifestation delay used from Day 2 onward.")]
        [SerializeField] [Min(0f)] private float _standardManifestationDelay = 1.5f;

        /// <summary>
        /// Fired whenever Emma's laugh intensity changes.
        /// Values are 0 for soft, 1 for medium, and 2 for critical.
        /// </summary>
        public static event Action<int> OnLaugh;

        /// <summary>
        /// Fired after the day profile changes. The argument is the active attack window.
        /// UI and accessibility feedback can use this to remain synchronized with difficulty.
        /// </summary>
        public static event Action<float> OnAttackWindowChanged;

        private int _laughIntensity;
        private float _manifestationTimer;
        private float _activeManifestationDelay;

        public int LaughIntensity => _laughIntensity;
        public bool HasFullyManifested { get; private set; }
        public float CurrentAttackWindow => AttackWindow;
        public float CurrentManifestationDelay => _activeManifestationDelay;

        private void Awake()
        {
            GhostType = GhostType.Emma;
            ApplyDifficultyProfile(1);
        }

        protected override void Update()
        {
            base.Update();

            if (CurrentState == GhostState.Active)
            {
                _manifestationTimer -= Time.deltaTime;

                if (_manifestationTimer <= 0f)
                {
                    HasFullyManifested = true;
                    BeginAttack();
                }
            }

            if (CurrentState == GhostState.Attacking)
            {
                UpdateLaughIntensity();
            }
        }

        protected override void OnActivate()
        {
            if (_driverSeatRightPosition != null)
            {
                transform.position = _driverSeatRightPosition.position;
                transform.rotation = _driverSeatRightPosition.rotation;
            }

            _laughIntensity = 0;
            HasFullyManifested = false;
            _manifestationTimer = _activeManifestationDelay;
            OnLaugh?.Invoke(_laughIntensity);
        }

        protected override void OnAttack()
        {
            // GhostBase starts the configured attack window and emits OnAttackStarted.
        }

        protected override void OnDefeated()
        {
            _laughIntensity = 0;
            HasFullyManifested = false;
            _manifestationTimer = 0f;
        }

        protected override void OnObservationComplete()
        {
            _observationTimer = 0f;
            _isBeingObserved = false;

            if (!HasFullyManifested)
            {
                Despawn();
                BusShift.Core.GameManager.Instance?.SanitySystem?.AddTension(0.10f);
                return;
            }

            Despawn();
            BusShift.Core.GameManager.Instance?.SanitySystem?.ReduceTension(0.05f);
        }

        private void UpdateLaughIntensity()
        {
            float safeAttackWindow = Mathf.Max(AttackWindow, 0.01f);
            float elapsed = safeAttackWindow - _windowTimer;
            float segmentSize = safeAttackWindow / 3f;
            int newIntensity = Mathf.Clamp(
                Mathf.FloorToInt(elapsed / segmentSize),
                0,
                2);

            if (newIntensity == _laughIntensity)
            {
                return;
            }

            _laughIntensity = newIntensity;
            OnLaugh?.Invoke(_laughIntensity);
        }

        /// <summary>
        /// Applies Emma's day based profile.
        ///
        /// Day 1 preserves the six second tutorial window required by Build 0.1.0.
        /// Day 2 onward restores the intended two second advanced reaction window.
        /// </summary>
        public void SetDayDifficulty(int day)
        {
            ApplyDifficultyProfile(Mathf.Max(1, day));
        }

        private void ApplyDifficultyProfile(int day)
        {
            int dayIndex = Mathf.Max(0, day - 1);
            AttackInterval = Mathf.Max(8f, _baseCooldown - (dayIndex * 2f));

            bool isDayOne = day <= 1;
            AttackWindow = isDayOne
                ? Mathf.Max(2f, _dayOneAttackWindow)
                : Mathf.Max(0.5f, _standardAttackWindow);

            _activeManifestationDelay = isDayOne
                ? Mathf.Max(0f, _dayOneManifestationDelay)
                : Mathf.Max(0f, _standardManifestationDelay);

            OnAttackWindowChanged?.Invoke(AttackWindow);
        }
    }
}
