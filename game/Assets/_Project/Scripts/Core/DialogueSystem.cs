using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace BusShift.Core
{
    /// <summary>
    /// Singleton MonoBehaviour that drives data-driven radio dialogue sequences
    /// between Earl Hayes (dispatcher) and Ray Morgan (driver).
    ///
    /// <para>
    /// <b>Setup:</b>
    /// <list type="bullet">
    ///   <item>Attach to a persistent <c>DontDestroyOnLoad</c> GameObject.</item>
    ///   <item>Assign <see cref="_subtitleText"/> (TMP_Text) and
    ///         <see cref="_subtitlePanel"/> (CanvasGroup) in the Inspector, or let
    ///         <c>Awake</c> find them automatically via
    ///         <see cref="Object.FindAnyObjectByType{T}"/>.</item>
    ///   <item>Place <see cref="DialogueSequence"/> assets in
    ///         <c>Resources/DialogueSequences/</c>.</item>
    /// </list>
    /// </para>
    ///
    /// <para>
    /// <b>Usage:</b>
    /// <code>
    ///   // Start a radio dialogue by sequence ID
    ///   DialogueSystem.Instance.PlayRadioDialogue("Day1IntroSequence");
    ///
    ///   // Stop at any time
    ///   DialogueSystem.Instance.CancelDialogue();
    /// </code>
    /// </para>
    ///
    /// <para>
    /// Subtitles are shown on a <see cref="CanvasGroup"/> that fades in and out
    /// using unscaled time, so playback is unaffected by <see cref="Time.timeScale"/>.
    /// </para>
    /// </summary>
    public class DialogueSystem : MonoBehaviour
    {
        // ── Singleton ─────────────────────────────────────────────────────────

        /// <summary>The single active instance in the scene.</summary>
        public static DialogueSystem Instance { get; private set; }

        // ── Events ────────────────────────────────────────────────────────────

        /// <summary>
        /// Fired immediately when a new <see cref="DialogueLine"/> begins
        /// (before fade-in).  Useful for driving reaction animations or logs.
        /// </summary>
        public static event Action<DialogueLine> OnLineStarted;

        /// <summary>
        /// Fired when the active sequence ends naturally (not on
        /// <see cref="CancelDialogue"/>).
        /// </summary>
        public static event Action OnSequenceComplete;

        // ── Inspector ─────────────────────────────────────────────────────────

        [Header("Subtitle UI")]
        [Tooltip("TMP_Text element that shows the dialogue subtitle. " +
                 "Auto-found via FindAnyObjectByType if not assigned.")]
        [SerializeField] private TMP_Text _subtitleText;

        [Tooltip("CanvasGroup wrapping the subtitle panel, used for fade in/out. " +
                 "Auto-found via FindAnyObjectByType if not assigned.")]
        [SerializeField] private CanvasGroup _subtitlePanel;

        [Header("Fade")]
        [Tooltip("Duration in seconds for each subtitle fade in or fade out.")]
        [SerializeField] [Range(0.05f, 1f)] private float _fadeDuration = 0.25f;

        [Header("Resources")]
        [Tooltip("Sub-folder inside Resources/ where DialogueSequence assets live.")]
        [SerializeField] private string _resourcesFolder = "DialogueSequences";

        // ── Private state ─────────────────────────────────────────────────────

        private Coroutine _activeSequence;
        private bool      _isCancelled;

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

            // Auto-find UI references when not wired in the Inspector.
            // Designers must ensure only one TMP_Text / CanvasGroup is auto-findable,
            // or assign explicitly to avoid ambiguity.
            if (_subtitleText  == null) _subtitleText  = FindAnyObjectByType<TMP_Text>();
            if (_subtitlePanel == null) _subtitlePanel = FindAnyObjectByType<CanvasGroup>();

            HidePanelImmediate();
        }

        // ── Public API ────────────────────────────────────────────────────────

        /// <summary>
        /// Loads a <see cref="DialogueSequence"/> ScriptableObject from
        /// <c>Resources/<see cref="_resourcesFolder"/>/<paramref name="sequenceId"/></c>
        /// and starts playback.
        ///
        /// <para>
        /// If a sequence is already running it is cancelled immediately before
        /// the new one begins.
        /// </para>
        /// </summary>
        /// <param name="sequenceId">
        /// Asset name matching <see cref="DialogueSequence.SequenceId"/> and the
        /// filename inside <c>Resources/DialogueSequences/</c>.
        /// Example: <c>"Day1IntroSequence"</c>.
        /// </param>
        public void PlayRadioDialogue(string sequenceId)
        {
            var seq = Resources.Load<DialogueSequence>($"{_resourcesFolder}/{sequenceId}");

            if (seq == null)
            {
                Debug.LogWarning(
                    $"[DialogueSystem] Sequence '{sequenceId}' not found in " +
                    $"Resources/{_resourcesFolder}/. Ensure the asset is placed there.");
                return;
            }

            if (_activeSequence != null)
                StopCoroutine(_activeSequence);

            _activeSequence = StartCoroutine(PlaySequence(seq));
        }

        /// <summary>
        /// Stops the active sequence immediately and fades out the subtitle panel.
        /// Does <b>not</b> fire <see cref="OnSequenceComplete"/>.
        /// </summary>
        public void CancelDialogue()
        {
            if (_activeSequence == null) return;

            _isCancelled = true;
            StopCoroutine(_activeSequence);
            _activeSequence = null;

            // Graceful fade out so the panel does not snap off.
            StartCoroutine(FadePanel(0f));

#if UNITY_EDITOR || DEVELOPMENT_BUILD
            Debug.Log("[DialogueSystem] Active dialogue cancelled.");
#endif
        }

        /// <summary>
        /// Plays all <see cref="DialogueLine"/>s in <paramref name="seq"/> sequentially.
        /// Can be awaited directly from another coroutine (e.g. <see cref="CutsceneManager"/>).
        /// </summary>
        /// <param name="seq">Sequence to play.  Returns immediately if null or empty.</param>
        /// <returns>Coroutine handle.</returns>
        public IEnumerator PlaySequence(DialogueSequence seq)
        {
            if (seq == null || seq.Lines == null || seq.Lines.Length == 0)
                yield break;

            _isCancelled = false;

            foreach (DialogueLine line in seq.Lines)
            {
                if (_isCancelled) break;
                yield return StartCoroutine(PlayLine(line));
            }

            // Always fade out when the sequence ends (natural or early break).
            yield return StartCoroutine(FadePanel(0f));

            _activeSequence = null;

            // Only fire the event for a natural (non-cancelled) completion.
            if (!_isCancelled)
                OnSequenceComplete?.Invoke();
        }

        /// <summary>
        /// Plays a single <see cref="DialogueLine"/>:
        /// <list type="number">
        ///   <item>Fires <see cref="OnLineStarted"/>.</item>
        ///   <item>Plays <see cref="DialogueLine.VoiceClip"/> if present.</item>
        ///   <item>Fades in the subtitle panel.</item>
        ///   <item>Holds for <see cref="DialogueLine.Duration"/> seconds (unscaled).</item>
        ///   <item>Fades out the subtitle panel.</item>
        /// </list>
        /// </summary>
        /// <param name="line">The line to present.</param>
        public IEnumerator PlayLine(DialogueLine line)
        {
            // Update subtitle text first (before fade-in so the text is ready).
            if (_subtitleText != null)
                _subtitleText.text = BuildSubtitleText(in line);

            // Fire event before audio/fade so listeners react at the correct moment.
            OnLineStarted?.Invoke(line);

            // Play optional voice-over clip through the shared audio pool.
            if (line.VoiceClip != null && AudioManager.Instance != null)
                AudioManager.Instance.PlaySFX(line.VoiceClip);

            // ── Fade in ───────────────────────────────────────────────────────
            yield return StartCoroutine(FadePanel(1f));

            // ── Hold ──────────────────────────────────────────────────────────
            float elapsed = 0f;
            while (elapsed < line.Duration && !_isCancelled)
            {
                elapsed += Time.unscaledDeltaTime;
                yield return null;
            }

            // ── Fade out ──────────────────────────────────────────────────────
            yield return StartCoroutine(FadePanel(0f));
        }

        // ── Private helpers ───────────────────────────────────────────────────

        /// <summary>
        /// Builds the subtitle string.  Wraps <see cref="DialogueLine.SpeakerId"/>
        /// in bold TMP rich-text when present.
        /// </summary>
        private static string BuildSubtitleText(in DialogueLine line)
        {
            return string.IsNullOrEmpty(line.SpeakerId)
                ? line.Text
                : $"<b>{line.SpeakerId}:</b> {line.Text}";
        }

        /// <summary>
        /// Animates <see cref="_subtitlePanel"/> alpha toward <paramref name="target"/>
        /// using <see cref="Time.unscaledDeltaTime"/> so fades work even when
        /// <see cref="Time.timeScale"/> is 0 (e.g. <see cref="GameState.Paused"/>).
        /// </summary>
        /// <param name="target">Destination alpha value (0 = hidden, 1 = visible).</param>
        private IEnumerator FadePanel(float target)
        {
            if (_subtitlePanel == null) yield break;

            float start   = _subtitlePanel.alpha;
            float elapsed = 0f;

            _subtitlePanel.blocksRaycasts = target > 0f;

            while (elapsed < _fadeDuration)
            {
                elapsed              += Time.unscaledDeltaTime;
                _subtitlePanel.alpha  = Mathf.Lerp(start, target, elapsed / _fadeDuration);
                yield return null;
            }

            _subtitlePanel.alpha          = target;
            _subtitlePanel.blocksRaycasts = target > 0f;
        }

        /// <summary>
        /// Instantly hides the subtitle panel without animation.
        /// Called during <c>Awake</c> to ensure a clean initial state.
        /// </summary>
        private void HidePanelImmediate()
        {
            if (_subtitlePanel == null) return;
            _subtitlePanel.alpha          = 0f;
            _subtitlePanel.blocksRaycasts = false;
        }

#if UNITY_EDITOR
        // ── Editor context menus ──────────────────────────────────────────────

        [ContextMenu("Debug — Cancel Active Dialogue")]
        private void EditorCancel() => CancelDialogue();

        [ContextMenu("Debug — Test Line (no audio)")]
        private void EditorTestLine()
        {
            var testLine = new DialogueLine
            {
                SpeakerId = "Earl Hayes",
                Text      = "Morgan, you still out there?",
                Duration  = 3f,
                VoiceClip = null
            };
            StartCoroutine(PlayLine(testLine));
        }
#endif
    }
}
