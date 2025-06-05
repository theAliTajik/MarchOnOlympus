using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;

public class HealthyPerk : BasePerk
{
    public override void OnAdd()
    {
        //change player max hp + 10
        // Debug.Log("change player max hp + 10");
        GameActionHelper.IncreasePlayerMaxHP(10);
    }

    public override void OnRemove()
    {
        // revert max hp increase
        Debug.Log("revert player max hp");
        GameActionHelper.DecreasePlayerMaxHP(10);
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
