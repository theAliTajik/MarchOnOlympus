using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using UnityEditor;
using UnityEngine;

public class MechanicsList
{
    public event Action<MechanicType> OnMechanicUpdated;
    public event Action<MechanicType> OnMechanicRemoved;

    
    private Dictionary<MechanicType, BaseMechanic> m_mechanics = new Dictionary<MechanicType, BaseMechanic>();

    private readonly IReadOnlyList<MechanicType> m_orderOfSenderMechanics = new List<MechanicType>
    {
        MechanicType.FRENZY,
        MechanicType.DAZE,
        MechanicType.STRENGTH,
        MechanicType.IMPALE,
    }.AsReadOnly();
    
    private readonly IReadOnlyList<MechanicType> m_orderOfMechanics = new List<MechanicType>
    {
        MechanicType.VULNERABLE,
        MechanicType.FORTIFIED,
        MechanicType.BLOCK,
        MechanicType.THORNS,
        MechanicType.BURN,
    }.AsReadOnly();


    private IHaveMechanics m_mechanicsOwner;
    private bool m_isPlayer;

    public MechanicsList(bool isPlayer)
    {
        m_isPlayer = isPlayer;
    }

    public MechanicsList(bool isPlayer, MechanicType mechanicType, BaseMechanic mechanic)
    {
        m_isPlayer = isPlayer;
    }
    
    

    private void OnMechanicEnd(MechanicType mechanicType)
    {
        Remove(mechanicType);
    }

    private void OnMechanicChanged(MechanicType mechanicType)
    {
        OnMechanicUpdated?.Invoke(mechanicType);
    }

    
    public BaseMechanic GetMechanic(MechanicType mechanicType)
    {
        if (m_mechanics.ContainsKey(mechanicType))
        {
            return m_mechanics[mechanicType];
        }

        Debug.Log(mechanicType.ToString() + " mechanic not found");
        return null;
    }

    public List<MechanicType> GetAllMechanics()
    {
        return m_mechanics.Keys.ToList();
    }
    
    public bool Contains(MechanicType mechanic)
    {
        if (m_mechanics.ContainsKey(mechanic))
        {
            return true;
        }
        return false;
    }

    public bool ContainsAny(List<MechanicType> mechanics)
    {
        bool DoesContain = false;
        foreach (MechanicType mechanic in mechanics)
        {
            if (Contains(mechanic))
            {
                DoesContain = true;
            }
        }
        return DoesContain;
    }
    
    public int GetCount()
    {
        return m_mechanics.Count;
    }

    public int GetBuffsCount()
    {
        int count = m_mechanics.Count;

        count -= GetDebuffsCount();
        

        return count;
    }

    public int GetDebuffsCount()
    {
        int count = 0;
        foreach (MechanicType mechanic in MechanicsManager.Instance.DebuffMechanics)
        {
            foreach (MechanicType listMechanic in m_mechanics.Keys)
            {
                if (listMechanic == mechanic)
                {
                    count++;
                }
            }
        }

        return count;
    }
    
    public void ApplyMechanics(Fighter.DamageContext context)
    {
        foreach (MechanicType mechanicType in m_orderOfSenderMechanics)
        {
            if (MechanicsManager.Instance.Contains(context.Sender, mechanicType))
            {
               MechanicsManager.Instance.ApplyMechanic(mechanicType, context);
            }
        }
        
        foreach (MechanicType mechanicType in m_orderOfMechanics)
        {
            if (m_mechanics.ContainsKey(mechanicType))
            {
                m_mechanics[mechanicType].Apply(context);
            }
        }
    }

    public void ApplyMechanic(MechanicType mechanicType, Fighter.DamageContext context)
    {
        if (m_mechanics.ContainsKey(mechanicType))
        {
            m_mechanics[mechanicType].Apply(context);
        }
    }
    
    
    public bool Add(BaseMechanic mechanic)
    {
        BaseMechanic dexterity = null;
        MechanicType mechanicType = mechanic.GetMechanicType();
        if (m_mechanics.ContainsKey(MechanicType.DEXTERITY))
        {
            dexterity = m_mechanics[MechanicType.DEXTERITY];
        }
        if (dexterity != null && mechanicType == MechanicType.BLOCK)
        {
            mechanic.IncreaseStack(dexterity.Stack);
        }
        
        bool success = m_mechanics.TryAdd(mechanicType, mechanic);

        if (!success)
        {
            m_mechanics[mechanicType].IncreaseStack(mechanic.Stack);
            success = true;
            if (mechanic.HasGuard)
            {
                m_mechanics[mechanicType].SetGuard(mechanic.GuardMin);
            }
        }


        if (mechanicType == MechanicType.IMPALE)
        {
            if (m_mechanics[mechanicType].Stack > 10)
            {
                Remove(mechanicType);
                Add(new BleedMechanic(5, mechanic.MechanicOwner));
            } 
        }
        
        if (mechanicType == MechanicType.BURN)
        {
            if (m_mechanics[mechanicType].Stack > 10)
            {
                Add(new ExplodeMechanic(1, mechanic.MechanicOwner));
                Remove(MechanicType.BURN);
            } 
        }
        
        mechanic.OnEnd += OnMechanicEnd;
        mechanic.OnChange += OnMechanicChanged;
        OnMechanicUpdated?.Invoke(mechanicType);
        return success;
    }

    public void Remove(MechanicType mechanicType)
    {
        OnMechanicRemoved?.Invoke(mechanicType);
        m_mechanics.Remove(mechanicType);
    }

    public void ReduceMechanic(MechanicType mechanicType, int amount)
    {
        if (!m_mechanics.ContainsKey(mechanicType))
        {
            Debug.Log($"WARNING: tried to reduce non existent mechanic of type {mechanicType}");
            return;
        }
        
        m_mechanics[mechanicType].ReduceStack(amount);
    }



    public void OnPhaseChange(CombatPhase phase)
    {
        bool isMyTurn = IsMyTurn();
        
        List<BaseMechanic> mechanicsValues = m_mechanics.Values.ToList();
        for (int i = 0; i < mechanicsValues.Count; i++)
        {
            mechanicsValues[i].TryReduceStack(phase, isMyTurn);
        }
    }

    protected bool IsMyTurn()
    {
        return CombatManager.Instance.IsPlayersTurn == m_isPlayer;
    }

    public void RemoveAllMechanicsOfCategory(MechanicCategory category)
    {
        List<MechanicType> keysToRemove = MechanicsManager.Instance.GetMechanicsOfCatigoryPresent(m_mechanics.Keys.ToList(), category);

        foreach (MechanicType mechanic in keysToRemove)
        {
            Remove(mechanic);
        }
    }
}
