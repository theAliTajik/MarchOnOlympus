using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;

public class StanceBattleState : StanceBaseState
{
    public StanceBattleState(StanceStateMachine context)
    {
        m_context = context;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        GameActionHelper.IncreaseDrawAmount(1);
    }

    public override void OnExit()
    {
        GameActionHelper.DecreaseDrawAmount(1);
    }

    public override Stance GetStance()
    {
        return Stance.BATTLE;
    }
}
