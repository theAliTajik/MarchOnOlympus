using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;

public class StanceDefenciveState : StanceBaseState
{
    public StanceDefenciveState(StanceStateMachine context)
    {
        m_context = context;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        GameActionHelper.AddMechanicToPlayer(3, MechanicType.DEXTERITY);
        GameActionHelper.AddMechanicToPlayer(2, MechanicType.THORNS);
    }

    public override Stance GetStance()
    {
        return Stance.DEFENCIVE;
    }
}
