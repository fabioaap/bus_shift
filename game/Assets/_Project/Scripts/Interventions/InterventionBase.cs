using UnityEngine;

namespace BusShift.Interventions
{
    /// <summary>
    /// Abstract base class for all player interventions (countermeasures).
    /// Handles cooldown lifecycle and exposes a common contract for all intervention types.
    /// </summary>
    public abstract class InterventionBase : MonoBehaviour
    {
        // ── Configuration ──────────────────────────────────────────────────────

        [Header("Intervention Settings")]
        [Tooltip("The keyboard key that triggers this intervention.")]
        [SerializeField] protected KeyCode triggerKey = KeyCode.None;

        [Tooltip("Cooldown duration in seconds after the intervention is used.")]
        [SerializeField] protected float cooldownDuration = 20f;

        // ── State ───────────────────────────────────────────────────────────────

        private float _cooldownTimer = 0f;

        // ── Public API ──────────────────────────────────────────────────────────

        /// <summary>Keyboard key that triggers this intervention.</summary>
        public KeyCode TriggerKey => triggerKey;

        /// <summary>Cooldown length in seconds configured for this intervention.</summary>
        public float CooldownDuration => cooldownDuration;

        /// <summary>True while the intervention is still cooling down.</summary>
        public bool IsOnCooldown => _cooldownTimer > 0f;

        /// <summary>Remaining cooldown time in seconds (0 when ready).</summary>
        public float CooldownRemaining => Mathf.Max(0f, _cooldownTimer);

        // ── Events ──────────────────────────────────────────────────────────────

        /// <summary>Fired when cooldown begins. Passes the total cooldown duration.</summary>
        public event System.Action<float> OnCooldownStarted;

        /// <summary>Fired when cooldown expires and the intervention is ready again.</summary>
        public event System.Action OnCooldownEnded;

        // ── Abstract Contract ────────────────────────────────────────────────────

        /// <summary>
        /// Returns true when the intervention may be triggered
        /// (not on cooldown and any other preconditions are met).
        /// </summary>
        public abstract bool CanActivate();

        /// <summary>Executes the intervention logic.</summary>
        public abstract void Activate();

        // ── Unity Lifecycle ─────────────────────────────────────────────────────

        protected virtual void Update()
        {
            TickCooldown();
        }

        // ── Helpers ─────────────────────────────────────────────────────────────

        /// <summary>
        /// Starts the cooldown counter.
        /// Call this at the end of the intervention's active phase.
        /// </summary>
        protected void StartCooldown()
        {
            _cooldownTimer = cooldownDuration;
            OnCooldownStarted?.Invoke(cooldownDuration);
        }

        /// <summary>Advances the cooldown timer by one frame and fires OnCooldownEnded when done.</summary>
        private void TickCooldown()
        {
            if (_cooldownTimer <= 0f) return;

            _cooldownTimer -= Time.deltaTime;

            if (_cooldownTimer <= 0f)
            {
                _cooldownTimer = 0f;
                OnCooldownEnded?.Invoke();
            }
        }
    }
}
