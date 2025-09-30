
using System;
using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public class CyclopsClub : IDamageable, IColliderMatcher, IHaveMechanics, IHaveHUD, IHaveHP
{
    public Action OnDeath;
    public Action OnRevive;
    public event Action<int> OnDamage;

    [SerializeField] private ColliderMatcher m_colliderMatcher;
    [SerializeField] private FighterHP m_fighterHP;
    [SerializeField] private CyclopsClubMoveData m_data;
    [SerializeField] private Transform m_root;

    private int m_hp;
    private MechanicsList m_mechanicsList;
    
    private IDamageable m_damageable;


    public void Config()
    {
        m_damageable = new CyclopsClubDamageBehaviour(this);
        ConfigFighterHP();
        
        m_damageable.OnDamage += m_fighterHP.TakeDamage;
        m_damageable.OnDamage += (damage) => OnDamage?.Invoke(damage);
        m_fighterHP.Death += () => OnDeath?.Invoke();
    }

    public bool IsMyCollider(Collider2D collider)
    {
        Debug.Log($"is null = {collider == null}");
        return m_colliderMatcher.IsMyCollider(collider);
    }

    public Fighter.DamageContext TakeDamage(int damage, Fighter sender, bool doesReturnToSender = true,
        bool isArmorPiercing = false,
        Fighter.DamageContext damageContext = null)
    {
        return m_damageable.TakeDamage(damage, sender, doesReturnToSender, isArmorPiercing);
    }

    public void Revive()
    {
        ConfigFighterHP();
        OnRevive?.Invoke();
    }
    
    public void ConfigFighterHP()
    {
        m_fighterHP.SetMax(m_data.HP);
        m_fighterHP.ResetHP();
    }

    public bool isAlive()
    {
        return m_fighterHP.CheckIsAlive();
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
