using System;
using UnityEngine;

namespace BusShift.Core
{
    public enum GameState
    {
        MainMenu,
        Loading,
        Playing,
        Paused,
        GameOver,
        Victory
    }

    /// <summary>
    /// Authoritative owner of game wide state and persistent core systems.
    ///
    /// All state transitions must pass through this component. GameStateManager remains
    /// as a compatibility adapter for legacy scene wiring and forwards to this class.
    /// </summary>
    [DisallowMultipleComponent]
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [Header("Systems")]
        public SanitySystem SanitySystem;
        public DayManager DayManager;

        [Header("Initial state")]
        [SerializeField] private GameState _initialState = GameState.MainMenu;

        public GameState CurrentState { get; private set; } = GameState.MainMenu;
        public string LastGameOverReason { get; private set; } = string.Empty;

        /// <summary>Legacy event containing only the new state.</summary>
        public static event Action<GameState> OnGameStateChanged;

        /// <summary>Canonical event containing the previous and next states.</summary>
        public static event Action<GameState, GameState> OnGameStateTransition;

        public static event Action<int> OnDayChanged;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            CurrentState = _initialState;
            ApplyTimeScale(CurrentState);
        }

        private void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
                Time.timeScale = 1f;
            }
        }

        /// <summary>
        /// Commits a game state transition and applies the canonical time scale.
        /// Repeating the active state is intentionally ignored.
        /// </summary>
        public void SetState(GameState newState)
        {
            GameState previousState = CurrentState;

            if (previousState == newState)
            {
                return;
            }

            CurrentState = newState;
            ApplyTimeScale(newState);

            OnGameStateTransition?.Invoke(previousState, newState);
            OnGameStateChanged?.Invoke(newState);

#if UNITY_EDITOR || DEVELOPMENT_BUILD
            Debug.Log(
                $"[GameManager] {previousState} -> {newState} " +
                $"(timeScale={Time.timeScale})");
#endif
        }

        public void StartGame()
        {
            LastGameOverReason = string.Empty;
            SetState(GameState.Playing);
        }

        public void PauseGame()
        {
            if (CurrentState == GameState.Playing)
            {
                SetState(GameState.Paused);
            }
        }

        public void ResumeGame()
        {
            if (CurrentState == GameState.Paused)
            {
                SetState(GameState.Playing);
            }
        }

        public void TriggerGameOver()
        {
            TriggerGameOver(string.Empty);
        }

        public void TriggerGameOver(string reason)
        {
            if (CurrentState == GameState.GameOver)
            {
                return;
            }

            LastGameOverReason = reason ?? string.Empty;

            if (!string.IsNullOrWhiteSpace(LastGameOverReason))
            {
                Debug.Log($"[GameManager] Game Over: {LastGameOverReason}");
            }

            SetState(GameState.GameOver);
        }

        public void TriggerVictory()
        {
            SetState(GameState.Victory);
        }

        public void ReturnToMainMenu()
        {
            LastGameOverReason = string.Empty;
            SetState(GameState.MainMenu);
        }

        private static void ApplyTimeScale(GameState state)
        {
            Time.timeScale = state == GameState.Paused ? 0f : 1f;
        }
    }
}
