using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;

public class FortunePerk : BasePerk
{

    public int damage = 3;

    public override void OnAdd(){}
    
    public override void OnRemove(){}

    public override EGamePhase[] GetPhases()
    {
        EGamePhase[] phases = new EGamePhase[] { EGamePhase.CARD_DRAW_FINISHED};
        return phases;
    }

    public override float GetPriority()
    {
        return 1;
    }

    public void OnExtraCardDraw()
    {
        Fighter randEnemy = GameInfoHelper.GetRandomEnemy();
        GameActionHelper.DamageFighter(randEnemy, GameInfoHelper.GetPlayer(), damage);
    }

    public override void OnPhaseActivate(EGamePhase phase, Action callback)
    {
        switch (phase)
        {
            case EGamePhase.CARD_DRAW_FINISHED:
                Debug.Log("Ask about fortune perk exact workings");
                break;
        }
    }
}
