using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace BusShift.Core
{
    /// <summary>
    /// Manages scene transitions between periods with a black-screen fade effect.
    ///
    /// Scene name array layout (index = (day-1)*2 + period):
    ///   0 → Day1Morning  |  1 → Day1Night
    ///   2 → Day2Morning  |  3 → Day2Night
    ///   4 → Day3Morning  |  5 → Day3Night
    ///   6 → Day4Morning  |  7 → Day4Night
    ///   8 → Day5Morning  |  9 → Day5Night
    ///
    /// Setup:
    ///   1. Attach this component to a persistent GameObject (DontDestroyOnLoad).
    ///   2. Populate <see cref="SceneNames"/> in the Inspector with the exact scene names
    ///      registered in File → Build Settings.
    ///   3. Optionally assign an existing <see cref="CanvasGroup"/> to <see cref="_fadeCanvasGroup"/>;
    ///      if left null the component creates a full-screen black overlay automatically.
    /// </summary>
    public class SceneTransitionManager : MonoBehaviour
    {
        // ── Inspector ─────────────────────────────────────────────────────────────

        [Header("Scene Names (index = (day-1)*2 + period)")]
        [Tooltip("10 scene names: Day1Morning, Day1Night, Day2Morning, …, Day5Night.")]
        [SerializeField] private string[] _sceneNames = new string[]
        {
            "Day1Morning", "Day1Night",
            "Day2Morning", "Day2Night",
            "Day3Morning", "Day3Night",
            "Day4Morning", "Day4Night",
            "Day5Morning", "Day5Night"
        };

        [Header("Fade Settings")]
        [Tooltip("CanvasGroup on a full-screen black Image. Auto-created if not assigned.")]
        [SerializeField] private CanvasGroup _fadeCanvasGroup;

        [Tooltip("Duration (seconds) of each fade direction.")]
        [SerializeField] [Range(0.1f, 2f)] private float _fadeDuration = 0.5f;

        // ── Singleton ─────────────────────────────────────────────────────────────

        public static SceneTransitionManager Instance { get; private set; }

        // ── State ─────────────────────────────────────────────────────────────────

        private bool _isTransitioning;

        /// <summary>True while a fade/load sequence is in progress.</summary>
        public bool IsTransitioning => _isTransitioning;

        // ── Events ────────────────────────────────────────────────────────────────

        /// <summary>
        /// Fired when the fade-out to black is complete (screen is fully black).
        /// Use this to hide UI elements or pause gameplay before the new scene loads.
        /// </summary>
        public static event Action OnFadeOut;

        /// <summary>
        /// Fired when the fade-in from black is complete (new scene is fully visible).
        /// Use this to reveal UI or resume gameplay after the scene loads.
        /// </summary>
        public static event Action OnFadeIn;

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

            if (_fadeCanvasGroup == null)
                _fadeCanvasGroup = BuildFadeCanvas();

            // Start with a fully transparent overlay
            _fadeCanvasGroup.alpha = 0f;
            _fadeCanvasGroup.blocksRaycasts = false;
        }

        // ── Public API ───────────────────────────────────────────────────────────

        /// <summary>
        /// Fades to black, loads the scene for the given day/period, then fades back in.
        /// Does nothing if a transition is already in progress.
        /// </summary>
        /// <param name="day">Target day (1-5).</param>
        /// <param name="period">Target period (0 = Morning, 1 = Night).</param>
        public void LoadPeriodScene(int day, int period)
        {
            if (_isTransitioning)
            {
                Debug.LogWarning("[SceneTransitionManager] Transition already in progress — request ignored.");
                return;
            }

            StartCoroutine(TransitionRoutine(day, period));
        }

        // ── Core Coroutine ────────────────────────────────────────────────────────

        private IEnumerator TransitionRoutine(int day, int period)
        {
            _isTransitioning = true;

            // ── Phase 1: Fade out (reveal black overlay) ──────────────────────────
            yield return StartCoroutine(Fade(0f, 1f));
            OnFadeOut?.Invoke();

            // ── Phase 2: Load scene asynchronously ────────────────────────────────
            int index = SceneIndex(day, period);

            if (index < 0 || index >= _sceneNames.Length || string.IsNullOrEmpty(_sceneNames[index]))
            {
                Debug.LogError($"[SceneTransitionManager] No scene configured for " +
                               $"Day {day} {(period == 0 ? "Morning" : "Night")} (index {index}).");
                // Gracefully fade back in so the game is not stuck on a black screen.
                yield return StartCoroutine(Fade(1f, 0f));
                _isTransitioning = false;
                yield break;
            }

            AsyncOperation loadOp = SceneManager.LoadSceneAsync(_sceneNames[index]);

            if (loadOp != null)
            {
                // Prevent the scene from activating until the fade is ready
                loadOp.allowSceneActivation = false;

                while (loadOp.progress < 0.9f)
                    yield return null;

                loadOp.allowSceneActivation = true;

                while (!loadOp.isDone)
                    yield return null;
            }

            // ── Phase 3: Fade in (hide black overlay) ─────────────────────────────
            yield return StartCoroutine(Fade(1f, 0f));
            OnFadeIn?.Invoke();

            _isTransitioning = false;
        }

        // ── Private Helpers ───────────────────────────────────────────────────────

        /// <summary>Returns the scene array index for a given day/period.</summary>
        private static int SceneIndex(int day, int period) =>
            (Mathf.Clamp(day, 1, DayManager.TotalDays) - 1) * DayManager.PeriodsPerDay +
            Mathf.Clamp(period, 0, DayManager.PeriodsPerDay - 1);

        /// <summary>Interpolates the fade overlay alpha from <paramref name="from"/> to <paramref name="to"/>.</summary>
        private IEnumerator Fade(float from, float to)
        {
            if (_fadeCanvasGroup == null) yield break;

            _fadeCanvasGroup.blocksRaycasts = to > 0f;

            float elapsed = 0f;
            while (elapsed < _fadeDuration)
            {
                elapsed += Time.unscaledDeltaTime; // unscaled so fades work even when TimeScale = 0
                _fadeCanvasGroup.alpha = Mathf.Lerp(from, to, elapsed / _fadeDuration);
                yield return null;
            }

            _fadeCanvasGroup.alpha = to;
            _fadeCanvasGroup.blocksRaycasts = to > 0f;
        }

        /// <summary>
        /// Programmatically creates a full-screen black overlay with a CanvasGroup.
        /// Called automatically in Awake when <see cref="_fadeCanvasGroup"/> is not assigned.
        /// </summary>
        private CanvasGroup BuildFadeCanvas()
        {
            // ── Root canvas object ─────────────────────────────────────────────────
            var canvasGO = new GameObject("[FadeCanvas]");
            canvasGO.transform.SetParent(transform, false);

            var canvas = canvasGO.AddComponent<Canvas>();
            canvas.renderMode  = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 999; // always on top

            canvasGO.AddComponent<CanvasScaler>();
            canvasGO.AddComponent<GraphicRaycaster>();

            // ── Full-screen black image ────────────────────────────────────────────
            var imageGO = new GameObject("BlackFill");
            imageGO.transform.SetParent(canvasGO.transform, false);

            var rt = imageGO.AddComponent<RectTransform>();
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.offsetMin = Vector2.zero;
            rt.offsetMax = Vector2.zero;

            var img = imageGO.AddComponent<Image>();
            img.color = Color.black;
            img.raycastTarget = true;

            // ── CanvasGroup on the canvas root ────────────────────────────────────
            var cg = canvasGO.AddComponent<CanvasGroup>();
            cg.alpha = 0f;
            cg.blocksRaycasts = false;
            cg.interactable    = false;

            Debug.Log("[SceneTransitionManager] Fade canvas created automatically.");
            return cg;
        }
    }
}
