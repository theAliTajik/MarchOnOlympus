using UnityEditor;
using UnityEditor.Compilation;
using UnityEngine;
using System.IO;
using System;
using System.Collections.Generic;

public class ToolInventFinisherGenerator : EditorWindow
{
    // Paths - adjust as needed
    private const string ACTION_SCRIPTS_PATH = "Assets/Scripts/ScriptableObjects/InventFinishers/Actions/";
    private const string FINISHER_SCRIPTS_PATH = "Assets/Scripts/ScriptableObjects/InventFinishers/";
    private const string SO_ASSETS_PATH = "Assets/Data/Resources/InventFinishers/";
    private const string ACTION_SCRIPT_TEMPLATE_PATH = "Assets/Scripts/ScriptableObjects/InventFinishers/_InventActionTemplate.txt";

    // UI fields
    private string m_name = "NewFinisher";
    private int m_levelCount = 3;


    // Internal state / results
    private float m_afterCompileTime = -1f;

    private string
        m_finisherClassNameForSO = ""; // the actual class name used for finisher SO creation, e.g. "InventFinisher"

    private List<(string scriptFile, string soAsset)> m_generatedFiles = new List<(string, string)>();
    private List<string> m_errors = new List<string>();
    private string m_finisherSoPath = "";

    [MenuItem("Tools/Invent Finisher Generator")]
    public static void ShowWindow()
    {
        GetWindow<ToolInventFinisherGenerator>("Invent Finisher Generator");
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("Create an Invent Finisher and level actions", EditorStyles.boldLabel);

        m_name = EditorGUILayout.TextField("Name", m_name);
        m_levelCount = EditorGUILayout.IntField("Level Count", m_levelCount);

        EditorGUILayout.Space();

        if (GUILayout.Button("Generate"))
        {
            // Clear previous results
            m_generatedFiles.Clear();
            m_errors.Clear();
            m_finisherSoPath = "";

            // 1) Validation
            if (string.IsNullOrWhiteSpace(m_name))
            {
                Debug.LogError("Name cannot be empty.");
                m_errors.Add("Name cannot be empty.");
                return;
            }

            if (m_levelCount <= 0)
            {
                Debug.LogError("Level Count must be greater than zero.");
                m_errors.Add("Level Count must be greater than zero.");
                return;
            }

            // Read template files
            string actionScriptTemplate = "";
            string soTemplate = "";
            try
            {
                if (!string.IsNullOrWhiteSpace(ACTION_SCRIPT_TEMPLATE_PATH) && File.Exists(ACTION_SCRIPT_TEMPLATE_PATH))
                {
                    actionScriptTemplate = File.ReadAllText(ACTION_SCRIPT_TEMPLATE_PATH);
                }
            }
            catch (Exception ex)
            {
                string err = $"Failed to read action script template: {ex.Message}";
                Debug.LogError(err);
                m_errors.Add(err);
                return;
            }
            // try
            // {
            //     if (!string.IsNullOrWhiteSpace(SO_TEMPLATE_PATH) && File.Exists(SO_TEMPLATE_PATH))
            //     {
            //         soTemplate = File.ReadAllText(SO_TEMPLATE_PATH);
            //     }
            // }
            // catch (Exception ex)
            // {
            //     string err = $"Failed to read SO template: {ex.Message}";
            //     Debug.LogError(err);
            //     m_errors.Add(err);
            //     return;
            // }
            
            if (string.IsNullOrWhiteSpace(actionScriptTemplate))
            {
                Debug.LogError("Action script template is required.");
                m_errors.Add("Action script template is required.");
                return;
            }

            // 2) Prepare directories
            try
            {
                Directory.CreateDirectory(ACTION_SCRIPTS_PATH);
                Directory.CreateDirectory(FINISHER_SCRIPTS_PATH);
                Directory.CreateDirectory(SO_ASSETS_PATH);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to create directories: {ex.Message}");
                m_errors.Add($"Failed to create directories: {ex.Message}");
                return;
            }

            // 3) InventFinisher SO creation will be deferred until compilation completes (if new classes were created)
            // We'll write an empty finisher SO now using the assumed type "InventFinisher" if it exists in the project;
            // otherwise we'll attempt to create it after compilation.

            // Decide finisher class name used for scriptable object creation
            // If user provided a name that already contains "InventFinisher", use it; otherwise append "InventFinisher"
            string finisherClassName = m_name.EndsWith("InventFinisher") ? m_name : m_name + "InventFinisher";
            m_finisherClassNameForSO = finisherClassName;

            // Path for the finisher asset (file name uses the provided name without spaces)
            string finisherFileName = m_name.Replace(" ", "");
            m_finisherSoPath = Path.Combine(SO_ASSETS_PATH, $"{finisherFileName}.asset").Replace("\\", "/");

            // 4) Generate action scripts for each level
            for (int level = 1; level <= m_levelCount; level++)
            {
                // create a single sanitized base name (no spaces) and use it consistently for script class names and SO filenames
                string sanitizedBaseName = m_name.Replace(" ", "");
                string actionClassBase = sanitizedBaseName + "_InventAction_Level" + level; // include underscore for consistency
                string actionFileName = $"{actionClassBase}.cs";
                string actionFilePath = Path.Combine(ACTION_SCRIPTS_PATH, actionFileName).Replace("\\", "/");

                try
                {
                    // Replace tokens in template
                    string actionContent = actionScriptTemplate
                        .Replace("_NAME_", sanitizedBaseName)
                        .Replace("_LEVEL_", level.ToString());

                    // Basic safety: ensure we don't overwrite an existing file without notice
                    if (File.Exists(actionFilePath))
                    {
                        string err = $"Script file already exists: {actionFilePath}";
                        Debug.LogError(err);
                        m_errors.Add(err);
                        // skip this file (do not overwrite)
                        continue;
                    }

                    File.WriteAllText(actionFilePath, actionContent);

                    // validate creation
                    if (File.Exists(actionFilePath))
                    {
                        m_generatedFiles.Add((actionFilePath, "")); // SO path will be updated after SO creation
                        Debug.Log($"Created action script: {actionFilePath}");
                    }
                    else
                    {
                        string err = $"Failed to write action script: {actionFilePath}";
                        Debug.LogError(err);
                        m_errors.Add(err);
                        // Halt generation if file didn't write
                        return;
                    }
                }
                catch (Exception ex)
                {
                    string err = $"Exception writing script {actionFilePath}: {ex.Message}";
                    Debug.LogError(err);
                    m_errors.Add(err);
                    return;
                }
            }

            // 5) Optionally generate a finisher script/class file if dev wants (we add a small placeholder so compile ensures type exists)
            // We'll create a minimal finisher class file only if it doesn't already exist.
            string finisherScriptName = finisherClassName + ".cs";
            string finisherScriptPath = Path.Combine(FINISHER_SCRIPTS_PATH, finisherScriptName).Replace("\\", "/");
            if (!File.Exists(finisherScriptPath))
            {
                try
                {
                    string finisherScriptContent =
                        $@"using UnityEngine;

// Auto-generated placeholder InventFinisher class for {finisherClassName}
// Replace or expand this class as appropriate in your project.
public class {finisherClassName} : ScriptableObject
{{
    // Add public fields and properties used by your system here.
}}";
                    File.WriteAllText(finisherScriptPath, finisherScriptContent);

                    if (File.Exists(finisherScriptPath))
                    {
                        Debug.Log($"Created finisher script: {finisherScriptPath}");
                    }
                    else
                    {
                        string err = $"Failed to write finisher script: {finisherScriptPath}";
                        Debug.LogError(err);
                        m_errors.Add(err);
                    }
                }
                catch (Exception ex)
                {
                    string err = $"Exception writing finisher script {finisherScriptPath}: {ex.Message}";
                    Debug.LogError(err);
                    m_errors.Add(err);
                }
            }
            else
            {
                Debug.Log($"Finisher script already exists: {finisherScriptPath}");
            }

            // 6) Save assets & request compilation, then defer SO creation until after compile
            // Request script compilation so newly-written action scripts are compiled and available as types.
            CompilationPipeline.compilationFinished += CompilationFinishedForInventFinisher;
            AssetDatabase.Refresh();

            // Fallback: if compilationFinished doesn't fire (e.g., nothing new to compile), ensure SOs are created anyway.
            // EditorApplication.delayCall += () =>
            // {
            //     // wait briefly to give Unity time to refresh scripts
            //     EditorApplication.delayCall += () =>
            //     {
            //         if (m_afterCompileTime < 0f)
            //         {
            //             Debug.Log("Compilation did not trigger; creating SOs directly.");
            //             TryCreateFinisherSO();
            //             TryCreateActionLevelSOs();
            //         }
            //     };
            // };

            // a tiny delay/time will be set by CompilationFinishedForInventFinisher when compilation completes.
            m_afterCompileTime = -1f;
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Summary", EditorStyles.boldLabel);

        EditorGUILayout.LabelField("Finisher SO Path:", m_finisherSoPath);
        EditorGUILayout.LabelField("Generated Scripts:");
        foreach (var pair in m_generatedFiles)
        {
            EditorGUILayout.LabelField($" - Script: {pair.scriptFile}   SO: {pair.soAsset}");
        }

        if (m_errors.Count > 0)
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Errors:", EditorStyles.boldLabel);
            foreach (var err in m_errors)
            {
                EditorGUILayout.LabelField(" - " + err);
            }
        }
    }

