using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageIndicator : MonoBehaviour
{
    [SerializeField] private Vector3 m_offset;
    [SerializeField] private DamageIndicatorWidget m_Widget;
    
    public void Config(Fighter fighter)
    {
        fighter.HP.OnTookDamage += OnFighterDamaged;
        m_Widget.gameObject.SetActive(false);
    }
    
    public void Config(IHaveHUD owner)
    {
        if (owner is not IDamageable damageable)
        {
            Debug.Log($"ERROR: Damage indicator passed hud owner which was not a IDamageable. owner: {owner.GetType()}");
            return;
        }
        
        damageable.OnDamage += damage => OnFighterDamaged(damage);
        m_Widget.gameObject.SetActive(false);
    }

    public void OnFighterDamaged(int damage, bool HasDied = false)
    {
        m_Widget.gameObject.SetActive(true);
        m_Widget.Play(damage, () => OnAnimationFinished());
        
    }

    public void OnAnimationFinished()
    {
        m_Widget.gameObject.SetActive(false);
    }

    public Vector3 GetOffset()
    {
        return m_offset;
    }
}
