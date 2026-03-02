using UnityEngine;

namespace BusShift.Interventions
{
    /// <summary>
    /// Microphone countermeasure — hold Q for 3 seconds to use the intercom.
    /// Effective against CRIANÇA 1 and CRIANÇA 5.
    /// Releasing Q before the 3-second charge cancels the action.
    /// Cooldown: 20 seconds after successful use.
    /// Issue #23.
    /// </summary>
    public class MicrophoneSystem : InterventionBase
    {
        // ── Configuration ──────────────────────────────────────────────────────

        [Header("Microphone Settings")]
        [Tooltip("Hold duration required to fire the microphone action.")]
        [SerializeField] private float _chargeRequiredSeconds = 3f;

        // ── State ───────────────────────────────────────────────────────────────

        private float _chargeTimer = 0f;

        // ── Public API ──────────────────────────────────────────────────────────

        /// <summary>True while the player is holding Q and charging the microphone.</summary>
        public bool IsCharging => _chargeTimer > 0f;

        /// <summary>Charge progress from 0 (not started) to 1 (fully charged).</summary>
        public float ChargeProgress => _chargeRequiredSeconds > 0f
            ? Mathf.Clamp01(_chargeTimer / _chargeRequiredSeconds)
            : 0f;

        // ── Events ──────────────────────────────────────────────────────────────

        /// <summary>Fired when the player successfully holds Q for the full charge duration.</summary>
        public static event System.Action OnMicrophoneUsed;

        /// <summary>Fired when charging is cancelled (key released early).</summary>
        public static event System.Action OnMicrophoneChargeCancelled;

        // ── Unity Lifecycle ─────────────────────────────────────────────────────

        private void Awake()
        {
            triggerKey       = KeyCode.Q;
            cooldownDuration = 20f;
        }

        protected override void Update()
        {
            base.Update(); // tick cooldown

            HandleInput();
        }

        // ── InterventionBase Contract ────────────────────────────────────────────

        /// <inheritdoc/>
        public override bool CanActivate() => !IsOnCooldown;

        /// <inheritdoc/>
        public override void Activate()
        {
            // Activation is fired internally after charge completes — no external call needed.
            // This method satisfies the abstract contract and can be used for forced activation.
            FireMicrophone();
        }

        // ── Helpers ─────────────────────────────────────────────────────────────

        private void HandleInput()
        {
            if (!CanActivate()) return;

            if (Input.GetKey(triggerKey))
            {
                _chargeTimer += Time.deltaTime;

                if (_chargeTimer >= _chargeRequiredSeconds)
                {
                    _chargeTimer = 0f;
                    FireMicrophone();
                }
            }
            else if (Input.GetKeyUp(triggerKey) && _chargeTimer > 0f)
            {
                // Player released early — cancel charge
                _chargeTimer = 0f;
                OnMicrophoneChargeCancelled?.Invoke();
            }
        }

        private void FireMicrophone()
        {
            OnMicrophoneUsed?.Invoke();
            StartCooldown();
        }
    }
}
