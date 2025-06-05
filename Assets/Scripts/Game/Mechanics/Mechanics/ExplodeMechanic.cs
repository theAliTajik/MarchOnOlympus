using System.Collections;
using System.Collections.Generic;
using Game;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

public class ExplodeMechanic : BaseMechanic
{
    [SerializeField] private int m_waitSecondsBeforeExplode = 2;
    [SerializeField] private int m_damage = 10;
    
    public ExplodeMechanic()
    {
        Explode();
    }

    public ExplodeMechanic(int stack, Fighter fighter)
    {
        m_stack = stack;    
        m_fighter = fighter;
        CombatManager.Instance.StartCoroutine(Explode());
    }

    public override MechanicType GetMechanicType()
    {
        return MechanicType.EXPLODE;
    }

    private IEnumerator Explode()
    {
        yield return new WaitForSeconds(m_waitSecondsBeforeExplode);
        m_fighter.TakeDamage(m_damage, m_fighter, false);
        ReduceStack(10000);
    }
    
    public override bool TryReduceStack(CombatPhase phase, bool isMyTurn)
    {
        return false;
    }
    
    public override void Apply(Fighter.DamageContext context)
    {
        
    }
}