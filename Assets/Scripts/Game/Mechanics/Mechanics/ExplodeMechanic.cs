using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;

public class ExplodeMechanic : BaseMechanic
{
    [SerializeField] private int m_waitSecondsBeforeExplode = 2;
    [SerializeField] private int m_damage = 10;
    
    public ExplodeMechanic(int stack, IHaveMechanics mOwner)
    {
        m_stack.SetValue(stack);
        m_mechanicOwner = mOwner;
        CombatManager.Instance.StartCoroutine(Explode());
    }

    public override MechanicType GetMechanicType()
    {
        return MechanicType.EXPLODE;
    }

    private IEnumerator Explode()
    {
        yield return new WaitForSeconds(m_waitSecondsBeforeExplode);
        if (m_mechanicOwner is IDamageable damageable)
        {
            damageable.TakeDamage(m_damage, null, false);
        }
        ReduceStack(10000);
    }
    
    public override bool TryReduceStack(CombatPhase phase, bool isMyTurn, bool isFirstTimeInTurn = false)
    {
        return false;
    }
    
    public override void Apply(Fighter.DamageContext context)
    {
        
    }
}