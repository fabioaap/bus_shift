using System;
using UnityEngine;

namespace BusShift.Core
{
    /// <summary>
    /// Singleton Finite-State Machine that governs game-wide state transitions.
    ///
    /// <para>
    /// Extends the lightweight state-set in <see cref="GameManager"/> with:
    /// <list type="bullet">
    ///   <item>A richer <see cref="OnStateChanged"/> event that carries both the previous
    ///         and the next <see cref="GameState"/> values.</item>
    ///   <item>Automatic <see cref="Time.timeScale"/> management:
    ///         <c>0</c> while <see cref="GameState.Paused"/>, <c>1</c> for all other states.</item>
    ///   <item>Convenience helpers: <see cref="Pause"/>, <see cref="Resume"/>,
    ///         <see cref="GameOver(string)"/>.</item>
    /// </list>
    /// </para>
    ///
    /// <para>
    /// <b>GameState enum</b> is declared in <c>GameManager.cs</c> (same namespace) and
    /// shared across both managers:
    /// <c>MainMenu → Loading → Playing → Paused / GameOver / Victory</c>
    /// </para>
    ///
    /// <para>
    /// Attach to a persistent GameObject (or the same one as <see cref="GameManager"/>).
    /// Only one instance is allowed; duplicates are automatically destroyed.
    /// </para>
    /// </summary>
    public class GameStateManager : MonoBehaviour
    {
        // ─────────────────────────────────────────────────────────────────────
        //  Singleton
        // ─────────────────────────────────────────────────────────────────────

        /// <summary>The single active instance in the scene.</summary>
        public static GameStateManager Instance { get; private set; }

        // ─────────────────────────────────────────────────────────────────────
        //  State
        // ─────────────────────────────────────────────────────────────────────

        /// <summary>Currently active game state. Starts at <see cref="GameState.MainMenu"/>.</summary>
        public GameState CurrentState { get; private set; } = GameState.MainMenu;

        // ─────────────────────────────────────────────────────────────────────
        //  Events
        // ─────────────────────────────────────────────────────────────────────

        /// <summary>
        /// Fired immediately after a state transition is committed.
        /// Parameters: (previous state, new state).
        /// </summary>
        public static event Action<GameState, GameState> OnStateChanged;

        // ─────────────────────────────────────────────────────────────────────
        //  Unity Lifecycle
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
        }

        // ─────────────────────────────────────────────────────────────────────
        //  Public API
        // ─────────────────────────────────────────────────────────────────────

        /// <summary>
        /// Transitions to <paramref name="next"/> if it differs from <see cref="CurrentState"/>.
        /// Fires <see cref="OnStateChanged"/> and updates <see cref="Time.timeScale"/>.
        /// </summary>
        /// <param name="next">Target game state.</param>
        public void SetState(GameState next)
        {
            GameState prev = CurrentState;
            if (prev == next) return;

            CurrentState = next;
            OnStateChanged?.Invoke(prev, next);
            ApplyTimeScale(next);

#if UNITY_EDITOR || DEVELOPMENT_BUILD
            Debug.Log($"[GameStateManager] {prev} → {next}  (timeScale={Time.timeScale})");
#endif
        }

        /// <summary>
        /// Pauses the game by transitioning to <see cref="GameState.Paused"/>.
        /// Sets <see cref="Time.timeScale"/> to <c>0</c>.
        /// </summary>
        public void Pause() => SetState(GameState.Paused);

        /// <summary>
        /// Resumes normal gameplay by transitioning to <see cref="GameState.Playing"/>.
        /// Restores <see cref="Time.timeScale"/> to <c>1</c>.
        /// </summary>
        public void Resume() => SetState(GameState.Playing);

        /// <summary>
        /// Logs <paramref name="reason"/> and transitions to <see cref="GameState.GameOver"/>.
        /// Restores <see cref="Time.timeScale"/> to <c>1</c> so end-screen animations run.
        /// </summary>
        /// <param name="reason">Human-readable description of why the game ended.</param>
        public void GameOver(string reason)
        {
            Debug.Log($"[GameStateManager] Game Over — {reason}");
            SetState(GameState.GameOver);
        }

        // ─────────────────────────────────────────────────────────────────────
        //  Private helpers
        // ─────────────────────────────────────────────────────────────────────

        /// <summary>
        /// Applies the canonical <see cref="Time.timeScale"/> for <paramref name="state"/>:
        /// <c>0</c> when <see cref="GameState.Paused"/>; <c>1</c> for all other states
        /// (including <see cref="GameState.GameOver"/> so end-screen UI animates normally).
        /// </summary>
        private static void ApplyTimeScale(GameState state)
        {
            Time.timeScale = state == GameState.Paused ? 0f : 1f;
        }

#if UNITY_EDITOR
        // ─────────────────────────────────────────────────────────────────────
        //  Editor helpers
        // ─────────────────────────────────────────────────────────────────────

        [ContextMenu("Debug — Set Playing")]
        private void EditorSetPlaying()   => SetState(GameState.Playing);

        [ContextMenu("Debug — Pause")]
        private void EditorPause()        => Pause();

        [ContextMenu("Debug — Resume")]
        private void EditorResume()       => Resume();

        [ContextMenu("Debug — Game Over")]
        private void EditorGameOver()     => GameOver("Triggered from Editor context menu");

        [ContextMenu("Debug — Victory")]
        private void EditorVictory()      => SetState(GameState.Victory);
#endif
    }
}
