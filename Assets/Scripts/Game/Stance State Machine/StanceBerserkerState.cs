using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;

public class StanceBerserkerState : StanceBaseState
{
    public StanceBerserkerState(StanceStateMachine context)
    {
        m_context = context;
    }
    
    public override void OnEnter()
    {
        base.OnEnter();
        GameActionHelper.AddMechanicToPlayer(2, MechanicType.STRENGTH, true, 2);
    }

    public override void OnExit()
    {
        GameActionHelper.RemovePlayerMechanicGuard(MechanicType.STRENGTH);
        GameActionHelper.ReduceMechanicStack(GameInfoHelper.GetPlayer(), 2, MechanicType.STRENGTH);
    }

    public override void OnMechanicAdded()
    {
        if (GameInfoHelper.MechanicsData.MechanicsTarget == GameInfoHelper.GetPlayer())
        {
            if (GameInfoHelper.MechanicsData.AddedMechanic.GetMechanicType() == MechanicType.BLOCK)
            {
                GameActionHelper.RemoveMechanicFromPlayer(MechanicType.BLOCK);
            }
        }
    }

    public override Stance GetStance()
    {
        return Stance.BERSERKER;
    }
}
