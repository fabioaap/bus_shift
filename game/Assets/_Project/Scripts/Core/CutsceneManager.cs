using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace BusShift.Core
{
    // ─────────────────────────────────────────────────────────────────────────

    /// <summary>
    /// All named cutscene sequences in Bus Shift.
    /// Pass a value to <see cref="CutsceneManager.PlayCutscene"/> to trigger
    /// the corresponding <see cref="CutsceneSequence"/> asset.
    /// </summary>
    public enum CutsceneId
    {
        /// <summary>Opening cinematic for Day 1 (morning dispatch briefing).</summary>
        Day1Intro,

        /// <summary>End-of-day cinematic for Day 1 night.</summary>
        Day1Night,

        /// <summary>Opening cinematic for Day 2.</summary>
        Day2Intro,

        /// <summary>Mid-game pivotal event on Day 3.</summary>
        Day3Event,

        /// <summary>Good ending sequence (Day 5).</summary>
        Day5Ending_Good,

        /// <summary>Neutral ending sequence (Day 5).</summary>
        Day5Ending_Neutral,

        /// <summary>Bad ending sequence (Day 5).</summary>
        Day5Ending_Bad,
    }

    // ─────────────────────────────────────────────────────────────────────────

    /// <summary>
    /// Singleton MonoBehaviour that manages cinematic cutscene playback for Bus Shift.
    ///
    /// <para>
    /// <b>Setup:</b>
    /// <list type="bullet">
    ///   <item>Attach to a persistent <c>DontDestroyOnLoad</c> GameObject.</item>
    ///   <item>Optionally assign <see cref="_fadeCanvasGroup"/> in the Inspector;
    ///         if left null a full-screen black overlay is created automatically.</item>
    ///   <item>Place <see cref="CutsceneSequence"/> assets in
    ///         <c>Resources/CutsceneSequences/</c> and name each asset after its
    ///         matching <see cref="CutsceneId"/> value
    ///         (e.g. <c>Day1Intro.asset</c>).</item>
    /// </list>
    /// </para>
    ///
    /// <para>
    /// <b>Usage:</b>
    /// <code>
    ///   CutsceneManager.Instance.PlayCutscene(CutsceneId.Day1Intro);
    ///
    ///   // Skip with Esc / Space (or programmatically)
    ///   CutsceneManager.Instance.SkipCutscene();
    /// </code>
    /// </para>
    ///
    /// <para>
    /// <b>State flow:</b>
    /// <c>GameState.Playing → Loading (cutscene) → Playing</c>
    /// (or whichever state was active before if needed; this implementation
    /// always restores <see cref="GameState.Playing"/>).
    /// </para>
    ///
    /// <para>
    /// Fade animations use <see cref="Time.unscaledDeltaTime"/> so they work
    /// regardless of <see cref="Time.timeScale"/>.
    /// The <see cref="_fadeCanvasGroup"/>'s <c>sortingOrder</c> is set to
    /// <c>998</c>, one below <see cref="SceneTransitionManager"/>'s <c>999</c>.
    /// </para>
    /// </summary>
    public class CutsceneManager : MonoBehaviour
    {
        // ── Singleton ─────────────────────────────────────────────────────────

        /// <summary>The single active instance in the scene.</summary>
        public static CutsceneManager Instance { get; private set; }

        // ── Events ────────────────────────────────────────────────────────────

        /// <summary>
        /// Fired immediately after <see cref="GameStateManager"/> is set to
        /// <see cref="GameState.Loading"/> and before the first frame of the cutscene.
        /// </summary>
        /// <param name="id">The <see cref="CutsceneId"/> that started.</param>
        public static event Action<CutsceneId> OnCutsceneStarted;

        /// <summary>
        /// Fired after the cutscene finishes (or is skipped) and before
        /// <see cref="GameStateManager"/> is restored to <see cref="GameState.Playing"/>.
        /// </summary>
        /// <param name="id">The <see cref="CutsceneId"/> that ended.</param>
        public static event Action<CutsceneId> OnCutsceneEnded;

        // ── Inspector ─────────────────────────────────────────────────────────

        [Header("Fade Canvas")]
        [Tooltip("CanvasGroup on a full-screen black image used for fade in/out. " +
                 "Auto-created if not assigned.")]
        [SerializeField] private CanvasGroup _fadeCanvasGroup;

        [Tooltip("Seconds per fade direction (fade-in or fade-out).")]
        [SerializeField] [Range(0.1f, 3f)] private float _fadeDuration = 0.5f;

        [Header("Skip Input")]
        [Tooltip("Allow Esc or Space to skip the currently playing cutscene.")]
        [SerializeField] private bool _allowSkip = true;

        [Header("Resources")]
        [Tooltip("Sub-folder inside Resources/ where CutsceneSequence assets live. " +
                 "Asset filenames must match CutsceneId values (e.g. Day1Intro.asset).")]
        [SerializeField] private string _resourcesFolder = "CutsceneSequences";

        // ── Private state ─────────────────────────────────────────────────────

        private Coroutine _activeCutscene;
        private bool      _skipRequested;

        /// <summary>Set to <c>true</c> by <see cref="HandleDialogueComplete"/> when
        /// <see cref="DialogueSystem.OnSequenceComplete"/> fires.</summary>
        private bool _dialogueComplete;

        // ── Unity Lifecycle ───────────────────────────────────────────────────

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

            // Ensure overlay is transparent and non-blocking at startup.
            _fadeCanvasGroup.alpha          = 0f;
            _fadeCanvasGroup.blocksRaycasts = false;
        }

        private void OnEnable()
        {
            // Listen for dialogue completion so cutscenes can end when dialogue finishes.
            DialogueSystem.OnSequenceComplete += HandleDialogueComplete;
        }

        private void OnDisable()
        {
            DialogueSystem.OnSequenceComplete -= HandleDialogueComplete;
        }

        private void Update()
        {
            if (!_allowSkip || _activeCutscene == null) return;

            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Space))
                SkipCutscene();
        }

        // ── Public API ────────────────────────────────────────────────────────

        /// <summary>
        /// Loads and plays the <see cref="CutsceneSequence"/> named after
        /// <paramref name="id"/> from
        /// <c>Resources/<see cref="_resourcesFolder"/>/{id}</c>.
        ///
        /// <para>
        /// If an asset is not found the cutscene still runs with default
        /// settings (fade in/out, 3-second hold) so development is not blocked.
        /// </para>
        /// <para>
        /// Does nothing if a cutscene is already playing.
        /// </para>
        /// </summary>
        /// <param name="id">Which cutscene to play.</param>
        public void PlayCutscene(CutsceneId id)
        {
            if (_activeCutscene != null)
            {
                Debug.LogWarning(
                    $"[CutsceneManager] Cannot start {id} — a cutscene is already playing.");
                return;
            }

            _activeCutscene = StartCoroutine(CutsceneRoutine(id));
        }

        /// <summary>
        /// Requests the active cutscene to skip at the next safe yield point.
        /// Cancels any in-progress <see cref="DialogueSystem"/> sequence as well.
        /// Does nothing if no cutscene is currently playing.
        /// </summary>
        public void SkipCutscene()
        {
            if (_activeCutscene == null) return;

            _skipRequested = true;

#if UNITY_EDITOR || DEVELOPMENT_BUILD
            Debug.Log("[CutsceneManager] Skip requested.");
#endif
        }

        // ── Core Coroutine ────────────────────────────────────────────────────

        private IEnumerator CutsceneRoutine(CutsceneId id)
        {
            _skipRequested    = false;
            _dialogueComplete = false;

            // ── Load SO ───────────────────────────────────────────────────────
            var seq = Resources.Load<CutsceneSequence>($"{_resourcesFolder}/{id}");

            if (seq == null)
            {
                Debug.LogWarning(
                    $"[CutsceneManager] No CutsceneSequence asset found for '{id}' in " +
                    $"Resources/{_resourcesFolder}/. Running with default settings.");
            }

            // ── Enter Loading state ───────────────────────────────────────────
            GameStateManager.Instance?.SetState(GameState.Loading);
            OnCutsceneStarted?.Invoke(id);

#if UNITY_EDITOR || DEVELOPMENT_BUILD
            Debug.Log($"[CutsceneManager] Started → {id}");
#endif

            // ── Fade in from black ────────────────────────────────────────────
            bool wantFadeIn = seq == null || seq.FadeIn;
            if (wantFadeIn)
            {
                // Ensure the overlay is fully opaque before revealing the scene.
                _fadeCanvasGroup.alpha          = 1f;
                _fadeCanvasGroup.blocksRaycasts = true;
                yield return StartCoroutine(Fade(1f, 0f));
            }

            // ── Dialogue or timed hold ────────────────────────────────────────
            bool hasDialogue = seq != null && !string.IsNullOrEmpty(seq.DialogueSequenceId);

            if (hasDialogue && !_skipRequested)
            {
                yield return StartCoroutine(PlayDialoguePhase(seq));
            }
            else if (!_skipRequested)
            {
                yield return StartCoroutine(TimedHoldPhase(seq != null ? seq.Duration : 3f));
            }

            // ── Cancel dialogue if the user skipped mid-playback ──────────────
            if (_skipRequested && DialogueSystem.Instance != null)
                DialogueSystem.Instance.CancelDialogue();

            // ── Fade out to black ─────────────────────────────────────────────
            bool wantFadeOut = seq == null || seq.FadeOut;
            if (wantFadeOut)
                yield return StartCoroutine(Fade(0f, 1f));

            // ── Restore state ─────────────────────────────────────────────────
            OnCutsceneEnded?.Invoke(id);
            GameStateManager.Instance?.SetState(GameState.Playing);

#if UNITY_EDITOR || DEVELOPMENT_BUILD
            Debug.Log($"[CutsceneManager] Ended → {id}");
#endif

            _activeCutscene = null;
        }

        // ── Phase helpers ─────────────────────────────────────────────────────

        /// <summary>
        /// Triggers <see cref="DialogueSystem.PlayRadioDialogue"/> and waits until
        /// dialogue finishes, the skip flag is set, or the cutscene duration times out.
        /// </summary>
        private IEnumerator PlayDialoguePhase(CutsceneSequence seq)
        {
            if (DialogueSystem.Instance == null)
            {
                Debug.LogWarning("[CutsceneManager] DialogueSystem.Instance is null — falling back to timed hold.");
                yield return StartCoroutine(TimedHoldPhase(seq.Duration));
                yield break;
            }

            _dialogueComplete = false;
            DialogueSystem.Instance.PlayRadioDialogue(seq.DialogueSequenceId);

            float timeout = seq.Duration > 0f ? seq.Duration : float.MaxValue;
            float elapsed = 0f;

            while (!_dialogueComplete && !_skipRequested && elapsed < timeout)
            {
                elapsed += Time.unscaledDeltaTime;
                yield return null;
            }
        }

        /// <summary>
        /// Waits for <paramref name="duration"/> seconds (unscaled) or until
        /// a skip is requested.
        /// </summary>
        private IEnumerator TimedHoldPhase(float duration)
        {
            float elapsed = 0f;
            while (!_skipRequested && elapsed < duration)
            {
                elapsed += Time.unscaledDeltaTime;
                yield return null;
            }
        }

        // ── Fade helpers ──────────────────────────────────────────────────────

        /// <summary>
        /// Animates <see cref="_fadeCanvasGroup"/> alpha from
        /// <paramref name="from"/> to <paramref name="to"/> using unscaled time.
        /// </summary>
        /// <param name="from">Starting alpha (0 = transparent, 1 = black).</param>
        /// <param name="to">Target alpha (0 = transparent, 1 = black).</param>
        private IEnumerator Fade(float from, float to)
        {
            if (_fadeCanvasGroup == null) yield break;

            _fadeCanvasGroup.blocksRaycasts = from > 0f || to > 0f;

            float elapsed = 0f;
            while (elapsed < _fadeDuration)
            {
                elapsed               += Time.unscaledDeltaTime;
                _fadeCanvasGroup.alpha  = Mathf.Lerp(from, to, elapsed / _fadeDuration);
                yield return null;
            }

            _fadeCanvasGroup.alpha          = to;
            _fadeCanvasGroup.blocksRaycasts = to > 0f;
        }

        // ── Event handler ─────────────────────────────────────────────────────

        /// <summary>
        /// Subscribed to <see cref="DialogueSystem.OnSequenceComplete"/> in
        /// <see cref="OnEnable"/>. Sets <see cref="_dialogueComplete"/> so the
        /// cutscene coroutine can advance past the dialogue phase.
        /// </summary>
        private void HandleDialogueComplete() => _dialogueComplete = true;

        // ── Fade canvas factory ───────────────────────────────────────────────

        /// <summary>
        /// Programmatically creates a full-screen black overlay <see cref="CanvasGroup"/>
        /// when <see cref="_fadeCanvasGroup"/> is not assigned in the Inspector.
        /// Uses <c>sortingOrder = 998</c> so it sits just below
        /// <see cref="SceneTransitionManager"/>'s overlay (999).
        /// </summary>
        /// <returns>The newly created <see cref="CanvasGroup"/>.</returns>
        private CanvasGroup BuildFadeCanvas()
        {
            // ── Canvas root ───────────────────────────────────────────────────
            var canvasGO = new GameObject("[CutsceneFadeCanvas]");
            canvasGO.transform.SetParent(transform, false);

            var canvas = canvasGO.AddComponent<Canvas>();
            canvas.renderMode   = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 998; // Below SceneTransitionManager's 999

            canvasGO.AddComponent<CanvasScaler>();
            canvasGO.AddComponent<GraphicRaycaster>();

            // ── Full-screen black image ───────────────────────────────────────
            var imageGO = new GameObject("BlackFill");
            imageGO.transform.SetParent(canvasGO.transform, false);

            var rt = imageGO.AddComponent<RectTransform>();
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.offsetMin = Vector2.zero;
            rt.offsetMax = Vector2.zero;

            var img = imageGO.AddComponent<Image>();
            img.color         = Color.black;
            img.raycastTarget = true;

            // ── CanvasGroup on the canvas root ────────────────────────────────
            var cg = canvasGO.AddComponent<CanvasGroup>();
            cg.alpha          = 0f;
            cg.blocksRaycasts = false;
            cg.interactable   = false;

            Debug.Log("[CutsceneManager] Fade canvas created automatically.");
            return cg;
        }

#if UNITY_EDITOR
        // ── Editor context menus ──────────────────────────────────────────────

        [ContextMenu("Debug — Play Day1Intro")]
        private void EditorPlayDay1Intro() => PlayCutscene(CutsceneId.Day1Intro);

        [ContextMenu("Debug — Play Day1Night")]
        private void EditorPlayDay1Night() => PlayCutscene(CutsceneId.Day1Night);

        [ContextMenu("Debug — Play Day5Ending_Good")]
        private void EditorPlayEndingGood() => PlayCutscene(CutsceneId.Day5Ending_Good);

        [ContextMenu("Debug — Skip Current")]
        private void EditorSkip() => SkipCutscene();
#endif
    }
}
