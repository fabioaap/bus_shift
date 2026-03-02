using UnityEngine;

namespace BusShift.Interventions
{
    /// <summary>
    /// Radio countermeasure — press R to toggle the bus radio on/off.
    /// While the radio is playing, Thomas (CRIANÇA 3) is neutralised.
    /// Cooldown: 25 seconds after the radio is turned off.
    /// Integrates with an AudioSource (volume 0 → 1 when active).
    /// Issue #23.
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public class RadioSystem : InterventionBase
    {
        // ── Configuration ──────────────────────────────────────────────────────

        [Header("Radio Settings")]
        [Tooltip("Volume the AudioSource is set to while the radio is playing.")]
        [SerializeField] [Range(0f, 1f)] private float _playVolume = 1f;

        // ── State ───────────────────────────────────────────────────────────────

        private AudioSource _audioSource;

        // ── Public API ──────────────────────────────────────────────────────────

        /// <summary>True while the radio is playing music.</summary>
        public bool IsPlaying => _audioSource != null && _audioSource.isPlaying;

        // ── Events ──────────────────────────────────────────────────────────────

        /// <summary>Fired when the radio turns on.</summary>
        public static event System.Action OnRadioTurnedOn;

        /// <summary>Fired when the radio turns off (before cooldown begins).</summary>
        public static event System.Action OnRadioTurnedOff;

        // ── Unity Lifecycle ─────────────────────────────────────────────────────

        private void Awake()
        {
            triggerKey       = KeyCode.R;
            cooldownDuration = 25f;

            _audioSource        = GetComponent<AudioSource>();
            _audioSource.volume = 0f;
            _audioSource.Stop();
        }

        protected override void Update()
        {
            base.Update(); // tick cooldown

            HandleInput();
        }

        // ── InterventionBase Contract ────────────────────────────────────────────

        /// <inheritdoc/>
        public override bool CanActivate() => !IsOnCooldown || IsPlaying;

        /// <inheritdoc/>
        public override void Activate()
        {
            if (IsPlaying)
            {
                TurnOff();
            }
            else if (!IsOnCooldown)
            {
                TurnOn();
            }
        }

        // ── Helpers ─────────────────────────────────────────────────────────────

        private void HandleInput()
        {
            if (Input.GetKeyDown(triggerKey))
            {
                Activate();
            }
        }

        private void TurnOn()
        {
            _audioSource.volume = _playVolume;
            _audioSource.Play();
            OnRadioTurnedOn?.Invoke();
        }

        private void TurnOff()
        {
            _audioSource.volume = 0f;
            _audioSource.Stop();
            OnRadioTurnedOff?.Invoke();
            StartCooldown();
        }
    }
}
