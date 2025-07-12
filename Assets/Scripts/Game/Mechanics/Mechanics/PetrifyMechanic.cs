
using Game;
using UnityEngine;

public class PetrifyMechanic : BaseMechanic
{
    private const int m_triggerThreshold = 10;

    public PetrifyMechanic(int stack, IHaveMechanics mOwner, bool hasGuard = false, int guardMin = 0)
    {
        m_stack.SetValue(stack);
        m_mechanicOwner = mOwner;
        this.OnChange += CheckThreshold;
    }

    private void CheckThreshold(MechanicType obj)
    {
        if (m_stack >= m_triggerThreshold)
        {
            StunPlayer();
            ReduceStack(m_triggerThreshold);
        } 
    }

    private void StunPlayer()
    {
        Debug.Log("petrify stunned player");
    }

    public override MechanicType GetMechanicType()
    {
        return MechanicType.PETRIFY;
    }

    public override bool TryReduceStack(CombatPhase phase, bool isMyTurn)
    {
        return false;
    }
    
    public override void Apply(Fighter.DamageContext context)
    {
        
    }
}
