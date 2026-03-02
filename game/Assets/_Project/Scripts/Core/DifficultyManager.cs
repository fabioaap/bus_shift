using System;
using UnityEngine;

namespace BusShift.Core
{
    // ─────────────────────────────────────────────────────────────────────────
    //  Data
    // ─────────────────────────────────────────────────────────────────────────

    /// <summary>
    /// Serializable difficulty snapshot for a single in-game day.
    /// Five instances are stored in <see cref="DifficultyManager._difficultyTable"/>
    /// and can be tweaked in the Inspector without recompiling.
    /// </summary>
    [Serializable]
    public struct DifficultyConfig
    {
        /// <summary>The day number this config applies to (1–5).</summary>
        public int Day;

        /// <summary>
        /// Minimum seconds between consecutive ghost attack attempts.
        /// Lower values create more aggressive, frequent attacks.
        /// </summary>
        public float GhostAttackInterval;

        /// <summary>
        /// Multiplier applied to every raw sanity drain value.
        /// 1.0 = baseline; 2.5 = Day 5 nightmare mode.
        /// </summary>
        public float SanityDrainMultiplier;

        /// <summary>
        /// Maximum number of ghosts that may be active at the same time during this period.
        /// </summary>
        public int MaxGhostsPerPeriod;
    }

    // ─────────────────────────────────────────────────────────────────────────
    //  Manager
    // ─────────────────────────────────────────────────────────────────────────

    /// <summary>
    /// Singleton MonoBehaviour that tracks and broadcasts the active difficulty
    /// configuration as the player advances through the five in-game days.
    ///
    /// <para>
    /// <b>Difficulty table (Inspector-editable defaults):</b>
    /// <list type="table">
    ///   <listheader><term>Day</term><term>Attack Interval</term><term>Sanity Drain ×</term><term>Max Ghosts</term></listheader>
    ///   <item><term>1</term><term>60 s</term><term>1.0×</term><term>1</term></item>
    ///   <item><term>2</term><term>50 s</term><term>1.2×</term><term>1</term></item>
    ///   <item><term>3</term><term>40 s</term><term>1.5×</term><term>2</term></item>
    ///   <item><term>4</term><term>30 s</term><term>1.8×</term><term>3</term></item>
    ///   <item><term>5</term><term>20 s</term><term>2.5×</term><term>5</term></item>
    /// </list>
    /// </para>
    ///
    /// <para>
    /// Subscribes to <see cref="DayManager.OnDayDifficultyChanged"/> and updates the
    /// active config automatically. Ghost systems should read
    /// <see cref="CurrentAttackInterval"/>, <see cref="CurrentSanityMultiplier"/>,
    /// and <see cref="MaxGhostsPerPeriod"/> — or subscribe to <see cref="OnDifficultyChanged"/>.
    /// </para>
    ///
    /// <para>
    /// Setup: Attach to a persistent GameObject alongside <see cref="DayManager"/>.
    /// </para>
    /// </summary>
    public class DifficultyManager : MonoBehaviour
    {
        // ─────────────────────────────────────────────────────────────────────
        //  Singleton
        // ─────────────────────────────────────────────────────────────────────

        /// <summary>Global singleton instance.</summary>
        public static DifficultyManager Instance { get; private set; }

        // ─────────────────────────────────────────────────────────────────────
        //  Static Events
        // ─────────────────────────────────────────────────────────────────────

        /// <summary>
        /// Fired whenever the active difficulty config is updated (once per day change).
        /// Provides the full <see cref="DifficultyConfig"/> so subscribers can cache
        /// all three values in a single call.
        /// </summary>
        public static event Action<DifficultyConfig> OnDifficultyChanged;

        // ─────────────────────────────────────────────────────────────────────
        //  Inspector
        // ─────────────────────────────────────────────────────────────────────

        [Header("Difficulty Table (Days 1 – 5)")]
        [Tooltip("One entry per day. Day field must match the DayManager day number (1-5). " +
                 "Entries are searched by Day value, not array index.")]
        [SerializeField] private DifficultyConfig[] _difficultyTable = new DifficultyConfig[]
        {
            new DifficultyConfig { Day = 1, GhostAttackInterval = 60f, SanityDrainMultiplier = 1.0f, MaxGhostsPerPeriod = 1 },
            new DifficultyConfig { Day = 2, GhostAttackInterval = 50f, SanityDrainMultiplier = 1.2f, MaxGhostsPerPeriod = 1 },
            new DifficultyConfig { Day = 3, GhostAttackInterval = 40f, SanityDrainMultiplier = 1.5f, MaxGhostsPerPeriod = 2 },
            new DifficultyConfig { Day = 4, GhostAttackInterval = 30f, SanityDrainMultiplier = 1.8f, MaxGhostsPerPeriod = 3 },
            new DifficultyConfig { Day = 5, GhostAttackInterval = 20f, SanityDrainMultiplier = 2.5f, MaxGhostsPerPeriod = 5 },
        };

