using System;
using System.Collections.Generic;
using Game;
using UnityEngine;

public class SiegeMasterPerk : BasePerk
{

    private SiegeMasterPerkData m_perkData;
    
    public override void Config(BasePerkData perkData)
    {
        m_perkData = (SiegeMasterPerkData)perkData;
    }

    public override void OnAdd()
    {
        UpgradeTargetInventFinishers();
    }

    public override void OnRemove(){}
    
    private void OnDestroy(){}

    public override EGamePhase[] GetPhases()
    {
        EGamePhase[] phases = new EGamePhase[] { EGamePhase.CARD_DRAW_FINISHED};
        return phases;
    }

    public override float GetPriority()
    {
        return 6;
    }

    public override void OnPhaseActivate(EGamePhase phase, Action callback)
    {
        UpgradeTargetInventFinishers();
    }

    private bool m_hasUpgraded = false;
    private void UpgradeTargetInventFinishers()
    {
        if (m_hasUpgraded) return;
        m_hasUpgraded = true;

        List<InventFinisher> finishers = GameInfoHelper.GetAllInventFinishersOfPack(InventFinisherPack.MACHINE);

        var modifier = new AddValueModifier<int>(m_perkData.InventLevelIncrease);
        foreach (var finisher in finishers)
        {
            finisher.ModifyInventLevel(modifier);
        }
    }

}
