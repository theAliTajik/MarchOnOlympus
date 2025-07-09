
using System;

public class ChimeraDamageBehaviour : StandardDamageBehaviour
{
    private ChimeraHead m_TargetedHead;

    public void SetTargetedHead(ChimeraHead head)
    {
        m_TargetedHead = head;
    }
    
    public override int TakeDamage(int damage, Fighter sender, bool doesReturnToSender, bool isArmorPiercing = false)
    {
        damage = m_TargetedHead.TakeDamage(damage, sender, doesReturnToSender, isArmorPiercing);
        return base.TakeDamage(damage, sender, doesReturnToSender, isArmorPiercing);
    }
}
