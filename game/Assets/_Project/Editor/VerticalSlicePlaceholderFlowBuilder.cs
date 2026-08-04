using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BusShift.VerticalSlice;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BusShift.EditorTools
{
    /// <summary>
    /// Builds the temporary navigable scene flow used to prove the Build 0.1.0
    /// foundation before gameplay systems are integrated.
    /// </summary>
    public static class VerticalSlicePlaceholderFlowBuilder
    {
        private const string MenuPath = "Bus Shift/Vertical Slice/Build Navigable Placeholder Flow";

        private static readonly string[] RequiredSceneNames =
        {
            "Bootstrap",
            "MainMenu",
            "Day1Morning",
            "Day1Night",
            "SliceEnding"
        };

        [MenuItem(MenuPath, priority = 0)]
        public static void BuildNavigablePlaceholderFlow()
        {
            if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                Debug.LogWarning("[VerticalSlice] Placeholder flow build cancelled by the user.");
                return;
            }

            Scene originalScene = SceneManager.GetActiveScene();
            string originalScenePath = originalScene.path;

            try
            {
                VerticalSliceBuildValidator.CreateMissingPlaceholderScenes();

                Dictionary<string, string> scenePaths = FindRequiredScenePaths();
                List<string> missing = RequiredSceneNames
                    .Where(sceneName => !scenePaths.ContainsKey(sceneName))
                    .ToList();

                if (missing.Count > 0)
                {
                    Debug.LogError(
                        $"[VerticalSlice] Cannot complete placeholder flow. Missing scenes: " +
                        string.Join(", ", missing));
                    return;
                }

                foreach (string sceneName in RequiredSceneNames)
                {
                    Scene scene = EditorSceneManager.OpenScene(
                        scenePaths[sceneName],
                        OpenSceneMode.Single);

                    AttachController(scene, sceneName);
                    EditorSceneManager.SaveScene(scene);
                }

                VerticalSliceBuildValidator.RegisterRequiredScenes();

                Debug.Log(
                    "[VerticalSlice] Navigable placeholder flow is ready. " +
                    "Generate a development build and validate Bootstrap -> MainMenu -> " +
                    "Day1Morning -> Day1Night -> SliceEnding -> MainMenu.");

                EditorUtility.DisplayDialog(
                    "Placeholder flow ready",
                    "The five Build 0.1.0 scenes exist, contain the temporary navigation " +
                    "controller, and are registered in Build Settings.",
                    "OK");
            }
            finally
            {
                RestoreOriginalScene(originalScenePath);
            }
        }

        private static Dictionary<string, string> FindRequiredScenePaths()
        {
            Dictionary<string, string> result = new Dictionary<string, string>(
                StringComparer.OrdinalIgnoreCase);

            foreach (string guid in AssetDatabase.FindAssets("t:Scene"))
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                string name = Path.GetFileNameWithoutExtension(path);

                if (RequiredSceneNames.Contains(name) && !result.ContainsKey(name))
                {
                    result.Add(name, path);
                }
            }

            return result;
        }

        private static void AttachController(Scene scene, string sceneName)
        {
            VerticalSliceSceneController existing = UnityEngine.Object.FindAnyObjectByType<
                VerticalSliceSceneController>();

            if (existing != null)
            {
                Debug.Log($"[VerticalSlice] Controller already present in {sceneName}.");
                return;
            }

            GameObject root = scene.GetRootGameObjects()
                .FirstOrDefault(gameObject => gameObject.name == $"[{sceneName}]");

            if (root == null)
            {
                root = new GameObject($"[{sceneName}]");
                SceneManager.MoveGameObjectToScene(root, scene);
            }

            Undo.AddComponent<VerticalSliceSceneController>(root);
            EditorSceneManager.MarkSceneDirty(scene);

            Debug.Log($"[VerticalSlice] Navigation controller attached to {sceneName}.");
        }

        private static void RestoreOriginalScene(string originalScenePath)
        {
            if (string.IsNullOrWhiteSpace(originalScenePath))
            {
                return;
            }

            string absolutePath = Path.GetFullPath(originalScenePath);
            if (!File.Exists(absolutePath))
            {
                Debug.LogWarning(
                    $"[VerticalSlice] Original scene could not be restored: {originalScenePath}");
                return;
            }

            EditorSceneManager.OpenScene(originalScenePath, OpenSceneMode.Single);
        }
    }
}
