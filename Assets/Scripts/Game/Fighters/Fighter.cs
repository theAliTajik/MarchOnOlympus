using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;

[RequireComponent(typeof(FighterHP))]
public abstract class Fighter : MonoBehaviour, IDamageable, IHaveHUD, IHaveMechanics
{

    public class DamageContext
    {
        public int OriginalDamage;
        public int ModifiedDamage;
        
        public IHaveMechanics Sender;
        public IHaveMechanics Target;

        public bool DoesReturnToSender;
        public bool IsArmorPiercing;
    }
    
    public event Action<Fighter> Death;
    public event Action<int> OnDamage;
    
    [SerializeField] protected FighterHP m_fighterHP;
    [SerializeField] protected Transform m_head;
    [SerializeField] protected Transform m_root;

    protected IDamageable m_damageable;
    private MechanicsList m_mechanicsList;

    public FighterHP HP => m_fighterHP;


    protected virtual void Awake()
    {
        m_fighterHP.Death += OnDeath;
        m_fighterHP.OnTookDamage += OnTookDamage;
        m_damageable = new StandardDamageBehaviour(this);
        m_damageable.OnDamage += m_fighterHP.TakeDamage;
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
        return m_damageable.TakeDamage(damage, sender, doesReturnToSender, isArmorPiercing);
    }
    

    public virtual void Heal(int heal)
    {
        // Debug.Log(this.gameObject.name + " restored: " + heal);
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

    public MechanicsList GetMechanicsList()
    {
        return m_mechanicsList;
    }
}
