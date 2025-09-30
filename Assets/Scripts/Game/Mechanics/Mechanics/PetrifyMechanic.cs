
using System.Collections;
using Game;
using UnityEngine;

public class PetrifyMechanic : BaseMechanic
{
    private const int m_triggerThreshold = 10;
    
    public PetrifyMechanic(int stack, IHaveMechanics mOwner, int guardMin = 0)
    {
        m_stack.SetValue(stack);
        m_mechanicOwner = mOwner;
        this.OnChange += CheckThreshold;
        
        m_stack.SetGuard(guardMin);
        
    }

    private void CheckThreshold(MechanicType obj = MechanicType.PETRIFY)
    {
        Debug.Log($"Checked threshold stack: {m_stack.Amount}, threshold: {m_triggerThreshold}");
        if (m_stack >= m_triggerThreshold)
        {
            StunPlayer();
            ReduceStack(m_triggerThreshold);
        } 
    }

    private void StunPlayer()
    {
        Debug.Log("petrify stunned player");
        CombatManager.Instance.StunPlayer();
    }

    public override MechanicType GetMechanicType()
    {
        return MechanicType.PETRIFY;
    }

    public override bool TryReduceStack(CombatPhase phase, bool isMyTurn, bool isFirstTimeInTurn = false)
    {
        return false;
    }
    
    public override void Apply(Fighter.DamageContext context)
    {
        
    }
}
