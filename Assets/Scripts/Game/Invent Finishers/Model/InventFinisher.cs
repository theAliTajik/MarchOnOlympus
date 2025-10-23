
using System;
using System.Collections.Generic;
using Game.ModifiableParam;
using Mono.CSharp;
using UnityEngine;

[CreateAssetMenu(fileName = "InventFinisher", menuName = "Invent/Finisher")]
[System.Serializable]
public class InventFinisher : ScriptableObject
{
    public string ID;
    public InventFinisherPack Pack;
    public List<InventAction> InventActions;

    private ModifiableParam<int> m_inventLevel = new();
    private bool m_clampHasBienApplied;
    

    public void Execute(IDamageable target, Action finishCallback)
    {
        InventAction action = GetInventAction();
        action?.Execute(target, finishCallback);
    }

    public InventAction GetInventAction()
    {
        if (m_inventLevel == 0) return null;
        
        if (InventActions == null || InventActions.Count == 0)
        {
            CustomDebug.LogError("Finisher does not have invent action or any actions", Categories.Combat.Invent.Root);
            return null;
        }
        
        InventAction action = InventActions.Find(a => a.InventLevel == m_inventLevel.Value);

        if (action == null)
        {
            CustomDebug.LogError("Invent action with invent level " + m_inventLevel.Value + " not found", Categories.Combat.Invent.Root);
            return null;
        }
        
        return action;
    }

    public TargetType GetTargetType()
    {
        var action = GetInventAction();
        return action.TargetType;
    }

    public void OnInventLevelChanged(int inventLevel)
    {
        ApplyClamp();
        m_inventLevel = inventLevel;
    }

    public void ModifyInventLevel(IParamModifier<int> modifier)
    {
        ApplyClamp();
        m_inventLevel.AddModifier(modifier);
    }

    private void ApplyClamp()
    {
        if (m_clampHasBienApplied) return;

        var clampModifier = new ClampValueModifier<int>(min:0);
        m_inventLevel.AddModifier(clampModifier);
    }
}
