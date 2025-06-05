using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public enum ETriggerType
{
    ABOVE,
    EQUAL,
    BELOW
}

public class FighterHP : MonoBehaviour
{
    public event Action Death;
    public event Action OnHPChanged;
    public event Action<int, bool> OnTookDamage;
    public event Action<TriggerPercentage> OnPercentageTrigger;  

    private int m_max = 100;
    private int m_maxModifier;
    private int m_current;

    [Serializable]
    public struct TriggerPercentage : IEquatable<TriggerPercentage>
    {
        public float Percentage;
        public ETriggerType TriggerType;
        public bool Equal;
        public bool RemoveOnTrigger;

        public TriggerPercentage(int percentage,  ETriggerType triggerType, bool equal = false, bool removeOnTrigger = false)
        {
            this.Percentage = percentage/100f;
            this.TriggerType = triggerType;
            this.Equal = equal;
            this.RemoveOnTrigger = removeOnTrigger;
        }


        public bool Equals(TriggerPercentage other)
        {
            //bool equal = Percentage.Equals(other.Percentage);

            if (!Mathf.Approximately(Percentage, other.Percentage) && !Mathf.Approximately(other.Percentage/100f, Percentage))
            {
                return false;
            }
            
            if (TriggerType != other.TriggerType)
            {
                return false;
            }

            if (Equal != other.Equal)
            {
                return false;
            }

            if (RemoveOnTrigger != other.RemoveOnTrigger)
            {
                return false;
            }
            
            return true;
        }

        public override bool Equals(object obj)
        {
            return obj is TriggerPercentage other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Percentage, (int)TriggerType, Equal, RemoveOnTrigger);
        }

        public static bool operator ==(TriggerPercentage left, TriggerPercentage right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(TriggerPercentage left, TriggerPercentage right)
        {
            return !left.Equals(right);
        }
    }
    private List<TriggerPercentage> m_triggerPercentages = new List<TriggerPercentage>(); 
    
    private bool m_hasGuard = false;
    private int m_guardMin = 0;

    private bool m_hadDamageGurad = false;
    private float m_damageGuardMaxPercentage = 1;    

    public int Max => m_max; 
    public int Current { get { return m_current; } }
    
    public bool HasGuard { get { return m_hasGuard; } }
    
    public bool HadDamageGurad { get { return m_hadDamageGurad; } }
    
#if UNITY_EDITOR

    [ContextMenu("Kill")]
    public void Kill()
    {
        int dmg = m_current;
        TakeDamage(dmg);
    }
    
    [ContextMenu("Heal 5")]
    public void Heal5()
    {
        Heal(5);
    }
    
    [ContextMenu("Set to 1")]
    public void SetTo1()
    {
        int dmg = m_current - 1;
        
        TakeDamage(dmg);
    }
    
    [ContextMenu("Set to 67%")]
    public void SetTo67()
    {
        int sixtySevenPrecentOfHP = (int)Math.Round(m_max * 0.67f);
        int dmg = 0;
        if (m_current > sixtySevenPrecentOfHP)
        {
            dmg = m_current - sixtySevenPrecentOfHP;
        }

        if (dmg > 0)
        {
            TakeDamage(dmg);
        }
        else
        {
            Heal(-dmg);
        }
    }

#endif

    public void SetGuard(int minHP)
    {
        m_hasGuard = true;
        m_guardMin = minHP;
    }

    public void RemoveGuard()
    {
        m_hasGuard = false;
        m_guardMin = 0;
    }
    
    public void SetDamageGuard(int maxDamagePercentage)
    {
        m_hadDamageGurad = true;
        m_damageGuardMaxPercentage = maxDamagePercentage / 100f;
    }

    public void RemoveDamageGuard()
    {
        m_hadDamageGurad = false;
        m_damageGuardMaxPercentage = 1;
    }
    
    private void Awake()
    {
        m_current = m_max;
        OnHPChanged += OnHpChanged;
    }

    public void SetMax(int max)
    {
        m_max = max;
        OnHPChanged?.Invoke();
    }

    public void IncreaseMaxHP(int amount)
    {
        m_maxModifier += amount;
        m_max += amount;
        OnHPChanged?.Invoke();
    }

    public void DecreaseMaxHP(int amount)
    {
        m_maxModifier -= amount;
        m_max -= amount;
        if (m_current > m_max)
        {
            m_current = m_max;
        }
        OnHPChanged?.Invoke();
    }

    public void TakeDamage(int damage)
    {
        if (damage <= 0)
        {
            return;
        }
        
        if (damage > m_current)
        {
            damage = m_current;
        }
        
        float DamagePercentage = (float)damage / m_max;
        if (m_hadDamageGurad && DamagePercentage  > m_damageGuardMaxPercentage)
        {
            damage = (int)(m_max * m_damageGuardMaxPercentage);
        }

        if (m_hasGuard && m_current - damage < m_guardMin)
        {
            damage = m_current - m_guardMin;
        }
        m_current -= damage;
        if (m_current == 0)
        {
            OnHPChanged?.Invoke();
            OnTookDamage?.Invoke(damage, true);
            Death?.Invoke();
            return;
        }
        
        OnHPChanged?.Invoke();
        OnTookDamage?.Invoke(damage, false);
    }

    public void Heal(int heal)
    {
        if (m_current + heal >= m_max)
        {
            m_current = m_max;
        }
        else
        {
            m_current += heal;
        }
        OnHPChanged?.Invoke();
    }

    public void ResetHP()
    {
        m_current = m_max;
        OnHPChanged?.Invoke();
    }

    public bool IsDamageFatal(int damage)
    {
        if (m_current - damage <= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void SetTrigger(TriggerPercentage trigger)
    {
        if (trigger.Percentage > 1)
        {
            trigger.Percentage /= 100f;
        }
        m_triggerPercentages.Add(trigger);
    }
    
    private void OnHpChanged()
    {
        float fighterHPPercentage = m_current / (float)m_max;
        for (var i = 0; i < m_triggerPercentages.Count; i++)
        {
            bool isTriggerd = m_triggerPercentages[i].Equal && m_triggerPercentages[i].Percentage == fighterHPPercentage;

            switch (m_triggerPercentages[i].TriggerType)
            {
                case ETriggerType.ABOVE:
                    if (fighterHPPercentage > m_triggerPercentages[i].Percentage)
                    {
                        isTriggerd = true;
                    }
                    break;
                case ETriggerType.BELOW:
                    if (fighterHPPercentage < m_triggerPercentages[i].Percentage)
                    {
                        isTriggerd = true;
                    }
                    break;
            }

            if (isTriggerd)
            {
                OnPercentageTrigger?.Invoke(m_triggerPercentages[i]);
                if (m_triggerPercentages[i].RemoveOnTrigger)
                {
                    m_triggerPercentages.RemoveAt(i);
                    i--;
                }
            }

        }
    }
    
    
}
