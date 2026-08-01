using System;
using System.Collections;
using BusShift.Ghosts;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BusShift.Core
{
    public enum EndingType
    {
        Good,
        Neutral,
        Bad
    }

    /// <summary>
    /// Selects and plays the canonical ending after the full five day game.
    ///
    /// Ending thresholds use RemainingSanity, not CurrentTension. A ghost kill always
    /// forces the bad ending. Build 0.1.0 uses SliceEnding and does not invoke this flow.
    /// </summary>
    [DisallowMultipleComponent]
    public class EndingManager : MonoBehaviour
    {
        public static EndingManager Instance { get; private set; }

        [Header("Remaining sanity thresholds")]
        [Tooltip("Remaining sanity required for the good ending.")]
        [SerializeField] [Range(0f, 1f)] private float _goodEndingSanityThreshold = 0.60f;

        [Tooltip("Remaining sanity required to avoid the bad ending.")]
        [SerializeField] [Range(0f, 1f)] private float _badEndingSanityThreshold = 0.20f;

        [Header("Credits")]
        [SerializeField] private string _creditsSceneName = "Credits";

        [Tooltip("Fallback delay used only when CutsceneManager is unavailable.")]
        [SerializeField] [Range(1f, 30f)] private float _cutsceneFallbackDuration = 5f;

        private bool _killedByGhost;
        private bool _endingTriggered;
        private CutsceneId? _activeEndingCutscene;
        private Coroutine _fallbackRoutine;

        public static event Action<EndingType> OnEndingTriggered;

        /// <summary>
        /// Informational request event retained for UI, analytics, and legacy integrations.
        /// EndingManager now invokes CutsceneManager directly when it is available.
        /// </summary>
        public static event Action<CutsceneId> OnCutsceneRequested;

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
            DayManager.OnGameCompleted += HandleGameCompleted;
            GhostBase.OnGameOverTriggered += HandleGhostGameOver;
            CutsceneManager.OnCutsceneEnded += HandleCutsceneEnded;
        }

        private void OnDisable()
        {
            DayManager.OnGameCompleted -= HandleGameCompleted;
            GhostBase.OnGameOverTriggered -= HandleGhostGameOver;
            CutsceneManager.OnCutsceneEnded -= HandleCutsceneEnded;
        }

        private void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }

        public EndingType CalculateEnding()
        {
            if (_killedByGhost)
            {
                return EndingType.Bad;
            }

            float remainingSanity =
                GameManager.Instance?.SanitySystem?.RemainingSanity ?? 0f;

            if (remainingSanity >= _goodEndingSanityThreshold)
            {
                return EndingType.Good;
            }

            if (remainingSanity >= _badEndingSanityThreshold)
            {
                return EndingType.Neutral;
            }

            return EndingType.Bad;
        }

        public void TriggerEnding(EndingType type)
        {
            if (_endingTriggered)
            {
                Debug.LogWarning("[EndingManager] Ending already triggered. Duplicate request ignored.");
                return;
            }

            _endingTriggered = true;
            GameManager.Instance?.TriggerVictory();
            OnEndingTriggered?.Invoke(type);

            CutsceneId cutsceneId = GetCutsceneId(type);
            _activeEndingCutscene = cutsceneId;
            OnCutsceneRequested?.Invoke(cutsceneId);

            if (CutsceneManager.Instance != null)
            {
                CutsceneManager.Instance.PlayCutscene(cutsceneId);
                return;
            }

            Debug.LogWarning(
                "[EndingManager] CutsceneManager is unavailable. Using the credits fallback delay.");
            _fallbackRoutine = StartCoroutine(FallbackToCreditsRoutine());
        }

        public void ResetEndingState()
        {
            if (_fallbackRoutine != null)
            {
                StopCoroutine(_fallbackRoutine);
                _fallbackRoutine = null;
            }

            _killedByGhost = false;
            _endingTriggered = false;
            _activeEndingCutscene = null;
        }

        public void LoadCreditsScene()
        {
            if (!string.IsNullOrWhiteSpace(_creditsSceneName) &&
                Application.CanStreamedLevelBeLoaded(_creditsSceneName))
            {
                SceneManager.LoadSceneAsync(_creditsSceneName);
                return;
            }

            Debug.LogWarning(
                $"[EndingManager] Credits scene '{_creditsSceneName}' is unavailable. " +
                "Returning to MainMenu instead.");

            if (Application.CanStreamedLevelBeLoaded("MainMenu"))
            {
                SceneManager.LoadSceneAsync("MainMenu");
            }

            GameManager.Instance?.ReturnToMainMenu();
        }

        public string GetEndingTitle(EndingType type)
        {
            return type switch
            {
                EndingType.Good => "Última Parada",
                EndingType.Neutral => "Mudança de Rota",
                EndingType.Bad => "Sem Retorno",
                _ => string.Empty
            };
        }

        public string GetEndingDescription(EndingType type)
        {
            return type switch
            {
                EndingType.Good =>
                    "Dale encontrou a verdade e saiu do ônibus inteiro o suficiente para voltar para casa.",
                EndingType.Neutral =>
                    "Dale saiu de Ravenswood, mas Ravenswood não saiu dele.",
                EndingType.Bad =>
                    "A rota 104 continua. Dale agora faz parte do ciclo.",
                _ => string.Empty
            };
        }

        private void HandleGameCompleted()
        {
            TriggerEnding(CalculateEnding());
        }

        private void HandleGhostGameOver(GhostType ghostType)
        {
            _killedByGhost = true;
        }

        private void HandleCutsceneEnded(CutsceneId cutsceneId)
        {
            if (!_endingTriggered || !_activeEndingCutscene.HasValue)
            {
                return;
            }

            if (_activeEndingCutscene.Value != cutsceneId)
            {
                return;
            }

            _activeEndingCutscene = null;
            LoadCreditsScene();
        }

        private IEnumerator FallbackToCreditsRoutine()
        {
            yield return new WaitForSecondsRealtime(_cutsceneFallbackDuration);
            _fallbackRoutine = null;
            LoadCreditsScene();
        }

        private static CutsceneId GetCutsceneId(EndingType type)
        {
            return type switch
            {
                EndingType.Good => CutsceneId.Day5Ending_Good,
                EndingType.Neutral => CutsceneId.Day5Ending_Neutral,
                _ => CutsceneId.Day5Ending_Bad
            };
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_goodEndingSanityThreshold <= _badEndingSanityThreshold)
            {
                _goodEndingSanityThreshold = Mathf.Clamp01(
                    _badEndingSanityThreshold + 0.01f);
            }
        }
#endif
    }
}
