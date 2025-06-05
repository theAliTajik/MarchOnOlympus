
using System;
using System.IO;
using UnityEditor;
using UnityEngine;

public class ToolPerkDataGenerator : EditorWindow
{
    
    private const string SCRIPTABLE_OBJECT_PATH = "Assets/Data/Resources/PerksData/";

    private string m_scriptableObjectName;
    private string m_perkActionName;
    
    [MenuItem("Tools/Perk Data Generator")]
    public static void ShowWindow()
    {
        GetWindow<ToolPerkDataGenerator>("Perk Data Generator");
    }

    private void OnGUI()
    {
        m_scriptableObjectName = EditorGUILayout.TextField("Data Name", m_scriptableObjectName);
        
        EditorGUILayout.Space();
        
        m_perkActionName = EditorGUILayout.TextField("Action Name", m_perkActionName);
        
        EditorGUILayout.Space();
        if (GUILayout.Button("Generate"))
        {
            if (string.IsNullOrEmpty(m_scriptableObjectName))
            {
                Debug.LogError("scriptable object name cannot be empty.");
                return;
            }
                    
            if (!string.IsNullOrEmpty(m_perkActionName))
            {
                Type type = Type.GetType(m_perkActionName + "Perk"); 
                if (type == null)
                {
                    Debug.LogError($"Perk action class {m_perkActionName + "Perk"} cannot be found. make sure it exists.");
                    return;
                }
            }

            CreateScriptableObjectAsset(m_scriptableObjectName);
        }
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
        for (int i = 1; i < 50; i++)
        {
            PerksDb.PerksInfo perkInfo = PerksDb.Instance.FindById(className);
            if (perkInfo != null)
            {
                //Remove previous suffix from class name
                className.Replace((i-1).ToString(), "");
                //add new suffix
                className += i.ToString(); 
            }
            else
            {
                Debug.Log("did not find perk in perkDB. creating perk");
                // create perk
                PerksDb.Instance.CreatePerk(className, null, string.Empty, asset as BasePerkData, m_perkActionName + "Perk");
            }
        }
        EditorUtility.SetDirty(PerksDb.Instance);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}
