
using System;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class CyclopsEye : IDamageable, IColliderMatcher, IHaveMechanics, IHaveHUD, IHaveHP
{
    public Action OnDeath; 
    public event Action<int> OnDamage;
    
    [SerializeField] private ColliderMatcher m_colliderMatcher;
    [SerializeField] private FighterHP m_fighterHP;
    [SerializeField] private CyclopsEyeMoveData m_data;
    [SerializeField] private Transform m_root;

    private int m_hp;
    private MechanicsList m_mechanicsList;
    
    private IDamageable m_damageable;
    private bool m_isActive;

    public CyclopsEye()
    {
        m_damageable = new EnemyDamageBehaviour(this);
    }

    public void Config()
    {
        ConfigFighterHP();
        
        m_damageable.OnDamage += m_fighterHP.TakeDamage;
        m_damageable.OnDamage += (damage) => OnDamage?.Invoke(damage);
        m_fighterHP.Death += () => OnDeath?.Invoke();
    }

    public bool IsMyCollider(Collider2D collider)
    {
        if(!m_isActive)
            return false;
            
        return m_colliderMatcher.IsMyCollider(collider);
    }


    public Fighter.DamageContext TakeDamage(int damage, Fighter sender, bool doesReturnToSender = true,
        bool isArmorPiercing = false,
        Fighter.DamageContext damageContext = null)
    {
        return m_damageable.TakeDamage(damage, sender, isArmorPiercing, doesReturnToSender);
    }

    public void SetState(bool isActive)
    {
        m_isActive = isActive;
    }
    
    public void ConfigFighterHP()
    {
        m_fighterHP.SetMax(m_data.HP);
        m_fighterHP.ResetHP();
    }

    public MechanicsList GetMechanicsList()
    {
        return m_mechanicsList;
    }

    public Vector3 GetRootPosition()
    {
        return m_root.position;
    }

    public Vector3 GetHeadPosition()
    {
        return m_root.position;
    }

    public FighterHP GetHP()
    {
        return m_fighterHP;
    }
}
