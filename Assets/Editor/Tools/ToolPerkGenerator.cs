#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;
using System.IO;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor.Compilation;
using UnityEditor.Localization.Plugins.XLIFF.V20;

public class ToolPerkGenerator : EditorWindow
{
    private const string CLASS_TEMPLATE_PATH = "Assets/Scripts/Game/Perks/Perks/_PerksTemplate.txt";
    private const string CLASS_PATH = "Assets/Scripts/Game/Perks/Perks/";

    
    private const string SCRIPTABLE_OBJECT_TEMPLATE_PATH = "Assets/Scripts/ScriptableObjects/Perks/_PerksDataTemplate.txt";
    private const string SCRIPTABLE_OBJECT_PATH = "Assets/Data/Resources/PerksData/";





    private string m_className = "NewPerk";
    private bool m_hasPhase = true;
    private float m_afterCompileTime = -1; 




    [MenuItem("Tools/Perk Generator")]
    public static void ShowWindow()
    {
        GetWindow<ToolPerkGenerator>("Perk Generator");
    }

    private void OnGUI()
    {
        m_className = EditorGUILayout.TextField("Perk Name", m_className);

        EditorGUILayout.Space();


        
        m_hasPhase = EditorGUILayout.ToggleLeft("Has Phase", m_hasPhase);

        EditorGUILayout.Space();

        if (GUILayout.Button("Generate"))
        {
            if (string.IsNullOrEmpty(m_className))
            {
                Debug.LogError("Perk name cannot be empty.");
                return;
            }

            if (!File.Exists(CLASS_TEMPLATE_PATH))
            {
                Debug.LogError($"Template file not found at path: {CLASS_TEMPLATE_PATH}");
                return;
            }

            GenerateFiles(m_className);
        }
    }

    private void GenerateFiles(string dataClassName)
    {

        CreatePerkActionScript(dataClassName);
        
        CreatePerkDataScript(dataClassName);
       

        CompilationPipeline.compilationFinished += CompilationFinished;
        AssetDatabase.Refresh();
    }

    private void CreatePerkActionScript(string dataClassName)
    {
        string templateContent, classContent, classFilePath;

        string actionName = dataClassName + "Perk";
        Directory.CreateDirectory(CLASS_PATH);
        
        templateContent = File.ReadAllText(CLASS_TEMPLATE_PATH);
        classContent = templateContent.Replace("_BasePerkData", dataClassName + "PerkData");
        
                
        classContent = classContent.Replace("_PerkTemplate", actionName);
                
        if (m_hasPhase)
        {
            classContent = classContent.Replace("*PHASEL1*", "EGamePhase[] phases = new EGamePhase[] { EGamePhase.PLAYER_TURN_END};");
            classContent = classContent.Replace("*PHASEL2*", "return phases;");
        }
        else
        {
            classContent = classContent.Replace("*PHASEL1*", "return null;");
            classContent = classContent.Replace("*PHASEL2*", "");
        }
        
        classFilePath = Path.Combine(CLASS_PATH, $"{actionName}.cs");
        File.WriteAllText(classFilePath, classContent);
    }

    private void CreatePerkDataScript(string dataClassName)
    {
        dataClassName += "PerkData";
        string templateContent, classFilePath, classContent;
        
        templateContent = File.ReadAllText(SCRIPTABLE_OBJECT_TEMPLATE_PATH);
        
        classContent = templateContent.Replace("_PerksDataTemplate", dataClassName);
        
        classFilePath = Path.Combine(CLASS_PATH, $"{dataClassName}.cs");
        File.WriteAllText(classFilePath, classContent);
    }

    private void CreateScriptableObjectAsset(string className)
    {
        
        ScriptableObject asset = ScriptableObject.CreateInstance(className + "PerkData");
        if (asset == null)
        {
            Debug.LogError($"Failed to create an instance of the class: {className}. Is it a ScriptableObject?");
            return;
        }

        Directory.CreateDirectory(SCRIPTABLE_OBJECT_PATH);
        string assetPath = Path.Combine(SCRIPTABLE_OBJECT_PATH, $"{className}.asset");
        AssetDatabase.CreateAsset(asset, assetPath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        
        //add to perksDB
        PerksDb.PerksInfo perkInfo = PerksDb.Instance.FindById(className);
        if (perkInfo != null)
        {
            perkInfo.PerkData = asset as BasePerkData;
            perkInfo.IsImplemented = true;
        }
        else
        {
            Debug.Log("did not find perk in perkDB. creating perk");
            // create perk
            PerksDb.Instance.CreatePerk(className, null, string.Empty, asset as BasePerkData);
        }
        EditorUtility.SetDirty(PerksDb.Instance);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }


    private void CompilationFinished(object context = null)
    {
        m_afterCompileTime = Time.realtimeSinceStartup + 0.1f;
    }


    protected virtual void OnEnable()
    {
        EditorApplication.update += OnEditorUpdate;
    }

    protected virtual void OnDisable()
    {
        EditorApplication.update -= OnEditorUpdate;
    }

    protected virtual void OnEditorUpdate()
    {
        if (m_afterCompileTime  > 0 && m_afterCompileTime  < Time.realtimeSinceStartup )
        {
            CreateScriptableObjectAsset(m_className);
            m_afterCompileTime = -1;
        }
    }
}

#endif