    // Called when compilation finishes: set a short timer to create SOs in the Editor update loop
    private void CompilationFinishedForInventFinisher(object _)
    {
        m_afterCompileTime = Time.realtimeSinceStartup + 0.1f;
    }

    protected virtual void OnEnable()
    {
        EditorApplication.update += OnEditorUpdateForInventFinisher;
    }

    protected virtual void OnDisable()
    {
        EditorApplication.update -= OnEditorUpdateForInventFinisher;
        // CompilationPipeline.compilationFinished -= CompilationFinishedForInventFinisher;
    }

    protected virtual void OnEditorUpdateForInventFinisher()
    {
        if (m_afterCompileTime > 0f && m_afterCompileTime < Time.realtimeSinceStartup)
        {
            // Attempt to create the main InventFinisher ScriptableObject
            TryCreateFinisherSO();

            // Attempt to create SOs for each generated action script
            TryCreateActionLevelSOs();

            // reset timer
            m_afterCompileTime = -1f;

            // detach handler so future compiles don't trigger duplicates
            CompilationPipeline.compilationFinished -= CompilationFinishedForInventFinisher;
        }
    }

    private void TryCreateFinisherSO()
    {
        // Always try to create ScriptableObject of type "InventFinisher"
        try
        {
            UnityEngine.Object asset = ScriptableObject.CreateInstance("InventFinisher");
            if (asset == null)
            {
                asset = ScriptableObject.CreateInstance(typeof(ScriptableObject));
                Debug.LogWarning("Could not create instance of InventFinisher; created a generic ScriptableObject placeholder instead.");
                m_errors.Add("Could not create instance of InventFinisher; created a generic placeholder SO.");
            }

            // Ensure folder exists
            Directory.CreateDirectory(SO_ASSETS_PATH);

            // Ensure unique path
            string finalPath = AssetDatabase.GenerateUniqueAssetPath(m_finisherSoPath);
            AssetDatabase.CreateAsset(asset, finalPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            

            // Validate
            var createdAsset = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(finalPath);
            if (createdAsset != null)
            {
                Debug.Log($"Created InventFinisher SO at {finalPath}");
                m_finisherSoPath = finalPath;
            }
            else
            {
                string err = $"Failed to create InventFinisher SO at {finalPath}";
                Debug.LogError(err);
                m_errors.Add(err);
            }
        }
        catch (Exception ex)
        {
            string err = $"Exception creating finisher SO: {ex.Message}";
            Debug.LogError(err);
            m_errors.Add(err);
        }
    }

    private void TryCreateActionLevelSOs()
    {
        // For each previously generated script, create a corresponding SO asset using the SO template.
        for (int i = 0; i < m_generatedFiles.Count; i++)
        {
            var entry = m_generatedFiles[i];
            // derive level from file name: look for "_LevelX" or "LevelX" suffix
            string fileName = Path.GetFileNameWithoutExtension(entry.scriptFile);
            int level = ExtractLevelFromName(fileName);

            // use the same sanitizedBaseName (no spaces) and the same underscore pattern as the action class names
            string soFileName = $"{m_name.Replace(" ", "")}_InventAction_Level{level}.asset";
            string soPath = Path.Combine(SO_ASSETS_PATH, soFileName).Replace("\\", "/");

            // Prepare SO text content from template (developer's template) and write to disk as a .asset placeholder file
            // Note: Unity's .asset binary format can't be directly written; instead create ScriptableObject instances in Editor.
            try
            {
                // Try to instantiate a ScriptableObject of a matching class name (the action class)
                string expectedActionClassName = fileName; // assumes action class name matches file name
                UnityEngine.Object actionAsset = null;
                try
                {
                    actionAsset = ScriptableObject.CreateInstance(expectedActionClassName);
                }
                catch
                {
                    actionAsset = null;
                }

                if (actionAsset == null)
                {
                    // fallback to generic ScriptableObject
                    actionAsset = ScriptableObject.CreateInstance(typeof(ScriptableObject));
                    m_errors.Add(
                        $"Could not instantiate action SO of type '{expectedActionClassName}'. Created generic placeholder instead.");
                }

                // ensure directory
                Directory.CreateDirectory(SO_ASSETS_PATH);

                string uniquePath = AssetDatabase.GenerateUniqueAssetPath(soPath);
                AssetDatabase.CreateAsset(actionAsset, uniquePath);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();

                var created = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(uniquePath);
                if (created != null)
                {
                    // store the created SO path in the generated files list
                    m_generatedFiles[i] = (entry.scriptFile, uniquePath);
                    Debug.Log($"Created action SO: {uniquePath}");
                }
                else
                {
                    string err = $"Failed to create action SO at {uniquePath}";
                    Debug.LogError(err);
                    m_errors.Add(err);
                }
            }
            catch (Exception ex)
            {
                string err = $"Exception creating action SO for {entry.scriptFile}: {ex.Message}";
                Debug.LogError(err);
                m_errors.Add(err);
            }
        }
    }

    private int ExtractLevelFromName(string fileName)
    {
        // attempt to extract a trailing "LevelX" or "_LevelX" pattern
        for (int lvl = 1; lvl <= 999; lvl++)
        {
            if (fileName.EndsWith($"Level{lvl}") || fileName.EndsWith($"_Level{lvl}") ||
                fileName.Contains($"_Level{lvl}"))
                return lvl;
        }

        return 0;
    }
}
