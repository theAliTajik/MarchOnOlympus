using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Game;
public class PerksSaveObject
{
    public List<string> Perks = new List<string>();
}

public class PerksManager : Singleton<PerksManager>
{
    public Action<string> OnPerkAdded;
    public Action<string> OnPerkRemoved;
    
    private string m_savePath = "/Progress/Perks.json";

    
    [SerializeField] private PerksDisplay m_perksDisplay;
    
    private List<BasePerk> m_perks = new List<BasePerk>();
    private List<string> m_perkIds = new List<string>();
    private Dictionary<EGamePhase, List<BasePerk>> m_phasePerks = new Dictionary<EGamePhase, List<BasePerk>>();
    
    private Action m_finishCallback;
    
    public List<string> PerkIds => m_perkIds;

    protected override void Init()
    {
        m_savePath = Application.persistentDataPath + m_savePath;
    }

    protected override void Awake()
    {
        base.Awake();
        //init all phases
        foreach (EGamePhase phase in Enum.GetValues(typeof(EGamePhase)))
        {
            m_phasePerks.Add(phase, new List<BasePerk>());
        }

        GameplayEvents.GamePhaseChanged += OnPhaseChange;
        GameplayEvents.PerkRemoved += RemovePerk;
        
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        GameplayEvents.GamePhaseChanged -= OnPhaseChange;
        GameplayEvents.PerkRemoved -= RemovePerk;
    }
    
    public void LoadPerks()
    {
        if (GameProgress.Instance != null)
        {
            List<string> perks = GameProgress.Instance.Data.PerkIds;
            foreach (string m_perkId in perks)
            {
                LoadPerk(m_perkId);
            }
            // m_perksDisplay.AddAllPerks(m_perkIds);
        }
        
    }



    public void OnPhaseChange(EGamePhase phase)
    {
        for (int j = 0; j < m_phasePerks[phase].Count; j++)
        {
            m_phasePerks[phase][j].OnPhaseActivate(phase, m_finishCallback);
        }
    }

    public void AddPerk(BasePerk perk)
    {
        
        string perkID = MakePerk(perk);

        m_perkIds.Add(perkID);
        OnPerkAdded?.Invoke(perkID);
        SavePerksToJson();
    }
    public void AddPerk(string PerkClientID)
    {
        BasePerk perksInstance = Instantiate(InstantiatePerk(PerkClientID), this.transform);
        
        AddPerk(perksInstance);
    }

    private string MakePerk(BasePerk perk)
    {
        string perkId = perk.GetType().FullName;
        perkId = perkId.Substring(0, perkId.Length - 4); // remove "Perk" suffix from ID
        BasePerkData data = PerksDb.Instance.FindById(perkId).PerkData;
        if (data != null)
        {
            perk.Config(data);
        }
        else
        {
            Debug.Log("could not get perk data");
        }
        EGamePhase[] phases = perk.GetPhases();
        if (phases == null)
        {
            m_perks.Add(perk);
        }
        else
        {
            foreach (EGamePhase phase in phases)
            {
                m_phasePerks[phase].Add(perk);
                SortPerksByPriority(m_phasePerks[phase]);
            }
        }

        perk.OnAdd();
        return perkId;
    }

    private void LoadPerk(string PerkClientID)
    {
        BasePerk perksInstance = Instantiate(InstantiatePerk(PerkClientID), this.transform);
        
        string perkID = MakePerk(perksInstance);
        
        m_perkIds.Add(perkID);
        OnPerkAdded?.Invoke(perkID);
        // Debug.Log("invoked perk added");
    }
    
    private BasePerk InstantiatePerk(string PerkClientID)
    {
        PerksDb.PerksInfo perkInfo = PerksDb.Instance.FindById(PerkClientID);
            
        Type actionType = GetActionTypeFromName(perkInfo.GetScriptName());
        if (actionType == null)
        {
            Debug.LogError("Failed to instantiate action: Action type is null for perk: " + PerkClientID + this);
            return null;
        }
        
        GameObject g = new GameObject(perkInfo.GetScriptName());

#if UNITY_EDITOR
        if (!typeof(BasePerk).IsAssignableFrom(actionType))
        {
            Debug.LogError("The type returned by GetActionType is not a BaseCardAction: " + actionType, this);
            return null;
        }
#endif

        return (BasePerk)g.AddComponent(actionType);
    }
    
    private Type GetActionTypeFromName(string typeName)
    {
        if (string.IsNullOrEmpty(typeName))
        {
            Debug.LogError("Type name is null or empty.");
            return null;
        }

        Type type = Type.GetType(typeName);
        if (type != null)
            return type;

        Debug.LogError($"Type '{typeName}' not found. Ensure it is correctly named and accessible.");
        return null;
    }

    public void SortPerksByPriority(List<BasePerk> perks)
    {
        perks.Sort((a, b) => a.GetPriority().CompareTo(b.GetPriority())); // Ascending order
    }

    public void RemovePerk(BasePerk perk, bool removeDisplay = true)
    {
        perk.OnRemove();
        EGamePhase[] phases = perk.GetPhases();
        if (phases != null)
        {
            foreach (EGamePhase phase in phases)
            {
                m_phasePerks[phase].Remove(perk);
            }
        }

        
        string perkID = perk.GetType().Name.Replace("Perk", "");
        Debug.Log("perk id to remove: " + perkID);
        m_perkIds.Remove(perkID);
        Debug.Log("removed: " + perkID + " List of perks: " + m_perkIds);
        
        if (removeDisplay)
        {
            OnPerkRemoved?.Invoke(perkID);
        }
        
        SavePerksToJson();
    }
    
    public void RemovePerk(string perkClientID)
    {
        BasePerk perkToRemove = FindPerkByClientID(perkClientID);
        RemovePerk(perkToRemove);
    }
    

    public void OnRemovePerkClicked(string perkClientID)
    {
        BasePerk perkToRemove = FindPerkByClientID(perkClientID);
        RemovePerk(perkToRemove, false);
    }
    
    private BasePerk FindPerkByClientID(string perkClientID)
    {
        string perkName = string.Empty;
        for (int i = 0; i < m_perks.Count; i++)
        {
            perkName = m_perks[i].GetType().Name;
            string trimmedName = perkName.EndsWith("Perk") ? perkName.Substring(0, perkName.Length - 4) : perkName;
            if (trimmedName == perkClientID)
            {
                return m_perks[i];
            }
        }

        foreach (var perkList in m_phasePerks)
        {
            for (int j = 0; j < perkList.Value.Count; j++)
            {
                perkName = perkList.Value[j].GetType().Name;
                string trimmedName = perkName.EndsWith("Perk") ? perkName.Substring(0, perkName.Length - 4) : perkName;
                if (trimmedName == perkClientID)
                {
                    return perkList.Value[j];
                }
            }
        }

        Debug.Log("did not find perk by client Id");
        return null;
    }

    public void RemoveAllPerks()
    {
        foreach (BasePerk bPerk in m_perks)
        {
            RemovePerk(bPerk);
        }
    }
    
    private void SavePerksToJson()
    {
        if (GameProgress.Instance == null)
        {
            Debug.Log("game progress data null");
            return;
        }

        GameProgress.Instance.Data.PerkIds = m_perkIds;
    }

    private void LogAllPerks()
    {
        Debug.Log("logging: " + m_perkIds.Count + " perks:");
        foreach (string perk in m_perkIds)
        {
            Debug.Log(perk);
        }
    }

}
