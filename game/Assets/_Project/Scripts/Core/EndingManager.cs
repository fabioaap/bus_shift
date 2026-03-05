using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using BusShift.Ghosts;

namespace BusShift.Core
{
    // ─────────────────────────────────────────────────────────────────────────────
    //  Enums
    // ─────────────────────────────────────────────────────────────────────────────

    /// <summary>
    /// The three possible endings for Bus Shift, determined by Dale's final sanity
    /// and whether he was killed by a ghost during Day 5.
    /// </summary>
    public enum EndingType { Good, Neutral, Bad }

    // ─────────────────────────────────────────────────────────────────────────────
    //  EndingManager
    // ─────────────────────────────────────────────────────────────────────────────

    /// <summary>
    /// Singleton that governs Bus Shift's three narrative endings.
    ///
    /// <para>
    /// <b>Ending selection logic (sanity is normalised 0–1):</b>
    /// <list type="bullet">
    ///   <item><b>Good</b>   — <see cref="_goodEndingSanityThreshold"/> (default 0.60) or above.</item>
    ///   <item><b>Neutral</b> — between <see cref="_badEndingSanityThreshold"/> and the good threshold.</item>
    ///   <item><b>Bad</b>   — below <see cref="_badEndingSanityThreshold"/> (default 0.20),
    ///                        <i>or</i> Dale was killed by a ghost.</item>
    /// </list>
    /// </para>
    ///
    /// <para>
    /// <b>Flow on <see cref="DayManager.OnGameCompleted"/>:</b>
    /// <list type="number">
    ///   <item>Calculate ending via <see cref="CalculateEnding"/>.</item>
    ///   <item>Fire <see cref="OnCutsceneRequested"/> (CutsceneManager hooks in here when ready).</item>
    ///   <item>Wait <see cref="_cutsceneFallbackDuration"/> then load the Credits scene.</item>
    /// </list>
    /// </para>
    ///
    /// Setup: attach to a persistent GameObject alongside <see cref="GameManager"/>.
    /// </summary>
    public class EndingManager : MonoBehaviour
    {
        // ── Singleton ─────────────────────────────────────────────────────────────

        /// <summary>Singleton instance. Persists across scenes.</summary>
        public static EndingManager Instance { get; private set; }

        // ── Inspector ─────────────────────────────────────────────────────────────

        [Header("Ending Sanity Thresholds  (normalised 0 – 1)")]
        [Tooltip("Sanity must exceed this value for the Good ending. Default 0.60 = 60 %.")]
        [SerializeField] [Range(0f, 1f)] private float _goodEndingSanityThreshold = 0.60f;

        [Tooltip("Sanity must be at or above this value for the Neutral ending. " +
                 "Below this threshold → Bad ending. Default 0.20 = 20 %.")]
        [SerializeField] [Range(0f, 1f)] private float _badEndingSanityThreshold = 0.20f;

        [Header("Credits Scene")]
        [Tooltip("Exact scene name registered in File → Build Settings.")]
        [SerializeField] private string _creditsSceneName = "Credits";

        [Tooltip("Seconds to wait before loading the Credits scene when no CutsceneManager " +
                 "is present. Replace with cutscene-driven logic once CutsceneManager lands.")]
        [SerializeField] [Range(1f, 30f)] private float _cutsceneFallbackDuration = 5f;

        // ── State ─────────────────────────────────────────────────────────────────

        private bool _killedByGhost;
        private bool _endingTriggered;

        // ── Events ────────────────────────────────────────────────────────────────

        /// <summary>
        /// Fired once when the ending is determined and activated.
        /// Subscribe to play cutscenes, show UI, or stop gameplay systems.
        /// </summary>
        public static event Action<EndingType> OnEndingTriggered;

        /// <summary>
        /// Forward-compatible hook for CutsceneManager (not yet implemented).
        /// CutsceneManager should subscribe here and call back when the cutscene ends.
        /// <para>
        /// When CutsceneManager is implemented:
        /// <c>EndingManager.OnCutsceneRequested += id => PlayCutscene(id);</c>
        /// </para>
        /// </summary>
        public static event Action<CutsceneId> OnCutsceneRequested;

        // ── Unity Lifecycle ──────────────────────────────────────────────────────

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
            DayManager.OnGameCompleted         += HandleGameCompleted;
            GhostBase.OnGameOverTriggered      += HandleGhostGameOver;
        }

        private void OnDisable()
        {
            DayManager.OnGameCompleted         -= HandleGameCompleted;
            GhostBase.OnGameOverTriggered      -= HandleGhostGameOver;
        }

        // ── Public API ───────────────────────────────────────────────────────────

        /// <summary>
        /// Determines the appropriate <see cref="EndingType"/> based on Dale's final
        /// sanity and whether the ghost killed him.
        /// </summary>
        /// <returns>The calculated <see cref="EndingType"/>.</returns>
        public EndingType CalculateEnding()
        {
            // A ghost kill always forces the worst outcome.
            if (_killedByGhost)
                return EndingType.Bad;

            float sanity = GameManager.Instance?.SanitySystem?.CurrentSanity ?? 0f;

            if (sanity > _goodEndingSanityThreshold)
                return EndingType.Good;

            if (sanity >= _badEndingSanityThreshold)
                return EndingType.Neutral;

            return EndingType.Bad;
        }

