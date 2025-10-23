using System;
using System.Collections.Generic;
using UnityEngine;

public class InventModel : MonoBehaviour
{
    public event Action<int> OnInventChanged;
    public event Action<int> OnInventLevelChanged;

    private int m_invent;
    private int m_inventLevel = 0;

    private List<InventFinisher> m_inventFinishers;

    private const int m_inventToLevelConversionRate = 20;
    private const string m_jsonSavePath = "Invent/invents.txt";

    private void Awake()
    {
        GameplayEvents.OnGainInvent += GainInvent;
    }

    private void OnDestroy()
    {
        GameplayEvents.OnGainInvent -= GainInvent;
    }

    public void GainInvent(int invent)
    {
        if (invent <= 0)
        {
            CustomDebug.LogWarning($"Invalid invent on InventGain. invent: {invent}", Categories.Combat.Invent.Root);
            return;
        }

        this.m_invent += invent;
        InventChanged();
    }

    public bool UseInvent(int invent, bool useLevel = true)
    {
        bool hasEnough = HasEnoughInvent(invent, useLevel);
        if (!hasEnough) return false;

        if (useLevel)
        {
            invent = ConvertLevelToInvent(invent);
        }

        this.m_invent -= invent;
        InventChanged();
        return true;
    }

    public void SetFinishers(List<InventFinisher> finishers)
    {
        this.m_inventFinishers = finishers;
    }

    private void AddFinisher(InventFinisher inventFinisher)
    {
        if (this.m_inventFinishers == null) m_inventFinishers = new();
        
        m_inventFinishers.Add(inventFinisher);
    }

    private InventFinisher m_executingFinisher;
    public void ExecuteFinisher(string id, IDamageable target)
    {
        m_executingFinisher = GetFinisher(id);
        m_executingFinisher?.Execute(target, finishCallback:OnFinisherExecuted);
    }

    private void OnFinisherExecuted()
    {
        if (m_executingFinisher != null)
        {
            GameplayEvents.SendOnInventPlayed(m_executingFinisher, m_inventLevel);
        }
        
        UseInvent(m_inventLevel);
    }

    public InventFinisher GetFinisher(string id)
    {
        if (m_inventFinishers == null)
        {
            CustomDebug.LogError($"Null finishers in model when asked for: {id} finisher", Categories.Combat.Invent.Root);
            return null;
        }
        
        InventFinisher finisher = m_inventFinishers.Find(f => f.ID == id);
        
        if (finisher == null)
        {
            CustomDebug.LogError($"Did not find finisher: {id}. inside of model", Categories.Combat.Invent.Root);
            return null;
        }
        return finisher;
    }

    public TargetType GetFinisherTargetType(string finisherId)
    {
        var finisher = GetFinisher(finisherId);
        return GetFinisherTargetType(finisher);
    }

    public TargetType GetFinisherTargetType(InventFinisher inventFinisher)
    {
        return inventFinisher.GetTargetType();
    }

    public bool HasEnoughInvent(int invent, bool useLevel = true)
    {
        if (invent < 0)
        {
            CustomDebug.LogWarning($"Invalid invent on HasEnoughInvent: {invent}" ,Categories.Combat.Invent.Root);
            return false;
        }

        if (useLevel)
        {
            invent = ConvertLevelToInvent(invent);
        }

        if (invent > this.m_invent) return false;

        return true;
    }

    private int ConvertInventToLevel(int invent)
    {
        int level = invent / m_inventToLevelConversionRate;


        return level;
    }

    private int ConvertLevelToInvent(int level)
    {
        int invent = level * m_inventToLevelConversionRate;
        return invent;
    }

    private void InventChanged()
    {
        UpdateInventLevel();
        OnInventChanged?.Invoke(this.m_invent);
    }

    private void UpdateInventLevel()
    {
        int inventLevel = ConvertInventToLevel(m_invent);

        if (inventLevel != this.m_inventLevel)
        {
            this.m_inventLevel = inventLevel;
            InventLevelChanged();
        }
    }

    private void InventLevelChanged()
    {
        foreach (var finisher in m_inventFinishers)
        {
            finisher.OnInventLevelChanged(m_inventLevel);
        }
        OnInventLevelChanged?.Invoke(this.m_inventLevel);
    }


    public void LoadFinishers()
    {
        if (!GameProgress.Instance)
        {
            CustomDebug.LogWarning("Could not Load finishers. game progress was null",  Categories.Combat.Invent.Root);
            return;
        }

        List<string> finishersId = GameProgress.Instance.Data.InventFinisherIds;
        foreach (var id in finishersId)
        {
            var finisher = InventsDb.Instance.FindById(id);
            
            if (finisher == null)
            {
                CustomDebug.LogWarning("Did not find finisher: {id}. when loading from json", Categories.Combat.Invent.Root);
                continue;
            }

            CustomDebug.Log($"Loaded finisher: {finishersId}", Categories.Combat.Invent.SaveAndLoad);
            AddFinisher(finisher);
        }
    }

    public List<InventFinisher> GetAllLoadedFinishers()
    {
        return m_inventFinishers;
    }
}