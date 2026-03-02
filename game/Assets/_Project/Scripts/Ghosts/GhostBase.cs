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

        protected abstract void OnActivate();
        protected abstract void OnAttack();
        protected abstract void OnDefeated();
    }
}
