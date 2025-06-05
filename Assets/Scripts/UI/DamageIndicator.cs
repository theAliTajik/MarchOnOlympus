using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageIndicator : MonoBehaviour
{
    [SerializeField] private Vector3 m_offset;
    [SerializeField] private DamageIndicatorWidget m_Widget;
    [SerializeField] private Fighter m_fighter;
    
    public void Config(Fighter fighter)
    {
        m_fighter = fighter;
        m_fighter.HP.OnTookDamage += OnFighterDamaged;
        m_Widget.gameObject.SetActive(false);
    }

    public void OnFighterDamaged(int damage, bool HasDied)
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
