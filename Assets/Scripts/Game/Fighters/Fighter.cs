using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;

[RequireComponent(typeof(FighterHP))]
public abstract class Fighter : MonoBehaviour, IHaveHUD
{

    public class DamageContext
    {
        public int OriginalDamage;
        public int ModifiedDamage;
        
        public Fighter Sender;
        public Fighter Target;

        public bool DoesReturnToSender;
        public bool IsArmorPiercing;
    }
    
    public event Action<Fighter> Death;
    [SerializeField] protected FighterHP m_fighterHP;
    [SerializeField] protected Transform m_head;
    [SerializeField] protected Transform m_root;

    private MechanicsList m_mechanicsList;

    public FighterHP HP => m_fighterHP;
    
    protected virtual void Awake()
    {
        m_fighterHP.Death += OnDeath;
        m_fighterHP.OnTookDamage += OnTookDamage;
    }

    protected virtual void OnTookDamage(int damage, bool isCritical)
    {
    }

    protected virtual void OnDeath()
    {
        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        if (collider != null)
        {
            collider.enabled = false;
        }
        
        Death?.Invoke(this);
    }

    public virtual int TakeDamage(int damage, Fighter sender, bool doesReturnToSender, bool isArmorPiercing = false)
    {
        if (damage < 0)
        {
            damage = 0;
        }
        

        if (m_mechanicsList == null)
        {
            m_mechanicsList = MechanicsManager.Instance.GetMechanicsList(this);
        }

        DamageContext context = new DamageContext();
        if (m_mechanicsList != null)
        {
            context.OriginalDamage = damage;
            context.ModifiedDamage = damage;
            context.Sender = sender;
            context.Target = this;
            context.DoesReturnToSender = doesReturnToSender;
            context.IsArmorPiercing = isArmorPiercing;
            
            m_mechanicsList.ApplyMechanics(context);
        }

        //Debug.Log(gameObject.name + " has taken: " + modifiedDamage + " damage");
        m_fighterHP.TakeDamage(context.ModifiedDamage);
        if (this is BaseEnemy)
        {
            GameplayEvents.SendGamePhaseChanged(EGamePhase.ENEMY_DAMAGED);
        }
        
        return context.ModifiedDamage;
    }
    

    public virtual void Heal(int heal)
    {
        Debug.Log(this.gameObject.name + " restored: " + heal);
        m_fighterHP.Heal(heal);
        GameplayEvents.SendFighterRestoredHP(this, heal);
    }


    public virtual Vector3 GetRootPosition()
    {
        return m_root.position;
    }

    public virtual Vector3 GetHeadPosition()
    {
        return m_head.position;
    }
    
    public virtual void ConfigFighterHP()
    {
        
    }
}
