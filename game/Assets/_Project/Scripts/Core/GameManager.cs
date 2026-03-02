using UnityEngine;
using UnityEngine.Events;

namespace BusShift.Core
{
    public enum GameState { MainMenu, Loading, Playing, Paused, GameOver, Victory }

    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [Header("Systems")]
        public SanitySystem SanitySystem;
        public DayManager DayManager;

        public GameState CurrentState { get; private set; }

        public static event System.Action<GameState> OnGameStateChanged;
        public static event System.Action<int> OnDayChanged;

        void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public void SetState(GameState newState)
        {
            CurrentState = newState;
            OnGameStateChanged?.Invoke(newState);
        }

        public void StartGame()  => SetState(GameState.Playing);
        public void PauseGame()  => SetState(GameState.Paused);
        public void TriggerGameOver() => SetState(GameState.GameOver);
    }
}
