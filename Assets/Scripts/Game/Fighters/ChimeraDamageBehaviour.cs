
using System;

public class ChimeraDamageBehaviour : StandardDamageBehaviour
{
    private ChimeraHead m_TargetedHead;

    public ChimeraDamageBehaviour()
    {
        
    }

    public ChimeraDamageBehaviour(IHaveMechanics owner)
    {
        m_mechanicsOwner = owner;
    }

    public void SetTargetedHead(ChimeraHead head)
    {
        m_TargetedHead = head;
    }
    
    public override Fighter.DamageContext TakeDamage(int damage, Fighter sender, bool doesReturnToSender,
        bool isArmorPiercing = false,
        Fighter.DamageContext damageContext = null)
    {
        Fighter.DamageContext context = m_TargetedHead.TakeDamage(damage, sender, doesReturnToSender, isArmorPiercing);
        return base.TakeDamage(damage, sender, doesReturnToSender, isArmorPiercing, context);
    }
}
