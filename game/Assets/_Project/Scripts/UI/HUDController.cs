using UnityEngine;
using BusShift.Core;

namespace BusShift.UI
{
    /// <summary>
    /// Master HUD orchestrator. Owns references to every UI sub-system and
    /// responds to <see cref="GameManager.OnGameStateChanged"/> to show or
    /// hide the entire HUD as game state changes.
    ///
    /// Attach to a root Canvas GameObject that wraps all HUD elements.
    /// </summary>
    public class HUDController : MonoBehaviour
    {
        // ── HUD Sub-Systems ───────────────────────────────────────────────────

        [Header("HUD Sub-Systems")]
        [Tooltip("Post-processing tension/sanity visual feedback.")]
        [SerializeField] private TensionUI tensionUI;

        [Tooltip("Driver's wristwatch (SPACE key).")]
        [SerializeField] private WatchUI watchUI;

        [Tooltip("Interactive route map (TAB key).")]
        [SerializeField] private MapUI mapUI;

        // ── Cooldown Icons ────────────────────────────────────────────────────

        [Header("Intervention Cooldown Icons")]
        [Tooltip("Ordered as: Microphone, PanelLock, Radio, Headlight.")]
        [SerializeField] private CooldownIconUI[] cooldownIcons;

        // ── Root Canvas Group ─────────────────────────────────────────────────

        [Header("Root HUD")]
        [Tooltip("CanvasGroup on the root HUD object. Controls global show/hide.")]
        [SerializeField] private CanvasGroup hudCanvasGroup;

        [Tooltip("Fade speed when showing or hiding the HUD.")]
        [SerializeField] private float fadeSpeed = 5f;

        // ── Private state ─────────────────────────────────────────────────────

        private float _targetAlpha;

        // ── Lifecycle ─────────────────────────────────────────────────────────

        private void Awake()
        {
            // Start invisible; will be shown once Playing state is entered
            SetAlphaImmediate(0f);
        }

        private void OnEnable()
        {
            GameManager.OnGameStateChanged += HandleGameStateChanged;
        }

        private void OnDisable()
        {
            GameManager.OnGameStateChanged -= HandleGameStateChanged;
        }

        private void Update()
        {
            // Smooth fade for the root HUD canvas
            if (hudCanvasGroup == null) return;
            hudCanvasGroup.alpha = Mathf.Lerp(hudCanvasGroup.alpha, _targetAlpha, Time.deltaTime * fadeSpeed);
        }

        // ── Handlers ─────────────────────────────────────────────────────────

        private void HandleGameStateChanged(GameState state)
        {
            switch (state)
            {
                case GameState.Playing:
                    ShowHUD();
                    break;

                case GameState.Paused:
                    // Keep HUD visible but block interaction while paused
                    SetInteractable(false);
                    break;

                case GameState.MainMenu:
                case GameState.GameOver:
                    HideHUD();
                    break;
            }
        }

        // ── Public control ────────────────────────────────────────────────────

        /// <summary>Fades the HUD in and enables interaction.</summary>
        public void ShowHUD()
        {
            _targetAlpha = 1f;
            SetInteractable(true);
        }

        /// <summary>Fades the HUD out and disables interaction.</summary>
        public void HideHUD()
        {
            _targetAlpha = 0f;
            SetInteractable(false);
        }

        // ── Helpers ───────────────────────────────────────────────────────────

        private void SetAlphaImmediate(float alpha)
        {
            _targetAlpha = alpha;
            if (hudCanvasGroup != null)
                hudCanvasGroup.alpha = alpha;
        }

        private void SetInteractable(bool value)
        {
            if (hudCanvasGroup == null) return;
            hudCanvasGroup.interactable    = value;
            hudCanvasGroup.blocksRaycasts  = value;
        }
    }
}
