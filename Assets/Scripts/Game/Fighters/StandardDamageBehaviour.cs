
using System;

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
    
    public virtual int TakeDamage(int damage, Fighter sender, bool doesReturnToSender, bool isArmorPiercing = false)
    {
        if (damage < 0)
        {
            damage = 0;
        }

        if (m_mechanicsList == null && m_mechanicsOwner != null)
        {
            m_mechanicsList = MechanicsManager.Instance.GetMechanicsList(m_mechanicsOwner);
        }

        Fighter.DamageContext context = new Fighter.DamageContext();
        context.ModifiedDamage = damage;
        if (m_mechanicsList != null)
        {
            context.OriginalDamage = damage;
            context.Sender = sender;
            context.Target = m_mechanicsOwner;
            context.DoesReturnToSender = doesReturnToSender;
            context.IsArmorPiercing = isArmorPiercing;

            m_mechanicsList.ApplyMechanics(context);
        }

        //Debug.Log(gameObject.name + " has taken: " + modifiedDamage + " damage");
        OnDamage?.Invoke(context.ModifiedDamage);

        return context.ModifiedDamage;
    }
}
