using System;
using UnityEngine;

namespace BusShift.Core
{
    /// <summary>
    /// Compatibility adapter for scenes and scripts that still reference GameStateManager.
    ///
    /// GameManager is the only authoritative state owner. This component keeps the richer
    /// previous and next state event used by legacy integrations without storing a second
    /// CurrentState value.
    /// </summary>
    [DisallowMultipleComponent]
    public class GameStateManager : MonoBehaviour
    {
        public static GameStateManager Instance { get; private set; }

        public GameState CurrentState =>
            GameManager.Instance != null
                ? GameManager.Instance.CurrentState
                : GameState.MainMenu;

        public static event Action<GameState, GameState> OnStateChanged;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void OnEnable()
        {
            GameManager.OnGameStateTransition += ForwardStateTransition;
        }

        private void OnDisable()
        {
            GameManager.OnGameStateTransition -= ForwardStateTransition;
        }

        private void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }

        public void SetState(GameState next)
        {
            GameManager manager = ResolveGameManager();

            if (manager == null)
            {
                Debug.LogError(
                    "[GameStateManager] GameManager is required because it owns the canonical game state.");
                return;
            }

            manager.SetState(next);
        }

        public void Pause()
        {
            GameManager manager = ResolveGameManager();

            if (manager == null)
            {
                Debug.LogError("[GameStateManager] Cannot pause without GameManager.");
                return;
            }

            manager.PauseGame();
        }

        public void Resume()
        {
            GameManager manager = ResolveGameManager();

            if (manager == null)
            {
                Debug.LogError("[GameStateManager] Cannot resume without GameManager.");
                return;
            }

            manager.ResumeGame();
        }

        public void GameOver(string reason)
        {
            GameManager manager = ResolveGameManager();

            if (manager == null)
            {
                Debug.LogError("[GameStateManager] Cannot trigger game over without GameManager.");
                return;
            }

            manager.TriggerGameOver(reason);
        }

        private static GameManager ResolveGameManager()
        {
            if (GameManager.Instance != null)
            {
                return GameManager.Instance;
            }

            return UnityEngine.Object.FindAnyObjectByType<GameManager>();
        }

        private static void ForwardStateTransition(GameState previous, GameState next)
        {
            OnStateChanged?.Invoke(previous, next);
        }

#if UNITY_EDITOR
        [ContextMenu("Debug, Set Playing")]
        private void EditorSetPlaying()
        {
            SetState(GameState.Playing);
        }

        [ContextMenu("Debug, Pause")]
        private void EditorPause()
        {
            Pause();
        }

        [ContextMenu("Debug, Resume")]
        private void EditorResume()
        {
            Resume();
        }

        [ContextMenu("Debug, Game Over")]
        private void EditorGameOver()
        {
            GameOver("Triggered from Editor context menu");
        }

        [ContextMenu("Debug, Victory")]
        private void EditorVictory()
        {
            SetState(GameState.Victory);
        }
#endif
    }
}
