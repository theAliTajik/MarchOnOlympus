using System;
using Game;
using UnityEngine;

public class ImprovisedHealingPerk : BasePerk
{

    private ImprovisedHealingPerkData m_perkData;
    
    public override void Config(BasePerkData perkData)
    {
        m_perkData = (ImprovisedHealingPerkData)perkData;
    }

    public override void OnAdd()
    {
        GameplayEvents.OnCardDrawnFromImprovise += OnCardDraw;
    }

    public override void OnRemove()
    {
        GameplayEvents.OnCardDrawnFromImprovise -= OnCardDraw;
    }

    private void OnDestroy()
    {
        GameplayEvents.OnCardDrawnFromImprovise -= OnCardDraw;
    }

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
    
    private void OnCardDraw()
    {
        GameActionHelper.GainInvent(m_perkData.Invent);
    }

}
