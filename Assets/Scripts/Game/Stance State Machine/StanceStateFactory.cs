
using System.Collections.Generic;

public class StanceStateFactory
{
    private Dictionary<Stance, StanceBaseState> m_states = new Dictionary<Stance, StanceBaseState>();

    public StanceStateFactory(StanceStateMachine context)
    {
        m_states.Add(Stance.NONE, new StanceNoneState(context));
        m_states.Add(Stance.BATTLE, new StanceBattleState(context));
        m_states.Add(Stance.DEFENCIVE, new StanceDefenciveState(context));
        m_states.Add(Stance.BERSERKER, new StanceBerserkerState(context));
    }

    public StanceBaseState GetState(Stance stance)
    {
        return m_states[stance];
    }
}
