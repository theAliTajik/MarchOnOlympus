using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StanceNoneState : StanceBaseState
{
    public StanceNoneState(StanceStateMachine context)
    {
        m_context = context;
    }

    public override void OnEnter()
    {
        m_context.SetInfiniteCd(true);
    }

    public override Stance GetStance()
    {
        return Stance.NONE;
    }

    public override void OnTurnStart()
    {
        
    }

    public override void OnExit()
    {
        m_context.SetInfiniteCd(false);
    }
}
