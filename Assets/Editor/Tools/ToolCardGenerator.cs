#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;
using System.IO;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor.Compilation;
using UnityEditor.Localization.Plugins.XLIFF.V20;

public class ToolCardGenerator : EditorWindow
{
    private const string CLASS_TEMPLATE_PATH = "Assets/Scripts/ScriptableObjects/Cards/CardsData/_CardTemplate.txt";
    private const string ACTION_TEMPLATE_PATH = "Assets/Scripts/ScriptableObjects/Cards/CardsActions/_CardActionTemplate.txt";

    private const string CLASS_PATH = "Assets/Scripts/ScriptableObjects/Cards/CardsData/";
    private const string ACTION_CLASS_PATH = "Assets/Scripts/ScriptableObjects/Cards/CardsActions/";

    
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

            string className = FormatClassName(m_className);
            if (File.Exists(GenerateClassPath(className)))
            {
                Debug.LogError("Class already exists.");
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

            CreateCard(m_className);
        }
    }

    private void CreateCard(string className)
    {
        GenerateFiles(className);
    }

    private void GenerateFiles(string className)
    {
        string  classContent, classFilePath;

        Directory.CreateDirectory(CLASS_PATH);
        Directory.CreateDirectory(ACTION_CLASS_PATH);

        className = FormatClassName(className);
        classContent = CreateClassContent(className);

        classFilePath = GenerateClassPath(className);
        File.WriteAllText(classFilePath, classContent);

        string actionClassName = FormatActionClassName(className);
        string ActionClassContent = CreateActionClassContent(actionClassName, className);

        classFilePath = Path.Combine(ACTION_CLASS_PATH, $"{actionClassName}.cs");
        File.WriteAllText(classFilePath, ActionClassContent);


        CompilationPipeline.compilationFinished += CompilationFinished;
        AssetDatabase.Refresh();
    }

    private string FormatClassName(string className)
    {
        if (className.Contains("Card"))
        {
            return className;
        }

        return className + "Card";
    }

    private string FormatActionClassName(string actionClassName)
    {
        actionClassName = FormatClassName(actionClassName);

        if (actionClassName.Contains("Action"))
        {
            return actionClassName;
        }
        
        return actionClassName + "Action";
    }

    private string GenerateClassPath(string className)
    {
        return Path.Combine(CLASS_PATH, $"{className}.cs");
    }

    private string CreateClassContent(string className)
    {
        string templateContent, classContent;
        
        templateContent = File.ReadAllText(CLASS_TEMPLATE_PATH);
        classContent = templateContent.Replace("_CardName", className.Replace("Card", ""));

        // string classNameSeperatedByCapitals = Regex.Replace(className, "(?<!^)([A-Z])", " $1");

        classContent = classContent.Replace("_CardMenuName", className);
        classContent = classContent.Replace("_CardTemplate", className);

        
        if (m_formatStance)
        {
            classContent = classContent.Replace("*STANCE*", "return string.Format(stanceDataSet.description, Damage);");
        }
        else
        {
            classContent = classContent.Replace("*STANCE*", "return stanceDataSet.description;");
        }

        if (m_formatNormal)
        {
            classContent = classContent.Replace("*NORMAL*", "return string.Format(normalDataSet.description, Damage);");
        }
        else
        {
            classContent = classContent.Replace("*NORMAL*", "return normalDataSet.description;");
        }


        return classContent;
    }
    
    private string CreateActionClassContent(string actionClassName, string className)
    {
        string templateContent, classContent;
        

        templateContent = File.ReadAllText(ACTION_TEMPLATE_PATH);
        classContent = templateContent.Replace("_CardActionTemplate", actionClassName);
        classContent = classContent.Replace("_CardClassName", className);
        
        return classContent;
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
        
        AddCardToDevTesting(cardData);
        
        //add to all cards Db
        AddCardToCardsDb(cardData);
    }

    private void AddCardToCardsDb(BaseCardData cardData)
    { 
        CardsDb.Instance.CreateCard(cardData);
    }

    private void AddCardToDevTesting(BaseCardData cardData)
    {
        //add to dev testing deck template
        DeckTemplates.Deck devtemplate = DeckTemplates.FindById("Dev Testing");
        if (devtemplate != null)
        {
            devtemplate.CardsInDeck.RemoveAt(0);
            CardInDeckStateMachine cardInDeck = new CardInDeckStateMachine();
            cardInDeck.Configure(cardData);
            devtemplate.CardsInDeck.Insert(0, cardInDeck);
            DeckTemplates.Save();
        }
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