        /// <summary>
        /// Activates the supplied ending: fires events, requests the matching cutscene,
        /// and schedules loading the Credits scene.
        /// </summary>
        /// <param name="type">Ending to play.</param>
        public void TriggerEnding(EndingType type)
        {
            if (_endingTriggered)
            {
                Debug.LogWarning("[EndingManager] TriggerEnding called more than once — ignoring.");
                return;
            }

            _endingTriggered = true;

            Debug.Log($"[EndingManager] Triggering ending: {type}");

            // 1. Notify all listeners (UI, audio, etc.)
            OnEndingTriggered?.Invoke(type);

            // 2. Request the matching cutscene.
            //    CutsceneManager listens here; when it finishes it should call LoadCreditsScene().
            CutsceneId cutsceneId = type switch
            {
                EndingType.Good    => CutsceneId.Day5Ending_Good,
                EndingType.Neutral => CutsceneId.Day5Ending_Neutral,
                _                  => CutsceneId.Day5Ending_Bad
            };

            OnCutsceneRequested?.Invoke(cutsceneId);

            // 3. Fallback: if no CutsceneManager is listening, transition after a delay.
            //    Once CutsceneManager is implemented, it should call LoadCreditsScene()
            //    directly at cutscene end and this coroutine can be removed.
            StartCoroutine(FallbackToCreditsRoutine());
        }

        /// <summary>
        /// Loads the Credits scene directly, bypassing the cutscene.
        /// Call from CutsceneManager when the ending cutscene is complete.
        /// </summary>
        public void LoadCreditsScene()
        {
            if (string.IsNullOrEmpty(_creditsSceneName))
            {
                Debug.LogError("[EndingManager] Credits scene name is not configured.");
                return;
            }

            SceneManager.LoadSceneAsync(_creditsSceneName);
        }

        /// <summary>Returns the display title for the given ending.</summary>
        /// <param name="type">The ending to query.</param>
        /// <returns>Localised title string.</returns>
        public string GetEndingTitle(EndingType type) => type switch
        {
            EndingType.Good    => "Última Parada",
            EndingType.Neutral => "Mudança de Rota",
            EndingType.Bad     => "Sem Retorno",
            _                  => string.Empty
        };

        /// <summary>Returns the narrative summary text for the given ending.</summary>
        /// <param name="type">The ending to query.</param>
        /// <returns>Multi-line description string.</returns>
        public string GetEndingDescription(EndingType type) => type switch
        {
            EndingType.Good =>
                "Dale encontrou a verdade enterrada sob décadas de silêncio. " +
                "Os cinco nomes gravados em sua memória finalmente têm descanso. " +
                "Ele sai do ônibus pela última vez — e desta vez, sai sozinho.",

            EndingType.Neutral =>
                "Dale sobreviveu à semana. O ônibus está de volta no depósito. " +
                "Mas algumas rotas deixam marcas que o GPS não registra. " +
                "Ele vai dirigir de novo amanhã. Só não sabe para onde.",

            EndingType.Bad =>
                "A rota 104 continua rodando. O motorista não mudou. " +
                "Dale não perdeu a sanidade — ele simplesmente parou de procurar " +
                "a linha entre o que é real e o que é o ônibus. " +
                "Nunca mais saiu.",

            _ => string.Empty
        };

        // ── Private ───────────────────────────────────────────────────────────────

        private void HandleGameCompleted()
        {
            EndingType ending = CalculateEnding();
            TriggerEnding(ending);
        }

        private void HandleGhostGameOver(GhostType _ghostType)
        {
            // Any ghost kill flags the forced-bad-ending for when Day 5 completes.
            // If a ghost kills Dale mid-run, the game also ends immediately via GameManager.
            _killedByGhost = true;
        }

        /// <summary>
        /// Coroutine that transitions to the Credits scene after a fallback delay.
        /// This runs only when no CutsceneManager subscribes to <see cref="OnCutsceneRequested"/>.
        /// </summary>
        private IEnumerator FallbackToCreditsRoutine()
        {
            yield return new WaitForSeconds(_cutsceneFallbackDuration);

            // If LoadCreditsScene() was already called by CutsceneManager, the scene
            // will be loading/loaded; this is a safety net, not the primary path.
            if (!string.IsNullOrEmpty(_creditsSceneName))
                SceneManager.LoadSceneAsync(_creditsSceneName);
        }

        // ── Editor Validation ─────────────────────────────────────────────────────

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_goodEndingSanityThreshold <= _badEndingSanityThreshold)
            {
                Debug.LogWarning("[EndingManager] Good threshold must be above Bad threshold. " +
                                 "Adjusting Good threshold.");
                _goodEndingSanityThreshold = _badEndingSanityThreshold + 0.01f;
            }
        }
#endif
    }
}
