
using UnityEngine;

public class ScyllaDamageBehaviour : StandardDamageBehaviour
{
    private ScyllaTentacle m_TargetedHead;

    public ScyllaDamageBehaviour()
    {
        
    }


    public ScyllaDamageBehaviour(IHaveMechanics owner)
    {
        m_mechanicsOwner = owner;
    }

    public void SetTargetedTentacle(ScyllaTentacle head)
    {
        m_TargetedHead = head;
    }
    
    public override Fighter.DamageContext TakeDamage(int damage, Fighter sender, bool doesReturnToSender,
        bool isArmorPiercing = false,
        Fighter.DamageContext damageContext = null)
    {
        if (m_TargetedHead == null)
        {
            return base.TakeDamage(damage, sender, doesReturnToSender, isArmorPiercing, damageContext);
        }
        
        Debug.Log($"damaged head: {m_TargetedHead.name}");
        Fighter.DamageContext context = m_TargetedHead.TakeDamage(damage, sender, doesReturnToSender, isArmorPiercing, damageContext);
        return context;
    }
}
