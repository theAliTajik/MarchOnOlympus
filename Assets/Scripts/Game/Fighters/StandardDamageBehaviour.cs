
using System;
using UnityEngine;

public class StandardDamageBehaviour : IDamageable
{
    public event Action<int> OnDamage;

    protected IHaveMechanics m_mechanicsOwner;
    protected MechanicsList m_mechanicsList;

    #region ctors

    public StandardDamageBehaviour()
    {
        
    }
    
    public StandardDamageBehaviour(IHaveMechanics owner)
    {
        m_mechanicsOwner = owner;
    }

    public StandardDamageBehaviour(MechanicsList mechanicsList)
    {
        m_mechanicsList = mechanicsList;
    }

    #endregion
    
    
    public virtual Fighter.DamageContext TakeDamage(int damage, Fighter sender, bool doesReturnToSender,
        bool isArmorPiercing = false,
        Fighter.DamageContext damageContext = null)
    {
        if (damage < 0)
        {
            damage = 0;
        }

        if (m_mechanicsList == null && m_mechanicsOwner != null)
        {
            // Debug.Log("getting mechanic list");
            m_mechanicsList = MechanicsManager.Instance.GetMechanicsList(m_mechanicsOwner);
        }


        // Debug.Log($"mechanic list is null: {m_mechanicsList == null}");
        Fighter.DamageContext context = new Fighter.DamageContext();
        context.ModifiedDamage = damage;
        if (m_mechanicsList != null)
        {
            context.OriginalDamage = damage;
            context.Sender = sender;
            context.Target = m_mechanicsOwner;
            context.DoesReturnToSender = doesReturnToSender;
            context.IsArmorPiercing = isArmorPiercing;

            if (damageContext != null)
            {
                context.IsDamageSentByThorns = damageContext.IsDamageSentByThorns;
                context.IsDamageSentByBurn = damageContext.IsDamageSentByBurn;
            }

            m_mechanicsList.ApplyMechanics(context);
        }

        //Debug.Log(gameObject.name + " has taken: " + modifiedDamage + " damage");
        OnDamage?.Invoke(context.ModifiedDamage);

        return context;
    }
}
