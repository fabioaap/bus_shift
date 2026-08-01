using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BusShift.VerticalSlice
{
    /// <summary>
    /// Provides a temporary, deterministic navigation path for the Build 0.1.0
    /// placeholder scenes.
    ///
    /// This component is intentionally small and isolated. It exists to prove that
    /// the project can start, navigate all required scenes, restart, and return to
    /// the menu outside the Unity Editor before gameplay integration begins.
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class VerticalSliceSceneController : MonoBehaviour
    {
        private static readonly IReadOnlyDictionary<string, string> NextSceneByName =
            new Dictionary<string, string>
            {
                { "Bootstrap", "MainMenu" },
                { "MainMenu", "Day1Morning" },
                { "Day1Morning", "Day1Night" },
                { "Day1Night", "SliceEnding" },
                { "SliceEnding", "MainMenu" }
            };

        [Header("Temporary Build 0.1.0 Navigation")]
        [Tooltip("When enabled, Bootstrap automatically loads MainMenu after one frame.")]
        [SerializeField] private bool _autoAdvanceBootstrap = true;

        [Tooltip("Show the temporary build navigation overlay.")]
        [SerializeField] private bool _showDebugOverlay = true;

        private bool _isLoading;
        private string _currentSceneName;
        private string _nextSceneName;

        private void Awake()
        {
            ResolveSceneRoute();
        }

        private IEnumerator Start()
        {
            if (_autoAdvanceBootstrap && _currentSceneName == "Bootstrap")
            {
                yield return null;
                LoadNextScene();
            }
        }

        private void Update()
        {
            if (_isLoading)
            {
                return;
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                LoadScene(_currentSceneName);
                return;
            }

            if (Input.GetKeyDown(KeyCode.Escape) && _currentSceneName != "MainMenu")
            {
                LoadScene("MainMenu");
                return;
            }

            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
            {
                LoadNextScene();
            }
        }

        private void ResolveSceneRoute()
        {
            _currentSceneName = SceneManager.GetActiveScene().name;
            NextSceneByName.TryGetValue(_currentSceneName, out _nextSceneName);
        }

        private void LoadNextScene()
        {
            if (string.IsNullOrWhiteSpace(_nextSceneName))
            {
                Debug.LogError($"[VerticalSlice] No next scene configured for {_currentSceneName}.");
                return;
            }

            LoadScene(_nextSceneName);
        }

        private void LoadScene(string sceneName)
        {
            if (_isLoading)
            {
                return;
            }

            if (!Application.CanStreamedLevelBeLoaded(sceneName))
            {
                Debug.LogError(
                    $"[VerticalSlice] Scene '{sceneName}' is not available in Build Settings.");
                return;
            }

            _isLoading = true;
            StartCoroutine(LoadSceneRoutine(sceneName));
        }

        private IEnumerator LoadSceneRoutine(string sceneName)
        {
            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

            if (operation == null)
            {
                _isLoading = false;
                Debug.LogError($"[VerticalSlice] Unity could not start loading '{sceneName}'.");
                yield break;
            }

            while (!operation.isDone)
            {
                yield return null;
            }
        }

        private void OnGUI()
        {
            if (!_showDebugOverlay)
            {
                return;
            }

            const float width = 520f;
            const float height = 150f;
            Rect panel = new Rect(24f, 24f, width, height);

            GUI.Box(panel, GUIContent.none);

            GUILayout.BeginArea(new Rect(panel.x + 16f, panel.y + 12f, width - 32f, height - 24f));
            GUILayout.Label("BUS SHIFT, BUILD 0.1.0 PLACEHOLDER FLOW");
            GUILayout.Label($"Current scene: {_currentSceneName}");
            GUILayout.Label($"Next scene: {(_nextSceneName ?? "not configured")}");
            GUILayout.Space(8f);
            GUILayout.Label("Enter or Space: advance   R: restart scene   Esc: return to menu");

            if (_isLoading)
            {
                GUILayout.Label("Loading...");
            }

            GUILayout.EndArea();
        }
    }
}
