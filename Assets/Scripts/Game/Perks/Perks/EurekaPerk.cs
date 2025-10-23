using System;
using Game;
using UnityEngine;

public class EurekaPerk : BasePerk
{

    private EurekaPerkData m_perkData;
    
    public override void Config(BasePerkData perkData)
    {
        m_perkData = (EurekaPerkData)perkData;
    }

    public override void OnAdd()
    {
        UpgradeFinisher();
    }
    
    public override void OnRemove(){}
    
    private void OnDestroy(){}

    public override EGamePhase[] GetPhases()
    {
        return null;
        
    }

    public override float GetPriority()
    {
        return -1;
    }

    public override void OnPhaseActivate(EGamePhase phase, Action callback)
    {
        throw new NotImplementedException();
    }

    private void UpgradeFinisher()
    {
        GameInfoHelper.GetInventFinisherSelectionFromPlayer();
        GameplayEvents.OnInventFinisherSelected += OnFinisherSelected;
    }

    private void OnFinisherSelected(InventFinisher finisher)
    {
        var modifier = new AddValueModifier<int>(m_perkData.InventFinisherLevelUpgrade);
        finisher.ModifyInventLevel(modifier);
        RemoveSelf();
    }
}
