#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;
using System.IO;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor.Compilation;

public class ToolCardGenerator : EditorWindow
{
    private const string CLASS_TEMPLATE_PATH = "Assets/Scripts/ScriptableObjects/Cards/CardsData/_CardTemplate.txt";
    private const string ACTION_TEMPLATE_PATH = "Assets/Scripts/ScriptableObjects/Cards/CardsActions/_CardActionTemplate.txt";

    private const string CLASS_PATH = "Assets/Scripts/ScriptableObjects/Cards/";
    private const string ACTION_CLASS_PATH = "Assets/Scripts/ScriptableObjects/CardsActions/";

    
    private const string SCRIPTABLE_OBJECT_PATH = "Assets/Data/Resources/CardsData/";





    private string m_className = "NewCard";
    private bool m_formatNormal = true;
    private bool m_formatStance = true;
    private float m_afterCompileTime = -1; 




    [MenuItem("Tools/Card Generator")]
    public static void ShowWindow()
    {
        GetWindow<ToolCardGenerator>("Card Generator");
    }

    private void OnGUI()
    {
        m_className = EditorGUILayout.TextField("Card Name", m_className);

        EditorGUILayout.Space();

        m_formatStance = EditorGUILayout.ToggleLeft("Stance Formatting", m_formatStance);
        m_formatNormal = EditorGUILayout.ToggleLeft("Normal Formatting", m_formatNormal);

        EditorGUILayout.Space();

        if (GUILayout.Button("Generate"))
        {
            if (string.IsNullOrEmpty(m_className))
            {
                Debug.LogError("Card name cannot be empty.");
                return;
            }

            if (!File.Exists(CLASS_TEMPLATE_PATH))
            {
                Debug.LogError($"Template file not found at path: {CLASS_TEMPLATE_PATH}");
                return;
            }

            if (!File.Exists(ACTION_TEMPLATE_PATH))
            {
                Debug.LogError($"Template file not found at path: {ACTION_TEMPLATE_PATH}");
                return;
            }

            GenerateFiles(m_className);
        }
    }

    private void GenerateFiles(string className)
    {
        string templateContent, classContent, classFilePath;

        Directory.CreateDirectory(CLASS_PATH);
        Directory.CreateDirectory(ACTION_CLASS_PATH);

        templateContent = File.ReadAllText(CLASS_TEMPLATE_PATH);
        classContent = templateContent.Replace("_CardName", className);

        className += "Card";
        string classNameSeperatedByCapitals = Regex.Replace(className, "(?<!^)([A-Z])", " $1");

        classContent = classContent.Replace("_CardMenuName", className);
        classContent = classContent.Replace("_CardTemplate", className);

        
        if (m_formatStance)
        {
            classContent = classContent.Replace("*STANCE*", "return string.Format(stanceDataSet.description, name);");
        }
        else
        {
            classContent = classContent.Replace("*STANCE*", "return stanceDataSet.description;");
        }

        if (m_formatNormal)
        {
            classContent = classContent.Replace("*NORMAL*", "return string.Format(normalDataSet.description, name);");
        }
        else
        {
            classContent = classContent.Replace("*NORMAL*", "return normalDataSet.description;");
        }


        classFilePath = Path.Combine(CLASS_PATH, $"{className}.cs");
        File.WriteAllText(classFilePath, classContent);


        string newClassName = className + "Action";

        templateContent = File.ReadAllText(ACTION_TEMPLATE_PATH);
        classContent = templateContent.Replace("_CardActionTemplate", newClassName);
        classContent = classContent.Replace("_CardClassName", className);
        classFilePath = Path.Combine(ACTION_CLASS_PATH, $"{newClassName}.cs");
        File.WriteAllText(classFilePath, classContent);


        CompilationPipeline.compilationFinished += CompilationFinished;
        AssetDatabase.Refresh();
    }


    private void CreateScriptableObjectAsset(string className)
    {
        
        ScriptableObject asset = ScriptableObject.CreateInstance(className + "Card");
        if (asset == null)
        {
            Debug.LogError($"Failed to create an instance of the class: {className}. Is it a ScriptableObject?");
            return;
        }

        BaseCardData cardData = asset as BaseCardData;
        cardData.SetName(className);
        Directory.CreateDirectory(SCRIPTABLE_OBJECT_PATH);
        string assetPath = Path.Combine(SCRIPTABLE_OBJECT_PATH, $"{className}.asset");
        AssetDatabase.CreateAsset(asset, assetPath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        
        //add to dev testing deck template
        DeckTemplates.Deck devtemplate = DeckTemplatesDb.Instance.FindById("Dev Testing");
        if (devtemplate != null)
        {
            devtemplate.CardsInDeck.RemoveAt(0);
            CardInDeckStateMachine cardInDeck = new CardInDeckStateMachine();
            cardInDeck.Configure(cardData);
            devtemplate.CardsInDeck.Insert(0, cardInDeck);
            EditorUtility.SetDirty(DeckTemplatesDb.Instance);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        
        //add to all cards Db
        CardsDb.Instance.CreateCard(cardData);
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