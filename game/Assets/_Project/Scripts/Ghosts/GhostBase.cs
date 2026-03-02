using UnityEngine;

namespace BusShift.Ghosts
{
    public enum GhostState { Idle, Active, Attacking, Defeated }
    public enum GhostType  { Marcus, Emma, Thomas, Grace, Oliver }

    public abstract class GhostBase : MonoBehaviour
    {
        [Header("Config")]
        public GhostType GhostType;
        public float AttackInterval = 30f;
        public float AttackWindow   = 3f;   // seconds player has to react

        // ── Observation ───────────────────────────────────────────────────────

        [Header("Observation")]
        [Tooltip("How many seconds the player must observe this ghost before OnObservationComplete fires.")]
        [SerializeField] protected float observationTimeToVanish = 3f;

        /// <summary>Accumulated seconds the player has been observing this ghost.</summary>
        protected float _observationTimer;

        /// <summary>True while <see cref="GhostVisibilityChecker"/> reports this ghost is visible.</summary>
        protected bool _isBeingObserved;

        // ─────────────────────────────────────────────────────────────────────

        public GhostState CurrentState { get; protected set; } = GhostState.Idle;
        public bool IsVulnerable { get; protected set; }

        protected float _attackTimer;
        protected float _windowTimer;

        public static event System.Action<GhostType> OnAttackStarted;
        public static event System.Action<GhostType> OnAttackDefeated;
        public static event System.Action<GhostType> OnGameOverTriggered;

        protected virtual void Update()
        {
            if (CurrentState == GhostState.Idle)
            {
                _attackTimer -= Time.deltaTime;
                if (_attackTimer <= 0f) Activate();
            }
            else if (CurrentState == GhostState.Attacking)
            {
                _windowTimer -= Time.deltaTime;
                if (_windowTimer <= 0f) TriggerGameOver();
            }

            TickObservation();
        }

        public void Activate()
        {
            CurrentState = GhostState.Active;
            _attackTimer = AttackInterval;
            OnActivate();
        }

        public void Defeat()
        {
            CurrentState = GhostState.Defeated;
            IsVulnerable = false;
            OnDefeated();
            OnAttackDefeated?.Invoke(GhostType);
            Invoke(nameof(ResetToIdle), 2f);
        }

        protected void BeginAttack()
        {
            CurrentState = GhostState.Attacking;
            IsVulnerable = true;
            _windowTimer  = AttackWindow;
            OnAttack();
            OnAttackStarted?.Invoke(GhostType);
        }

        protected void TriggerGameOver()
        {
            CurrentState = GhostState.Idle;
            OnGameOverTriggered?.Invoke(GhostType);
            BusShift.Core.GameManager.Instance?.TriggerGameOver();
        }

        private void ResetToIdle()
        {
            CurrentState = GhostState.Idle;
            _attackTimer  = AttackInterval;
        }

        // ── Observation API ───────────────────────────────────────────────────

        /// <summary>
        /// Called by <see cref="GhostVisibilityChecker"/> each visibility poll cycle.
        /// When <paramref name="observed"/> transitions to <c>false</c> the accumulated
        /// timer is reset so partial observations do not stack across separate glances.
        /// </summary>
        /// <param name="observed"><c>true</c> if the ghost is inside an active camera frustum.</param>
        public virtual void SetObserved(bool observed)
        {
            _isBeingObserved = observed;
            if (!observed) _observationTimer = 0f;
        }

        /// <summary>
        /// Advances the observation timer while the ghost is being observed.
        /// Calls <see cref="OnObservationComplete"/> once the threshold is reached.
        /// Skipped when the ghost is in the <see cref="GhostState.Defeated"/> state.
        /// Subclasses may also call this manually in their own Update if needed.
        /// </summary>
        protected void TickObservation()
        {
            if (!_isBeingObserved) return;
            if (CurrentState == GhostState.Defeated) return;

            _observationTimer += Time.deltaTime;
            if (_observationTimer >= observationTimeToVanish)
                OnObservationComplete();
        }

        /// <summary>
        /// Invoked when the player has observed this ghost for
        /// <see cref="observationTimeToVanish"/> continuous seconds.
        /// Override in subclasses for ghost-specific reactions.
        /// </summary>
        protected virtual void OnObservationComplete()
        {
            _observationTimer = 0f;
            _isBeingObserved  = false;
            Despawn();
        }

        /// <summary>
        /// Forces the ghost to exit the scene immediately (equivalent to a
        /// player-triggered defeat). Fires <see cref="OnAttackDefeated"/> and
        /// schedules a return to the Idle state after a short delay.
        /// </summary>
        protected void Despawn() => Defeat();

        // ─────────────────────────────────────────────────────────────────────

        protected abstract void OnActivate();
        protected abstract void OnAttack();
        protected abstract void OnDefeated();
    }
}
