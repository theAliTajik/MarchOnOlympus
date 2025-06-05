using System;
using System.Collections.Generic;
using Game;
using UnityEngine;

public class ExplosivePowerPerk : BasePerk
{

    private ExplosivePowerPerkData m_perkData;
    
    public override void Config(BasePerkData perkData)
    {
        m_perkData = (ExplosivePowerPerkData)perkData;
    }

    public override void OnAdd(){}
    
    public override void OnRemove(){}
    
    private void OnDestroy(){}

    public override EGamePhase[] GetPhases()
    {
        EGamePhase[] phases = new EGamePhase[] { EGamePhase.CARD_DRAW_FINISHED};
        return phases;
    }

    public override float GetPriority()
    {
        return 5;
    }

    public override void OnPhaseActivate(EGamePhase phase, Action callback)
    {
        int currentTurn = GameInfoHelper.GetCurrentTurn();
        if (currentTurn == m_perkData.TurnTrigger)
        {
            List<Fighter> enemeies = GameInfoHelper.GetAllEnemies();
            if (enemeies != null && enemeies.Count > 0)
            {
                foreach (Fighter enemey in enemeies)
                {
                    GameActionHelper.DamageFighter(enemey, GameInfoHelper.GetPlayer(), m_perkData.Damage);
                }
            }
        }
    }
}
