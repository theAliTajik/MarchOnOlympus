using System;
using Game;
using UnityEngine;

public class HedonistPerk : BasePerk
{

    private HedonistPerkData m_perkData;
    
    public override void Config(BasePerkData perkData)
    {
        m_perkData = (HedonistPerkData)perkData;
    }

    public override void OnAdd()
    {
        GameActionHelper.SetCardToBePlayedTwice(m_perkData.CardToBePlayedTwiceIndex);
    }

    public override void OnRemove()
    {
        GameActionHelper.RemoveCardsPlayTwice(m_perkData.CardToBePlayedTwiceIndex);
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
}
