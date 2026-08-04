using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace BusShift.EditorTools
{
    /// <summary>
    /// Gera um inventário somente leitura dos assets candidatos da Build 0.1.0.
    ///
    /// A ferramenta não altera import settings, prefabs, materiais, cenas ou assets
    /// de terceiros. O resultado é um relatório Markdown versionável no repositório.
    /// </summary>
    public static class Build010AssetAuditReporter
    {
        private const string MenuPath = "Bus Shift/Assets/Generate Build 0.1.0 Audit Report";
        private const string ReportRelativePath = "docs/game/art/BUILD_0_1_0_ASSET_AUDIT_GENERATED.md";
        private const int CandidateLimit = 40;

        private static readonly AssetSource[] Sources =
        {
            new AssetSource("School Bus", "Assets/School Bus"),
            new AssetSource("Synty", "Assets/Synty"),
            new AssetSource("Street Vehicles", "Assets/Vladislav Simakov"),
            new AssetSource("Bus Shift first-party", "Assets/_Project")
        };

        private static readonly string[] BusTokens =
        {
            "bus", "schoolbus", "school_bus", "school bus", "amtran"
        };

        private static readonly string[] EnvironmentTokens =
        {
            "road", "street", "building", "house", "school", "depot", "fence",
            "stop", "sidewalk", "tree", "industrial", "warehouse", "lamp", "sign"
        };

        private static readonly string[] CharacterTokens =
        {
            "child", "kid", "boy", "girl", "student", "character", "human", "people"
        };

        [MenuItem(MenuPath, priority = 20)]
        public static void GenerateBuild010AssetAudit()
        {
            try
            {
                AssetDatabase.SaveAssets();

                AuditReport report = BuildReport();
                string outputPath = GetReportOutputPath();
                string outputDirectory = Path.GetDirectoryName(outputPath);

                if (string.IsNullOrWhiteSpace(outputDirectory))
                {
                    throw new InvalidOperationException("Could not resolve the asset audit output directory.");
                }

                Directory.CreateDirectory(outputDirectory);
                File.WriteAllText(outputPath, report.ToMarkdown(), new UTF8Encoding(false));

                report.LogToConsole(outputPath);
                EditorUtility.RevealInFinder(outputPath);
                EditorUtility.DisplayDialog(
                    "Build 0.1.0 asset audit generated",
                    $"Report written to:\n{outputPath}\n\nWarnings: {report.WarningCount}",
                    "OK");
            }
            catch (Exception exception)
            {
                Debug.LogException(exception);
                EditorUtility.DisplayDialog(
                    "Asset audit failed",
                    "The report could not be generated. Inspect the Unity Console for the exact error.",
                    "OK");
                throw;
            }
        }

        private static AuditReport BuildReport()
        {
            List<SourceAudit> sourceAudits = Sources
                .Select(AuditSource)
                .ToList();

            List<string> allCandidatePaths = sourceAudits
                .SelectMany(source => source.AssetPaths)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .OrderBy(path => path, StringComparer.OrdinalIgnoreCase)
                .ToList();

            List<string> busCandidates = FindCandidates(allCandidatePaths, BusTokens);
            List<string> environmentCandidates = FindCandidates(allCandidatePaths, EnvironmentTokens);
            List<string> characterCandidates = FindCandidates(allCandidatePaths, CharacterTokens);

            return new AuditReport(
                DateTime.Now,
                Application.unityVersion,
                sourceAudits,
                busCandidates,
                environmentCandidates,
                characterCandidates);
        }

        private static SourceAudit AuditSource(AssetSource source)
        {
            bool exists = AssetDatabase.IsValidFolder(source.AssetRoot);

            if (!exists)
            {
                return SourceAudit.Missing(source);
            }

            string[] guids = AssetDatabase.FindAssets(string.Empty, new[] { source.AssetRoot });
            List<string> paths = guids
                .Select(AssetDatabase.GUIDToAssetPath)
                .Where(path => !string.IsNullOrWhiteSpace(path))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .OrderBy(path => path, StringComparer.OrdinalIgnoreCase)
                .ToList();

            List<string> modelPaths = paths
                .Where(path => AssetImporter.GetAtPath(path) is ModelImporter)
                .ToList();

            List<string> prefabPaths = FindTypedPaths("t:Prefab", source.AssetRoot);
            List<string> materialPaths = FindTypedPaths("t:Material", source.AssetRoot);
            List<string> texturePaths = FindTypedPaths("t:Texture", source.AssetRoot);
            List<string> audioPaths = FindTypedPaths("t:AudioClip", source.AssetRoot);
            List<string> animationPaths = FindTypedPaths("t:AnimationClip", source.AssetRoot);

            int meshCount = 0;
            int skinnedMeshCount = 0;

            foreach (string modelPath in modelPaths)
            {
                UnityEngine.Object[] subAssets = AssetDatabase.LoadAllAssetsAtPath(modelPath);
                meshCount += subAssets.OfType<Mesh>().Count();

                GameObject modelRoot = AssetDatabase.LoadAssetAtPath<GameObject>(modelPath);
                if (modelRoot != null)
                {
                    skinnedMeshCount += modelRoot
                        .GetComponentsInChildren<SkinnedMeshRenderer>(true)
                        .Length;
                }
            }

            int missingScriptCount = 0;

            foreach (string prefabPath in prefabPaths)
            {
                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
                if (prefab == null)
                {
                    continue;
                }

                Component[] components = prefab.GetComponentsInChildren<Component>(true);
                missingScriptCount += components.Count(component => component == null);
            }

            int brokenMaterialCount = 0;
            Dictionary<string, int> shaderUsage = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

            foreach (string materialPath in materialPaths)
            {
                Material material = AssetDatabase.LoadAssetAtPath<Material>(materialPath);
                string shaderName = material != null && material.shader != null
                    ? material.shader.name
                    : "<missing shader>";

                if (material == null || material.shader == null ||
                    string.Equals(shaderName, "Hidden/InternalErrorShader", StringComparison.OrdinalIgnoreCase))
                {
                    brokenMaterialCount++;
                }

                shaderUsage.TryGetValue(shaderName, out int currentCount);
                shaderUsage[shaderName] = currentCount + 1;
            }

            int lfsPointerCount = CountGitLfsPointers(source.AssetRoot);

            return new SourceAudit(
                source,
                true,
                paths,
                modelPaths.Count,
                prefabPaths.Count,
                materialPaths.Count,
                texturePaths.Count,
                audioPaths.Count,
                animationPaths.Count,
                meshCount,
                skinnedMeshCount,
                missingScriptCount,
                brokenMaterialCount,
                lfsPointerCount,
                shaderUsage);
        }

        private static List<string> FindTypedPaths(string filter, string root)
        {
            return AssetDatabase.FindAssets(filter, new[] { root })
                .Select(AssetDatabase.GUIDToAssetPath)
                .Where(path => !string.IsNullOrWhiteSpace(path))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .OrderBy(path => path, StringComparer.OrdinalIgnoreCase)
                .ToList();
        }

        private static List<string> FindCandidates(
            IReadOnlyList<string> paths,
            IReadOnlyList<string> tokens)
        {
            return paths
                .Where(path => tokens.Any(token =>
                    path.IndexOf(token, StringComparison.OrdinalIgnoreCase) >= 0))
                .Take(CandidateLimit)
                .ToList();
        }

        private static int CountGitLfsPointers(string assetRoot)
        {
            string projectRoot = Directory.GetParent(Application.dataPath)?.FullName;
            if (string.IsNullOrWhiteSpace(projectRoot))
            {
                return 0;
            }

            string physicalRoot = Path.Combine(
                projectRoot,
                assetRoot.Replace('/', Path.DirectorySeparatorChar));

            if (!Directory.Exists(physicalRoot))
            {
                return 0;
            }

            int count = 0;

            foreach (string filePath in Directory.EnumerateFiles(
                         physicalRoot,
                         "*",
                         SearchOption.AllDirectories))
            {
                if (filePath.EndsWith(".meta", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                try
                {
                    FileInfo info = new FileInfo(filePath);
                    if (info.Length <= 0 || info.Length > 1024)
                    {
                        continue;
                    }

                    using FileStream stream = File.OpenRead(filePath);
                    using StreamReader reader = new StreamReader(
                        stream,
                        Encoding.UTF8,
                        true,
                        256,
                        false);

                    string firstLine = reader.ReadLine();
                    if (string.Equals(
                            firstLine,
                            "version https://git-lfs.github.com/spec/v1",
                            StringComparison.Ordinal))
                    {
                        count++;
                    }
                }
                catch (IOException)
                {
                    // The file may be temporarily locked by Unity import. Skip it and keep the audit read-only.
                }
                catch (UnauthorizedAccessException)
                {
                    // Report generation should continue even if a vendor file cannot be inspected.
                }
            }

            return count;
        }

        private static string GetReportOutputPath()
        {
            DirectoryInfo projectRoot = Directory.GetParent(Application.dataPath);
            DirectoryInfo repositoryRoot = projectRoot?.Parent;

            if (repositoryRoot == null)
            {
                throw new InvalidOperationException(
                    "Could not resolve the repository root from Application.dataPath.");
            }

            return Path.GetFullPath(Path.Combine(repositoryRoot.FullName, ReportRelativePath));
        }

        private readonly struct AssetSource
        {
            public AssetSource(string name, string assetRoot)
            {
                Name = name;
                AssetRoot = assetRoot;
            }

            public string Name { get; }
            public string AssetRoot { get; }
        }

        private sealed class SourceAudit
        {
            public SourceAudit(
                AssetSource source,
                bool exists,
                IReadOnlyList<string> assetPaths,
                int modelCount,
                int prefabCount,
                int materialCount,
                int textureCount,
                int audioCount,
                int animationCount,
                int meshCount,
                int skinnedMeshCount,
                int missingScriptCount,
                int brokenMaterialCount,
                int lfsPointerCount,
                IReadOnlyDictionary<string, int> shaderUsage)
            {
                Source = source;
                Exists = exists;
                AssetPaths = assetPaths;
                ModelCount = modelCount;
                PrefabCount = prefabCount;
                MaterialCount = materialCount;
                TextureCount = textureCount;
                AudioCount = audioCount;
                AnimationCount = animationCount;
                MeshCount = meshCount;
                SkinnedMeshCount = skinnedMeshCount;
                MissingScriptCount = missingScriptCount;
                BrokenMaterialCount = brokenMaterialCount;
                LfsPointerCount = lfsPointerCount;
                ShaderUsage = shaderUsage;
            }

            public AssetSource Source { get; }
            public bool Exists { get; }
            public IReadOnlyList<string> AssetPaths { get; }
            public int ModelCount { get; }
            public int PrefabCount { get; }
            public int MaterialCount { get; }
            public int TextureCount { get; }
            public int AudioCount { get; }
            public int AnimationCount { get; }
            public int MeshCount { get; }
            public int SkinnedMeshCount { get; }
            public int MissingScriptCount { get; }
            public int BrokenMaterialCount { get; }
            public int LfsPointerCount { get; }
            public IReadOnlyDictionary<string, int> ShaderUsage { get; }

            public int WarningCount =>
                (Exists ? 0 : 1) +
                MissingScriptCount +
                BrokenMaterialCount +
                LfsPointerCount;

            public static SourceAudit Missing(AssetSource source)
            {
                return new SourceAudit(
                    source,
                    false,
                    Array.Empty<string>(),
                    0,
                    0,
                    0,
                    0,
                    0,
                    0,
                    0,
                    0,
                    0,
                    0,
                    0,
                    new Dictionary<string, int>());
            }
        }

        private sealed class AuditReport
        {
            public AuditReport(
                DateTime generatedAt,
                string unityVersion,
                IReadOnlyList<SourceAudit> sources,
                IReadOnlyList<string> busCandidates,
                IReadOnlyList<string> environmentCandidates,
                IReadOnlyList<string> characterCandidates)
            {
                GeneratedAt = generatedAt;
                UnityVersion = unityVersion;
                Sources = sources;
                BusCandidates = busCandidates;
                EnvironmentCandidates = environmentCandidates;
                CharacterCandidates = characterCandidates;
            }

            public DateTime GeneratedAt { get; }
            public string UnityVersion { get; }
            public IReadOnlyList<SourceAudit> Sources { get; }
            public IReadOnlyList<string> BusCandidates { get; }
            public IReadOnlyList<string> EnvironmentCandidates { get; }
            public IReadOnlyList<string> CharacterCandidates { get; }

            public int WarningCount => Sources.Sum(source => source.WarningCount);

            public string ToMarkdown()
            {
                StringBuilder builder = new StringBuilder();

                builder.AppendLine("# Bus Shift — Auditoria gerada de assets da Build 0.1.0");
                builder.AppendLine();
                builder.AppendLine($"- Gerado em: `{GeneratedAt:yyyy-MM-dd HH:mm:ss}`");
                builder.AppendLine($"- Unity: `{UnityVersion}`");
                builder.AppendLine($"- Avisos automáticos: `{WarningCount}`");
                builder.AppendLine("- Modo: somente leitura; nenhum asset foi alterado");
                builder.AppendLine();
                builder.AppendLine("> Este relatório apresenta inventário e candidatos por nome/caminho. Ele não aprova qualidade visual, licença, pivôs, escala, rig, colliders ou adequação ao gameplay. Essas decisões exigem inspeção no Editor.");
                builder.AppendLine();

                builder.AppendLine("## Resumo por fonte");
                builder.AppendLine();
                builder.AppendLine("| Fonte | Existe | Assets | Modelos | Prefabs | Materiais | Texturas | Áudio | Animações | Meshes | Skinned meshes | Missing scripts | Materiais quebrados | Ponteiros LFS |");
                builder.AppendLine("|---|---:|---:|---:|---:|---:|---:|---:|---:|---:|---:|---:|---:|---:|");

                foreach (SourceAudit source in Sources)
                {
                    builder.AppendLine(
                        $"| {Escape(source.Source.Name)} | {(source.Exists ? "sim" : "não")} | {source.AssetPaths.Count} | {source.ModelCount} | {source.PrefabCount} | {source.MaterialCount} | {source.TextureCount} | {source.AudioCount} | {source.AnimationCount} | {source.MeshCount} | {source.SkinnedMeshCount} | {source.MissingScriptCount} | {source.BrokenMaterialCount} | {source.LfsPointerCount} |");
                }

                builder.AppendLine();
                builder.AppendLine("## Shaders encontrados");
                builder.AppendLine();

                foreach (SourceAudit source in Sources.Where(item => item.Exists))
                {
                    builder.AppendLine($"### {source.Source.Name}");
                    builder.AppendLine();

                    if (source.ShaderUsage.Count == 0)
                    {
                        builder.AppendLine("Nenhum material encontrado.");
                        builder.AppendLine();
                        continue;
                    }

                    builder.AppendLine("| Shader | Materiais |");
                    builder.AppendLine("|---|---:|");

                    foreach (KeyValuePair<string, int> shader in source.ShaderUsage
                                 .OrderByDescending(pair => pair.Value)
                                 .ThenBy(pair => pair.Key, StringComparer.OrdinalIgnoreCase)
                                 .Take(25))
                    {
                        builder.AppendLine($"| `{Escape(shader.Key)}` | {shader.Value} |");
                    }

                    builder.AppendLine();
                }

                AppendCandidateSection(builder, "Candidatos relacionados ao ônibus", BusCandidates);
                AppendCandidateSection(builder, "Candidatos relacionados ao cenário", EnvironmentCandidates);
                AppendCandidateSection(builder, "Candidatos relacionados a personagens", CharacterCandidates);

                builder.AppendLine("## Gates manuais ainda obrigatórios");
                builder.AppendLine();
                builder.AppendLine("- [ ] Git LFS completamente baixado e zero ponteiros pendentes");
                builder.AppendLine("- [ ] Bus 104 inspecionado em escala, pivô, porta, rodas, interior e colliders");
                builder.AppendLine("- [ ] Materiais convertidos ou compatíveis com URP sem shader rosa");
                builder.AppendLine("- [ ] Kit Synty escolhido para as três paradas e rota curta");
                builder.AppendLine("- [ ] Base de passageiros validada ou decisão de produzir/adquirir registrada");
                builder.AppendLine("- [ ] Base de Emma validada ou decisão de modelagem autoral registrada");
                builder.AppendLine("- [ ] Origem e licença de cada asset utilizado registradas");
                builder.AppendLine("- [ ] Prefabs derivados criados em `Assets/_Project`, preservando vendor assets");
                builder.AppendLine("- [ ] Capturas de evidência anexadas à issue #75");

                return builder.ToString();
            }

            public void LogToConsole(string outputPath)
            {
                Debug.Log(
                    $"[AssetAudit] Build 0.1.0 asset report generated at {outputPath}. " +
                    $"Sources: {Sources.Count}, warnings: {WarningCount}.");

                foreach (SourceAudit source in Sources)
                {
                    if (!source.Exists)
                    {
                        Debug.LogError($"[AssetAudit] Missing asset root: {source.Source.AssetRoot}");
                    }

                    if (source.LfsPointerCount > 0)
                    {
                        Debug.LogError(
                            $"[AssetAudit] {source.Source.Name} contains " +
                            $"{source.LfsPointerCount} unresolved Git LFS pointer(s).");
                    }

                    if (source.MissingScriptCount > 0)
                    {
                        Debug.LogWarning(
                            $"[AssetAudit] {source.Source.Name} contains " +
                            $"{source.MissingScriptCount} missing prefab script reference(s).");
                    }

                    if (source.BrokenMaterialCount > 0)
                    {
                        Debug.LogWarning(
                            $"[AssetAudit] {source.Source.Name} contains " +
                            $"{source.BrokenMaterialCount} missing/error shader material(s).");
                    }
                }
            }

            private static void AppendCandidateSection(
                StringBuilder builder,
                string title,
                IReadOnlyList<string> paths)
            {
                builder.AppendLine($"## {title}");
                builder.AppendLine();

                if (paths.Count == 0)
                {
                    builder.AppendLine("Nenhum candidato localizado por nome ou caminho.");
                    builder.AppendLine();
                    return;
                }

                foreach (string path in paths)
                {
                    builder.AppendLine($"- `{path}`");
                }

                builder.AppendLine();
            }

            private static string Escape(string value)
            {
                return string.IsNullOrEmpty(value)
                    ? string.Empty
                    : value.Replace("|", "\\|");
            }
        }
    }
}
