using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace BusShift.Core
{
    // ─────────────────────────────────────────────────────────────────────────────
    //  GhostId enum
    // ─────────────────────────────────────────────────────────────────────────────

    /// <summary>
    /// Ghost identifier used by <see cref="JumpscareSystem"/> within the
    /// <c>BusShift.Core</c> namespace, decoupling Core from <c>BusShift.Ghosts</c>.
    /// Maps 1-to-1 to <see cref="BusShift.Ghosts.GhostType"/> by integer value.
    /// </summary>
    public enum GhostId
    {
        Marcus = 0,
        Emma   = 1,
        Thomas = 2,
        Grace  = 3,
        Oliver = 4
    }

    // ─────────────────────────────────────────────────────────────────────────────
    //  JumpscareSystem
    // ─────────────────────────────────────────────────────────────────────────────

    /// <summary>
    /// Singleton that coordinates all jumpscare effects: full-screen image overlay,
    /// audio amplification, and sanity damage.
    ///
    /// <para>
    /// <b>Effect pipeline on <see cref="TriggerJumpscare"/>:</b>
    /// <list type="number">
    ///   <item>
    ///     Requests audio amplification via
    ///     <see cref="TensionAudioManager.TriggerJumpscare"/> (duration proportional to
    ///     <see cref="_jumpscareImageDuration"/> × <c>intensity</c>).
    ///   </item>
    ///   <item>
    ///     Shows the ghost-specific <see cref="_jumpscareImages"/> sprite on a
    ///     full-screen <see cref="CanvasGroup"/> overlay for
    ///     <see cref="_jumpscareImageDuration"/> seconds.
    ///   </item>
    ///   <item>
    ///     Applies <see cref="_sanityDamage"/> × <c>intensity</c> tension to
    ///     <see cref="SanitySystem"/> via
    ///     <c>GameManager.Instance.SanitySystem.AddTension()</c>.
    ///   </item>
    ///   <item>Fires <see cref="OnJumpscareTriggered"/> with the ghost's id.</item>
    /// </list>
    /// </para>
    ///
    /// <para>
    /// <b>EmmaGhost integration:</b>
    /// When <c>EmmaGhost.HasFullyManifested == true</c> and the player has <i>not</i>
    /// observed her, <c>EmmaGhost</c> calls <c>Despawn()</c>.  Wire
    /// <c>GhostBase.OnAttackDefeated</c> or use <c>JumpscareSystem.Instance.TriggerJumpscare</c>
    /// from within <c>EmmaGhost.OnObservationComplete</c> before calling
    /// <c>Despawn()</c> when <c>HasFullyManifested</c> is <c>true</c>.
    /// </para>
    ///
    /// <para>
    /// <b>Setup:</b>
    /// <list type="bullet">
    ///   <item>Attach to a persistent GameObject (DontDestroyOnLoad).</item>
    ///   <item>
    ///     Assign <see cref="_jumpscareOverlay"/> (a <see cref="CanvasGroup"/> on a
    ///     Screen Space – Overlay canvas with a child <see cref="Image"/>).
    ///     If left null the overlay is created automatically.
    ///   </item>
    ///   <item>
    ///     Assign one <see cref="Sprite"/> per ghost in <see cref="_jumpscareImages"/>
    ///     (index matches <see cref="GhostId"/> integer value: 0 = Marcus … 4 = Oliver).
    ///   </item>
    /// </list>
    /// </para>
    /// </summary>
    public class JumpscareSystem : MonoBehaviour
    {
        // ── Singleton ─────────────────────────────────────────────────────────────

        /// <summary>Singleton instance. Persists across scenes.</summary>
        public static JumpscareSystem Instance { get; private set; }

        // ── Inspector ─────────────────────────────────────────────────────────────

        [Header("Overlay UI")]
        [Tooltip("CanvasGroup on a full-screen overlay canvas. Auto-created if not assigned.")]
        [SerializeField] private CanvasGroup _jumpscareOverlay;

        [Tooltip("Image component inside the overlay used to display the ghost sprite. " +
                 "Auto-resolved from the overlay's children if not assigned.")]
        [SerializeField] private Image _jumpscareImage;

        [Header("Ghost Sprites  (index = GhostId)")]
        [Tooltip("Five sprites indexed by GhostId: 0=Marcus, 1=Emma, 2=Thomas, 3=Grace, 4=Oliver.")]
        [SerializeField] private Sprite[] _jumpscareImages = new Sprite[5];

        [Header("Settings")]
        [Tooltip("How long (seconds) the full-screen overlay stays visible.")]
        [SerializeField] [Range(0.1f, 3f)] private float _jumpscareImageDuration = 0.8f;

        [Tooltip("Normalised sanity damage applied per jumpscare (0–1 range; 0.20 = 20 pts). " +
                 "Scaled by intensity parameter.")]
        [SerializeField] [Range(0f, 1f)] private float _sanityDamage = 0.20f;

        // ── State ─────────────────────────────────────────────────────────────────

        private TensionAudioManager _audioManager;

        /// <summary>True while a jumpscare effect is currently playing.</summary>
        public bool IsJumpscareActive { get; private set; }

        // ── Events ────────────────────────────────────────────────────────────────

        /// <summary>
        /// Fired immediately when a jumpscare is triggered, before the coroutine runs.
        /// Subscribe to sync additional effects (VFX, controller rumble, etc.).
        /// </summary>
        public static event Action<GhostId> OnJumpscareTriggered;

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

            _audioManager = FindAnyObjectByType<TensionAudioManager>();

            if (_jumpscareOverlay == null)
                _jumpscareOverlay = BuildOverlayCanvas(out _jumpscareImage);
            else if (_jumpscareImage == null)
                _jumpscareImage = _jumpscareOverlay.GetComponentInChildren<Image>();

            // Start invisible
            SetOverlayVisible(false);
        }

        // ── Public API ───────────────────────────────────────────────────────────

        /// <summary>
        /// Plays the full jumpscare effect for the given ghost.
        /// Safe to call from any MonoBehaviour; re-entrant calls interrupt the previous effect.
        /// </summary>
        /// <param name="ghostId">Which ghost is causing the scare.</param>
        /// <param name="intensity">
        ///   0-to-1 multiplier for audio duration and sanity damage.
        ///   Default 1.0 for a full-strength jumpscare; pass lower values for soft scares.
        /// </param>
        public void TriggerJumpscare(GhostId ghostId, float intensity = 1.0f)
        {
            intensity = Mathf.Clamp01(intensity);

            // Interrupt any active jumpscare before starting a new one.
            if (IsJumpscareActive)
                StopAllCoroutines();

            StartCoroutine(JumpscareRoutine(ghostId, intensity));
        }

        // ── Private Helpers ───────────────────────────────────────────────────────

        private IEnumerator JumpscareRoutine(GhostId ghostId, float intensity)
        {
            IsJumpscareActive = true;

            // ── 1. Audio amplification ────────────────────────────────────────────
            if (_audioManager == null)
                _audioManager = FindAnyObjectByType<TensionAudioManager>();

            float audioDuration = _jumpscareImageDuration * intensity;
            _audioManager?.TriggerJumpscare(audioDuration);

            // ── 2. Full-screen overlay ────────────────────────────────────────────
            AssignGhostSprite(ghostId);
            SetOverlayVisible(true);

            // ── 3. Sanity damage ──────────────────────────────────────────────────
            float damage = _sanityDamage * intensity;
            SanitySystem sanity = GameManager.Instance?.SanitySystem;
            if (sanity == null)
                sanity = FindAnyObjectByType<SanitySystem>();
            sanity?.AddTension(damage);

            // ── 4. Notify listeners ───────────────────────────────────────────────
            OnJumpscareTriggered?.Invoke(ghostId);

#if UNITY_EDITOR || DEVELOPMENT_BUILD
            Debug.Log($"[JumpscareSystem] {ghostId} jumpscare — intensity {intensity:F2}, " +
                      $"damage {damage:F2}, duration {_jumpscareImageDuration}s.");
#endif

            // ── 5. Wait, then hide ────────────────────────────────────────────────
            yield return new WaitForSeconds(_jumpscareImageDuration);

            SetOverlayVisible(false);
            IsJumpscareActive = false;
        }

        /// <summary>Sets the sprite matching the ghost; falls back to null if not assigned.</summary>
        private void AssignGhostSprite(GhostId ghostId)
        {
            if (_jumpscareImage == null) return;

            int index = (int)ghostId;
            _jumpscareImage.sprite = (index >= 0 && index < _jumpscareImages.Length)
                ? _jumpscareImages[index]
                : null;
        }

        private void SetOverlayVisible(bool visible)
        {
            if (_jumpscareOverlay == null) return;

            _jumpscareOverlay.alpha          = visible ? 1f : 0f;
            _jumpscareOverlay.blocksRaycasts = visible;
            _jumpscareOverlay.interactable   = false;
        }

        /// <summary>
        /// Programmatically builds a Screen Space – Overlay canvas with a full-screen
        /// black Image and a CanvasGroup. Called in Awake when no overlay is assigned.
        /// </summary>
        private CanvasGroup BuildOverlayCanvas(out Image imageComponent)
        {
            // Root canvas
            var canvasGO = new GameObject("[JumpscareOverlay]");
            canvasGO.transform.SetParent(transform, false);
            DontDestroyOnLoad(canvasGO);

            var canvas = canvasGO.AddComponent<Canvas>();
            canvas.renderMode   = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 1000; // above everything, including fade canvas

            canvasGO.AddComponent<CanvasScaler>();

            // Full-screen image child
            var imageGO = new GameObject("JumpscareImage");
            imageGO.transform.SetParent(canvasGO.transform, false);

            var rt = imageGO.AddComponent<RectTransform>();
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.offsetMin = Vector2.zero;
            rt.offsetMax = Vector2.zero;

            imageComponent              = imageGO.AddComponent<Image>();
            imageComponent.color        = Color.white; // tinted by sprite; white = neutral
            imageComponent.raycastTarget = false;

            // CanvasGroup on root canvas
            var cg            = canvasGO.AddComponent<CanvasGroup>();
            cg.alpha           = 0f;
            cg.blocksRaycasts  = false;
            cg.interactable    = false;

            Debug.Log("[JumpscareSystem] Overlay canvas created automatically.");
            return cg;
        }

        // ── Editor Validation ─────────────────────────────────────────────────────

#if UNITY_EDITOR
        private void OnValidate()
        {
            _jumpscareImageDuration = Mathf.Max(0.1f, _jumpscareImageDuration);
            _sanityDamage           = Mathf.Clamp01(_sanityDamage);

            if (_jumpscareImages != null && _jumpscareImages.Length != 5)
            {
                Debug.LogWarning("[JumpscareSystem] _jumpscareImages should have exactly 5 sprites " +
                                 "(one per GhostId). Resize the array in the Inspector.");
            }
        }
#endif
    }
}
