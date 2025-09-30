
using UnityEngine;

public class CyclopsDamageBehaviour: StandardDamageBehaviour
{
    private IDamageable m_TargetedPart;

    public CyclopsDamageBehaviour()
    {
        
    }

    public CyclopsDamageBehaviour(IHaveMechanics owner)
    {
        m_mechanicsOwner = owner;
    }
    
    public void SetTargetedHead(IDamageable part)
    {
        m_TargetedPart = part;
    }
    
    public override Fighter.DamageContext TakeDamage(int damage, Fighter sender, bool doesReturnToSender,
        bool isArmorPiercing = false,
        Fighter.DamageContext damageContext = null)
    {
        Debug.Log("cyclops damaged");
        if (m_TargetedPart != null)
        {
            Debug.Log("target took it like a champ");
            return m_TargetedPart.TakeDamage(damage, sender, doesReturnToSender, isArmorPiercing);
        }

        Debug.Log("actually hit cyclops");
        return base.TakeDamage(damage, sender, doesReturnToSender, isArmorPiercing);
    }
}
