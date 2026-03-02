using UnityEngine;

namespace BusShift.Core
{
    /// <summary>
    /// In-game time system for Bus Shift.
    /// Tracks game time independently from real time with a configurable speed multiplier.
    /// Session target: ~60 game minutes in ~7 real minutes (≈8.57 game-seconds per real-second).
    /// </summary>
    public class TimerSystem : MonoBehaviour
    {
        // ── Configuration ──────────────────────────────────────────────────────

        [Header("Time Settings")]
        [Tooltip("Game seconds elapsed per real second. Default 8.57 ≈ 60 game-min in 7 real-min.")]
        [SerializeField] private float _gameSecondsPerRealSecond = 8.57f;

        [Tooltip("Starting game time in seconds. 21600 = 06:00 (morning). 64800 = 18:00 (night).")]
        [SerializeField] private float _startTimeSeconds = 21600f; // 06:00

        [Tooltip("Game time (seconds) at which OnTimeLimitReached fires. 28800 = 08:00.")]
        [SerializeField] private float _arrivalTimeSeconds = 28800f; // 08:00

        // ── State ───────────────────────────────────────────────────────────────

        private bool _isPaused = false;
        private bool _timeLimitReached = false;

        // ── Public API ──────────────────────────────────────────────────────────

        /// <summary>Current in-game time in seconds since midnight.</summary>
        public float GameTimeSeconds { get; private set; }

        /// <summary>Game seconds that advance per real-world second (read-only at runtime).</summary>
        public float GameSecondsPerRealSecond => _gameSecondsPerRealSecond;

        /// <summary>Returns the current time formatted as "HH:mm".</summary>
        public string FormattedTime
        {
            get
            {
                int totalMinutes = Mathf.FloorToInt(GameTimeSeconds / 60f);
                int hours   = (totalMinutes / 60) % 24;
                int minutes = totalMinutes % 60;
                return $"{hours:D2}:{minutes:D2}";
            }
        }

        /// <summary>True when in-game time is before 12:00 (noon).</summary>
        public bool IsMorning => GameTimeSeconds < 43200f; // 12:00 = 43200s

        /// <summary>
        /// True when the bus is running behind schedule.
        /// Set this from RouteManager or BusStopSystem when stops are missed/delayed.
        /// OliverGhost doubles his tension rate while this is true.
        /// </summary>
        public bool IsLate { get; set; }

        /// <summary>True when the timer is paused (e.g. menu open).</summary>
        public bool IsPaused => _isPaused;

        // ── Events ──────────────────────────────────────────────────────────────

        /// <summary>Fired once when game time reaches the configured arrival time.</summary>
        public static event System.Action OnTimeLimitReached;

        /// <summary>Fired every frame the time advances. Passes current formatted time.</summary>
        public static event System.Action<string> OnTimeChanged;

        // ── Unity Lifecycle ─────────────────────────────────────────────────────

        private void Awake()
        {
            GameTimeSeconds = _startTimeSeconds;
        }

        private void Update()
        {
            if (_isPaused || _timeLimitReached) return;

            GameTimeSeconds += _gameSecondsPerRealSecond * Time.deltaTime;

            OnTimeChanged?.Invoke(FormattedTime);

            if (!_timeLimitReached && GameTimeSeconds >= _arrivalTimeSeconds)
            {
                _timeLimitReached = true;
                OnTimeLimitReached?.Invoke();
            }
        }

        // ── Control ─────────────────────────────────────────────────────────────

        /// <summary>Pauses the in-game clock (use for menus, cutscenes).</summary>
        public void Pause() => _isPaused = true;

        /// <summary>Resumes the in-game clock.</summary>
        public void Resume() => _isPaused = false;

        /// <summary>Resets the clock to the configured start time and clears the limit flag.</summary>
        public void ResetTimer()
        {
            GameTimeSeconds    = _startTimeSeconds;
            _timeLimitReached  = false;
            _isPaused          = false;
        }

        /// <summary>Overrides the current in-game time (for debugging / cutscenes).</summary>
        public void SetTime(float gameTimeInSeconds)
        {
            GameTimeSeconds = Mathf.Clamp(gameTimeInSeconds, 0f, 86400f);
        }
    }
}
