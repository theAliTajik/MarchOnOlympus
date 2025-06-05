using System;
using System.Collections;
using System.Collections.Generic;
using Game.ModifiableParam;
using Newtonsoft.Json;
using UnityEngine;

public class 
    GameProgress : Singleton<GameProgress>
{
    public Action OnDataChanged;
    
    private const string m_dataPath = "/Progress/Data.txt";
    private string m_savePath;
    
    private GameProgressData m_data;
    
    public GameProgressData Data { get { return m_data; } }


    
    protected override void Init()
    {
        m_savePath = Application.persistentDataPath + m_dataPath;
        Load();
        m_data.OnDataChanged += OnDataChange;
        DontDestroyOnLoad(this);
    }


    private void OnDataChange()
    {
        Debug.Log("game progress realized data changed");
        OnDataChanged?.Invoke();
        Save();
    }

    public void Save()
    {
        JsonHelper.SaveAdvanced(m_data, m_savePath);
    }

    private void Load()
    {
        GameProgressData data = JsonHelper.LoadAdvanced<GameProgressData>(m_savePath);
        
        if (data != null)
        {
            m_data = data;
            // Debug.Log("loaded data. honor: " + data.Honor);
        }
        else
        {
            Debug.Log("could not load game progress Data");
            m_data = new GameProgressData();
        }
    }
    
    
}


public class GameProgressData
{
    [JsonIgnore]
    public Action OnDataChanged;
        
    private int m_honor;
    private List<string> m_perkIds = new List<string>();
    private List<IParamModifier<int>> m_shopModifiers = new List<IParamModifier<int>>();

    public int Honor
    {
        get { return m_honor; }
        set
        {
            m_honor = value;
            DataChanged();
        }
    }
    
    public List<string> PerkIds
    {
        get { return m_perkIds; }
        set
        {
            m_perkIds = value;
            DataChanged();
        }
    }

    public List<IParamModifier<int>> ShopModifiers
    {
        get { return m_shopModifiers; }
        set
        {
            m_shopModifiers = value;
            DataChanged();
        }
    }
    
    private void DataChanged()
    {
        // Debug.Log("data called on data changed");
        OnDataChanged?.Invoke();
    }
    
}
