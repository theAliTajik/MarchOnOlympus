
using UnityEngine;

public class HydraDamageBehaviour : StandardDamageBehaviour
{
    private HydraHead m_TargetedHead;

    public HydraDamageBehaviour()
    {
        
    }

    public HydraDamageBehaviour(IHaveMechanics owner)
    {
        m_mechanicsOwner = owner;
    }

    public void SetTargetedHead(HydraHead head)
    {
        m_TargetedHead = head;
    }
    
    public override Fighter.DamageContext TakeDamage(int damage, Fighter sender, bool doesReturnToSender,
        bool isArmorPiercing = false,
        Fighter.DamageContext damageContext = null)
    {
        if (!m_TargetedHead)
        {
            CustomDebug.Log($"Null Targeted head. sender: {sender.name}", Categories.Fighters.Enemies.HydraHead);
            return damageContext;
        }
        
        CustomDebug.Log($"Head: {m_TargetedHead.name}, Took: {damage} damage. From {sender?.name}", Categories.Fighters.Enemies.HydraHead);
        Fighter.DamageContext context = m_TargetedHead.TakeDamage(damage, sender, doesReturnToSender, isArmorPiercing, damageContext);
        CustomDebug.Log($"Finial Damage: {context.ModifiedDamage}", Categories.Fighters.Enemies.HydraHead);
        return context;
    }
}