        // ─────────────────────────────────────────────────────────────────────
        //  Private State
        // ─────────────────────────────────────────────────────────────────────

        private DifficultyConfig _currentConfig;

        // ─────────────────────────────────────────────────────────────────────
        //  Public Properties
        // ─────────────────────────────────────────────────────────────────────

        /// <summary>
        /// Minimum seconds between ghost attack attempts for the current day.
        /// Ghost spawn controllers should use this as their timer reset value.
        /// </summary>
        public float CurrentAttackInterval => _currentConfig.GhostAttackInterval;

        /// <summary>
        /// Multiplier applied to every sanity drain event for the current day.
        /// Pass to <see cref="SanitySystem.AddTension"/> call-sites:
        /// <c>sanitySystem.AddTension(baseDrain * DifficultyManager.Instance.CurrentSanityMultiplier);</c>
        /// </summary>
        public float CurrentSanityMultiplier => _currentConfig.SanityDrainMultiplier;

        /// <summary>
        /// Hard cap on simultaneous active ghosts during the current period.
        /// Ghost spawn controllers should not spawn beyond this count.
        /// </summary>
        public int MaxGhostsPerPeriod => _currentConfig.MaxGhostsPerPeriod;

        // ─────────────────────────────────────────────────────────────────────
        //  Lifecycle
        // ─────────────────────────────────────────────────────────────────────

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Boot with Day 1 defaults so properties are safe before the first event fires
            _currentConfig = GetConfigForDay(1);
        }

        private void OnEnable()
        {
            DayManager.OnDayDifficultyChanged += HandleDayDifficultyChanged;
        }

        private void OnDisable()
        {
            DayManager.OnDayDifficultyChanged -= HandleDayDifficultyChanged;
        }

        // ─────────────────────────────────────────────────────────────────────
        //  Public API
        // ─────────────────────────────────────────────────────────────────────

        /// <summary>
        /// Returns the <see cref="DifficultyConfig"/> for the requested day.
        /// <para>
        /// Matching is done by <see cref="DifficultyConfig.Day"/> field value, not array index,
        /// so holes in the table are handled gracefully.
        /// Falls back to the last table entry if no exact match is found.
        /// </para>
        /// </summary>
        /// <param name="day">Day number (1–5).</param>
        /// <returns>The matching <see cref="DifficultyConfig"/> or a safe fallback.</returns>
        public DifficultyConfig GetConfigForDay(int day)
        {
            if (_difficultyTable == null || _difficultyTable.Length == 0)
            {
                Debug.LogWarning("[DifficultyManager] Difficulty table is empty — returning safe default config.");
                return new DifficultyConfig
                {
                    Day                   = day,
                    GhostAttackInterval   = 60f,
                    SanityDrainMultiplier = 1f,
                    MaxGhostsPerPeriod    = 1
                };
            }

            // Linear search: table is tiny (5 entries) so O(n) is negligible
            foreach (DifficultyConfig config in _difficultyTable)
            {
                if (config.Day == day)
                    return config;
            }

            // Fallback: clamp to closest boundary entry
            int clampedIndex = Mathf.Clamp(day - 1, 0, _difficultyTable.Length - 1);
            return _difficultyTable[clampedIndex];
        }

        // ─────────────────────────────────────────────────────────────────────
        //  Event Handlers
        // ─────────────────────────────────────────────────────────────────────

        /// <summary>
        /// Handles <see cref="DayManager.OnDayDifficultyChanged"/>.
        /// Looks up the config for the new day and fires <see cref="OnDifficultyChanged"/>.
        /// </summary>
        private void HandleDayDifficultyChanged(int day)
        {
            _currentConfig = GetConfigForDay(day);
            OnDifficultyChanged?.Invoke(_currentConfig);

#if UNITY_EDITOR || DEVELOPMENT_BUILD
            Debug.Log($"[DifficultyManager] Day {day} activated — " +
                      $"attackInterval={_currentConfig.GhostAttackInterval}s, " +
                      $"sanityMult={_currentConfig.SanityDrainMultiplier:F1}×, " +
                      $"maxGhosts={_currentConfig.MaxGhostsPerPeriod}.");
#endif
        }

        // ─────────────────────────────────────────────────────────────────────
        //  Editor Helpers
        // ─────────────────────────────────────────────────────────────────────

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_difficultyTable == null) return;

            for (int i = 0; i < _difficultyTable.Length; i++)
            {
                DifficultyConfig c = _difficultyTable[i];
                c.Day                   = Mathf.Max(1, c.Day);
                c.GhostAttackInterval   = Mathf.Max(1f, c.GhostAttackInterval);
                c.SanityDrainMultiplier = Mathf.Max(0.1f, c.SanityDrainMultiplier);
                c.MaxGhostsPerPeriod    = Mathf.Max(1, c.MaxGhostsPerPeriod);
                _difficultyTable[i]     = c;
            }
        }
#endif
    }
}
