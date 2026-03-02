using UnityEngine;
using BusShift.Core;

namespace BusShift.Bus
{
    /// <summary>
    /// Manages the bus fuel tank over the course of a shift.
    ///
    /// <para>
    /// Consumption rules (applied every frame while the engine is running):
    /// <list type="bullet">
    ///   <item><b>Moving</b>  – <see cref="FuelConsumptionRate"/> units/s
    ///         (when <see cref="Rigidbody.linearVelocity"/> magnitude &gt; <see cref="MovementThreshold"/>).</item>
    ///   <item><b>Idle</b>    – <see cref="IdleConsumptionRate"/> units/s (engine on, bus stopped).</item>
    /// </list>
    /// </para>
    ///
    /// <para>
    /// The engine is considered <em>running</em> while <see cref="GameState.Playing"/> is active.
    /// When fuel hits zero <see cref="GameManager.TriggerGameOver"/> is called exactly once and
    /// the static event <see cref="OnFuelEmpty"/> is broadcast.
    /// </para>
    ///
    /// <para>
    /// Between days call <see cref="Refuel"/> or <see cref="RefuelFull"/> to restore the tank.
    /// </para>
    /// </summary>
    [RequireComponent(typeof(BusController))]
    public class FuelSystem : MonoBehaviour
    {
        // ─────────────────────────────────────────────────────────────────────
        //  Static Events
        // ─────────────────────────────────────────────────────────────────────

        /// <summary>
        /// Broadcast whenever the fuel level changes.
        /// Argument is the normalised fuel percentage in the range [0, 1].
        /// </summary>
        public static event System.Action<float> OnFuelChanged;

        /// <summary>
        /// Broadcast once when the tank reaches exactly zero.
        /// Fired before <see cref="GameManager.TriggerGameOver"/> is called.
        /// </summary>
        public static event System.Action OnFuelEmpty;

        // ─────────────────────────────────────────────────────────────────────
        //  Inspector
        // ─────────────────────────────────────────────────────────────────────

        [Header("Tank")]
        [Tooltip("Maximum fuel capacity (full tank = 100 units).")]
        [SerializeField] private float _maxFuel = 100f;

        [Tooltip("Amount restored by RefuelFull() — called between days.")]
        [SerializeField] private float _refuelAmount = 100f;

        [Header("Consumption")]
        [Tooltip("Fuel units consumed per second while the bus is in motion.")]
        [SerializeField] private float _fuelConsumptionRate = 0.5f;

        [Tooltip("Fuel units consumed per second while the bus is stopped but the engine is running.")]
        [SerializeField] private float _idleConsumptionRate = 0.05f;

        [Tooltip("Rigidbody.linearVelocity.magnitude threshold above which the bus is considered moving.")]
        [SerializeField] private float _movementThreshold = 0.5f;

        [Header("Bus Physics Reference")]
        [Tooltip("Rigidbody of the bus. Auto-resolved from this GameObject if left empty.")]
        [SerializeField] private Rigidbody _busRigidbody;

        // ─────────────────────────────────────────────────────────────────────
        //  Private state
        // ─────────────────────────────────────────────────────────────────────

        private float _currentFuel;
        private bool  _engineRunning;
        private bool  _gameOverTriggered;

        // ─────────────────────────────────────────────────────────────────────
        //  Public API — Properties
        // ─────────────────────────────────────────────────────────────────────

        /// <summary>Current fuel level in raw units (0 – <see cref="MaxFuel"/>).</summary>
        public float CurrentFuel => _currentFuel;

        /// <summary>Maximum tank capacity.</summary>
        public float MaxFuel => _maxFuel;

        /// <summary><c>true</c> when the tank is completely empty.</summary>
        public bool IsEmpty => _currentFuel <= 0f;

        /// <summary>Normalised fuel level in the range [0, 1]. Use this for UI.</summary>
        public float FuelPercent => _maxFuel > 0f ? _currentFuel / _maxFuel : 0f;

        // ─────────────────────────────────────────────────────────────────────
        //  Lifecycle
        // ─────────────────────────────────────────────────────────────────────

        private void Awake()
        {
            if (_busRigidbody == null)
                _busRigidbody = GetComponent<Rigidbody>();

            _currentFuel = _maxFuel;
        }

        private void OnEnable()
        {
            GameManager.OnGameStateChanged += HandleGameStateChanged;
        }

        private void OnDisable()
        {
            GameManager.OnGameStateChanged -= HandleGameStateChanged;
        }

        private void Update()
        {
            if (!_engineRunning || _gameOverTriggered) return;

            ConsumeFuel();
        }

        // ─────────────────────────────────────────────────────────────────────
        //  Public API — Methods
        // ─────────────────────────────────────────────────────────────────────

        /// <summary>
        /// Adds <paramref name="amount"/> fuel to the tank, capped at <see cref="MaxFuel"/>.
        /// Call this between days to restore the tank.
        /// Clears the game-over flag so the system resumes normally.
        /// </summary>
        /// <param name="amount">Fuel units to add. Negative values are ignored.</param>
        public void Refuel(float amount)
        {
            if (amount <= 0f) return;

            _currentFuel       = Mathf.Min(_maxFuel, _currentFuel + amount);
            _gameOverTriggered = false;

            OnFuelChanged?.Invoke(FuelPercent);
        }

        /// <summary>
        /// Convenience wrapper — restores <see cref="_refuelAmount"/> units (full tank by default).
        /// Typically called by the day-change system between shifts.
        /// </summary>
        public void RefuelFull() => Refuel(_refuelAmount);

        // ─────────────────────────────────────────────────────────────────────
        //  Private helpers
        // ─────────────────────────────────────────────────────────────────────

        private void ConsumeFuel()
        {
            bool isMoving = _busRigidbody != null &&
                            _busRigidbody.linearVelocity.magnitude > _movementThreshold;

            float rate = isMoving ? _fuelConsumptionRate : _idleConsumptionRate;

            _currentFuel = Mathf.Max(0f, _currentFuel - rate * Time.deltaTime);

            OnFuelChanged?.Invoke(FuelPercent);

            if (_currentFuel <= 0f)
                TriggerFuelGameOver();
        }

        private void TriggerFuelGameOver()
        {
            if (_gameOverTriggered) return;

            _gameOverTriggered = true;
            _engineRunning     = false;

            OnFuelEmpty?.Invoke();

#if UNITY_EDITOR || DEVELOPMENT_BUILD
            Debug.Log("[FuelSystem] Tank is empty — triggering Game Over.");
#endif

            GameManager.Instance?.TriggerGameOver();
        }

        private void HandleGameStateChanged(GameState state)
        {
            // Engine is running only while the game is actively playing.
            _engineRunning = state == GameState.Playing;
        }

        // ─────────────────────────────────────────────────────────────────────
        //  Editor helpers
        // ─────────────────────────────────────────────────────────────────────

#if UNITY_EDITOR
        private void OnValidate()
        {
            _maxFuel              = Mathf.Max(1f,  _maxFuel);
            _refuelAmount         = Mathf.Max(0f,  _refuelAmount);
            _fuelConsumptionRate  = Mathf.Max(0f,  _fuelConsumptionRate);
            _idleConsumptionRate  = Mathf.Max(0f,  _idleConsumptionRate);
            _movementThreshold    = Mathf.Max(0f,  _movementThreshold);
        }
#endif
    }
}
