using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BusShift.EditorTools
{
    /// <summary>
    /// Audita a fundação executável da Build 0.1.0.
    ///
    /// A ferramenta não considera uma cena pronta apenas porque existe um script
    /// com o nome esperado. A cena precisa existir como asset e estar registrada
    /// no EditorBuildSettings.
    /// </summary>
    public static class VerticalSliceBuildValidator
    {
        private const string MenuRoot = "Bus Shift/Vertical Slice/";
        private const string PlaceholderSceneFolder = "Assets/_Project/Scenes/VerticalSlice";

        private static readonly string[] RequiredSceneNames =
        {
            "Bootstrap",
            "MainMenu",
            "Day1Morning",
            "Day1Night",
            "SliceEnding"
        };

        [MenuItem(MenuRoot + "Validate Build Readiness", priority = 1)]
        public static void ValidateBuildReadiness()
        {
            ValidationReport report = BuildReport();
            report.LogToConsole();

            string title = report.IsReady
                ? "Build 0.1.0 foundation is ready"
                : "Build 0.1.0 foundation is blocked";

            EditorUtility.DisplayDialog(title, report.ToSummary(), "OK");
        }

        [MenuItem(MenuRoot + "Register Required Scenes", priority = 2)]
        public static void RegisterRequiredScenes()
        {
            Dictionary<string, string> scenePaths = FindScenePaths();
            List<string> missingSceneAssets = RequiredSceneNames
                .Where(sceneName => !scenePaths.ContainsKey(sceneName))
                .ToList();

            if (missingSceneAssets.Count > 0)
            {
                string missing = string.Join(", ", missingSceneAssets);
                Debug.LogError($"[VerticalSlice] Cannot register scenes. Missing assets: {missing}");
                EditorUtility.DisplayDialog(
                    "Scenes are missing",
                    $"Create these scene assets before registering the build: {missing}",
                    "OK");
                return;
            }

            List<EditorBuildSettingsScene> scenes = new List<EditorBuildSettingsScene>();

            foreach (string sceneName in RequiredSceneNames)
            {
                scenes.Add(new EditorBuildSettingsScene(scenePaths[sceneName], true));
            }

            foreach (EditorBuildSettingsScene existingScene in EditorBuildSettings.scenes)
            {
                bool alreadyIncluded = scenes.Any(scene =>
                    string.Equals(scene.path, existingScene.path, StringComparison.OrdinalIgnoreCase));

                if (!alreadyIncluded)
                {
                    scenes.Add(existingScene);
                }
            }

            EditorBuildSettings.scenes = scenes.ToArray();
            AssetDatabase.SaveAssets();

            Debug.Log("[VerticalSlice] Required scenes registered in Build Settings.");
            ValidateBuildReadiness();
        }

        [MenuItem(MenuRoot + "Create Missing Placeholder Scenes", priority = 3)]
        public static void CreateMissingPlaceholderScenes()
        {
            EnsureFolderExists(PlaceholderSceneFolder);

            Dictionary<string, string> scenePaths = FindScenePaths();
            Scene originalScene = SceneManager.GetActiveScene();
            string originalScenePath = originalScene.path;
            bool createdAnyScene = false;

            try
            {
                foreach (string sceneName in RequiredSceneNames)
                {
                    if (scenePaths.ContainsKey(sceneName))
                    {
                        continue;
                    }

                    Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
                    scene.name = sceneName;

                    CreatePlaceholderHierarchy(sceneName);

                    string path = $"{PlaceholderSceneFolder}/{sceneName}.unity";
                    EditorSceneManager.SaveScene(scene, path);
                    createdAnyScene = true;

                    Debug.Log($"[VerticalSlice] Placeholder scene created: {path}");
                }
            }
            finally
            {
                if (!string.IsNullOrWhiteSpace(originalScenePath) && File.Exists(originalScenePath))
                {
                    EditorSceneManager.OpenScene(originalScenePath, OpenSceneMode.Single);
                }
            }

            AssetDatabase.Refresh();

            if (!createdAnyScene)
            {
                Debug.Log("[VerticalSlice] All required scene assets already exist.");
            }

            RegisterRequiredScenes();
        }

        private static ValidationReport BuildReport()
        {
            Dictionary<string, string> scenePaths = FindScenePaths();
            HashSet<string> enabledBuildScenePaths = EditorBuildSettings.scenes
                .Where(scene => scene.enabled)
                .Select(scene => scene.path)
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            List<string> missingSceneAssets = new List<string>();
            List<string> scenesMissingFromBuild = new List<string>();

            foreach (string sceneName in RequiredSceneNames)
            {
                if (!scenePaths.TryGetValue(sceneName, out string path))
                {
                    missingSceneAssets.Add(sceneName);
                    continue;
                }

                if (!enabledBuildScenePaths.Contains(path))
                {
                    scenesMissingFromBuild.Add(sceneName);
                }
            }

            List<string> architectureWarnings = new List<string>();

            if (AssetExistsByFileName("GameManager.cs") && AssetExistsByFileName("GameStateManager.cs"))
            {
                architectureWarnings.Add(
                    "GameManager.cs and GameStateManager.cs both exist. Confirm one authoritative game state owner before integration.");
            }

            if (PlayerSettings.companyName == "DefaultCompany")
            {
                architectureWarnings.Add("PlayerSettings.companyName still uses DefaultCompany.");
            }

            if (PlayerSettings.productName == "bus_shift_unity_seed")
            {
                architectureWarnings.Add("PlayerSettings.productName still uses the seed project name.");
            }

            if (EditorBuildSettings.scenes.Length == 0)
            {
                architectureWarnings.Add("EditorBuildSettings contains no scenes.");
            }

            return new ValidationReport(
                missingSceneAssets,
                scenesMissingFromBuild,
                architectureWarnings);
        }

        private static Dictionary<string, string> FindScenePaths()
        {
            Dictionary<string, string> result = new Dictionary<string, string>(
                StringComparer.OrdinalIgnoreCase);

            string[] guids = AssetDatabase.FindAssets("t:Scene");

            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                string sceneName = Path.GetFileNameWithoutExtension(path);

                if (!result.ContainsKey(sceneName))
                {
                    result.Add(sceneName, path);
                }
                else
                {
                    Debug.LogWarning(
                        $"[VerticalSlice] Duplicate scene name detected: {sceneName}. " +
                        $"Using {result[sceneName]} and ignoring {path}.");
                }
            }

            return result;
        }

        private static bool AssetExistsByFileName(string fileName)
        {
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
            string[] guids = AssetDatabase.FindAssets(fileNameWithoutExtension);

            return guids
                .Select(AssetDatabase.GUIDToAssetPath)
                .Any(path => string.Equals(
                    Path.GetFileName(path),
                    fileName,
                    StringComparison.OrdinalIgnoreCase));
        }

        private static void CreatePlaceholderHierarchy(string sceneName)
        {
            GameObject root = new GameObject($"[{sceneName}]");

            GameObject cameraObject = new GameObject("Main Camera");
            cameraObject.tag = "MainCamera";
            cameraObject.transform.SetParent(root.transform);
            cameraObject.transform.position = new Vector3(0f, 1.6f, -10f);

            Camera camera = cameraObject.AddComponent<Camera>();
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = Color.black;

            if (sceneName == "Day1Morning" || sceneName == "Day1Night")
            {
                GameObject lightObject = new GameObject("Directional Light");
                lightObject.transform.SetParent(root.transform);
                lightObject.transform.rotation = Quaternion.Euler(50f, -30f, 0f);

                Light light = lightObject.AddComponent<Light>();
                light.type = LightType.Directional;
                light.intensity = sceneName == "Day1Morning" ? 1f : 0.2f;

                GameObject ground = GameObject.CreatePrimitive(PrimitiveType.Plane);
                ground.name = "Placeholder Ground";
                ground.transform.SetParent(root.transform);
                ground.transform.localScale = new Vector3(5f, 1f, 5f);
            }

            GameObject marker = new GameObject("PLACEHOLDER_SCENE_NOT_GAMEPLAY_READY");
            marker.transform.SetParent(root.transform);

            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
        }

        private static void EnsureFolderExists(string folderPath)
        {
            string[] parts = folderPath.Split('/');
            string current = parts[0];

            for (int index = 1; index < parts.Length; index++)
            {
                string next = $"{current}/{parts[index]}";

                if (!AssetDatabase.IsValidFolder(next))
                {
                    AssetDatabase.CreateFolder(current, parts[index]);
                }

                current = next;
            }
        }

        private sealed class ValidationReport
        {
            public ValidationReport(
                IReadOnlyList<string> missingSceneAssets,
                IReadOnlyList<string> scenesMissingFromBuild,
                IReadOnlyList<string> warnings)
            {
                MissingSceneAssets = missingSceneAssets;
                ScenesMissingFromBuild = scenesMissingFromBuild;
                Warnings = warnings;
            }

            public IReadOnlyList<string> MissingSceneAssets { get; }
            public IReadOnlyList<string> ScenesMissingFromBuild { get; }
            public IReadOnlyList<string> Warnings { get; }

            public bool IsReady =>
                MissingSceneAssets.Count == 0 &&
                ScenesMissingFromBuild.Count == 0;

            public string ToSummary()
            {
                List<string> lines = new List<string>
                {
                    IsReady
                        ? "All required scene assets are registered in Build Settings."
                        : "The vertical slice foundation is not ready."
                };

                if (MissingSceneAssets.Count > 0)
                {
                    lines.Add($"Missing scene assets: {string.Join(", ", MissingSceneAssets)}");
                }

                if (ScenesMissingFromBuild.Count > 0)
                {
                    lines.Add($"Scenes missing from Build Settings: {string.Join(", ", ScenesMissingFromBuild)}");
                }

                if (Warnings.Count > 0)
                {
                    lines.Add($"Warnings: {Warnings.Count}");
                }

                return string.Join(Environment.NewLine, lines);
            }

            public void LogToConsole()
            {
                string prefix = "[VerticalSlice]";

                if (IsReady)
                {
                    Debug.Log($"{prefix} Required scenes exist and are registered in Build Settings.");
                }
                else
                {
                    Debug.LogError($"{prefix} Build 0.1.0 foundation is blocked.");
                }

                foreach (string sceneName in MissingSceneAssets)
                {
                    Debug.LogError($"{prefix} Missing scene asset: {sceneName}");
                }

                foreach (string sceneName in ScenesMissingFromBuild)
                {
                    Debug.LogError($"{prefix} Scene not registered in Build Settings: {sceneName}");
                }

                foreach (string warning in Warnings)
                {
                    Debug.LogWarning($"{prefix} {warning}");
                }
            }
        }
    }
}
