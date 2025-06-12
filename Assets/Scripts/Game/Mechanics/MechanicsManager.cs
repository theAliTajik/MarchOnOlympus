using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using UnityEngine;

public class MechanicsManager : Singleton<MechanicsManager>
{

    private Dictionary<Fighter, MechanicsList> m_allMechanics = new Dictionary<Fighter, MechanicsList>();
    [SerializeField] private CombatManager m_combatManager;
    [SerializeField] private MechanicsDisplay m_playerMechanicsDisplay, m_enemyMechanicsDisplay;
    
    private List<MechanicType> m_DebuffMechanics = new List<MechanicType>
    {
        MechanicType.BLEED,
        MechanicType.VULNERABLE,
        MechanicType.STUN,
        MechanicType.DAZE,
        MechanicType.BURN,
    };
    
    public List<MechanicType> DebuffMechanics { get { return m_DebuffMechanics; } }
    
    public List<MechanicType> DamageOverTimeMechanics = new List<MechanicType>
    {
        MechanicType.BLEED,
    };

    /*
    public List<MechanicType> m_DebuffMechanics
    {
        get
        {
            Debug.Log("Debug mechanics get");
            return m_DebuffMechanics2;
        }
        set
        {
            Debug.Log("Debuf mechanic is set");
            m_DebuffMechanics2 = value;
        }
    }
    */
    

    private void Start()
    {
        if (m_combatManager != null)
        {
            m_combatManager.OnCombatPhaseChanged += OnPhaseChange;
        }
    }
    
    public void OnPhaseChange(CombatPhase phase)
    {
        List<MechanicsList> mechanicsList = m_allMechanics.Values.ToList();
        for (int i = mechanicsList.Count - 1; i >= 0; i--)
        {
            mechanicsList[i].OnPhaseChange(phase);
        }
        GameplayEvents.SendGamePhaseChanged(EGamePhase.MECHANICS_FINISHED_TRY_REDUCE);
    }


    public MechanicsList GetMechanicsList(Fighter fighter)
    {
        if (m_allMechanics.ContainsKey(fighter))
        {
            return m_allMechanics[fighter];
        }

        //Debug.LogError("MechanicsManager::GetMechanicsList: fighter not found");
        return null;
    }

    public BaseMechanic GetMechanic(Fighter fighter, MechanicType mechanicType)
    {
        m_allMechanics.TryGetValue(fighter, out MechanicsList mechanicsList);
        if (mechanicsList == null)
        {
            return null;
        }
        
        return mechanicsList.GetMechanic(mechanicType);
    }

    public int GetMechanicsStack(Fighter fighter, MechanicType mechanicType)
    {
        if (!m_allMechanics.TryGetValue(fighter, out MechanicsList mechanicsList))
        {
            return -1;
        }

        BaseMechanic mechanic = null;
        if (mechanicsList.Contains(mechanicType))
        {
            mechanic = mechanicsList.GetMechanic(mechanicType);
        }
        else
        {
            return -1;
        }
        return mechanic.Stack;
    }

    public bool Contains(Fighter fighter, MechanicType mechanic)
    {
        if (m_allMechanics.ContainsKey(fighter))
        {
            if (m_allMechanics[fighter].Contains(mechanic))
            {
                return true;
            }
        }

        return false;
    }

    public bool ContainsAny(Fighter fighter, List<MechanicType> mechanics)
    {
        if (m_allMechanics.ContainsKey(fighter))
        {
            if (m_allMechanics[fighter].ContainsAny(mechanics))
            {
                return true;
            }
        }
        return false;
    }

    public bool AnyEnemyContainsAny(List<MechanicType> mechanics)
    {
        foreach (KeyValuePair<Fighter,MechanicsList> Item in m_allMechanics)
        {
            if (Item.Key.CompareTag("Player"))
            {
                continue;
            }

            if (Item.Value.ContainsAny(mechanics))
            {
                return true;
            }
        }
        return false;
    }
    
    public int GetMechanicsCount(Fighter fighter, MechanicCategory mechanicCategory)
    {
        if (!m_allMechanics.TryGetValue(fighter, out MechanicsList mechanicsList))
        {
            return -1;
        }

        switch (mechanicCategory)
        {
            case MechanicCategory.ALL:
                return mechanicsList.GetCount();
                break;
            case MechanicCategory.BUFF:
                return mechanicsList.GetBuffsCount();
                break;
            case MechanicCategory.DEBUFF:
                return mechanicsList.GetDebuffsCount();
            break;
            default:
                return -1;
                break;
        }
    }

    public void CreateMechanicsList(Fighter fighter)
    {
        if (m_allMechanics.ContainsKey(fighter))
        {
            Debug.Log($"Fighter {fighter.name} already has mechanics list");
            return;
        }
        
        bool isPlayer = fighter.CompareTag("Player");
        MechanicsList mechanicsList = new MechanicsList(isPlayer);
        m_allMechanics.Add(fighter, mechanicsList);

        HUD.Instance.SpawnMechanicsDisplay(fighter, mechanicsList);
    }
    public void RemoveMechanicsList(Fighter fighter)
    {
        m_allMechanics.Remove(fighter);
    }
    
    public void AddMechanic(BaseMechanic mechanic, Fighter sender = null)
    {
        if (sender == null)
        {
            sender = GameInfoHelper.GetPlayer();
        }
        Fighter fighter = mechanic.GetFighter();
        if (fighter == null)
        {
            return;
        }
        
        MechanicType mechanicType = mechanic.GetMechanicType();
        
        if (mechanic.Stack < 1)
        {
            return;
        }

        if (m_allMechanics.TryGetValue(fighter, out MechanicsList list))
        {
            list.Add(mechanic);
        }
        else
        {
            CreateMechanicsList(fighter);
            m_allMechanics[fighter].Add(mechanic);
        }
        
        GameplayEvents.SendMechanicAddedToFighter(fighter, sender, m_allMechanics[fighter].GetMechanic(mechanicType));
    }

    public void AddMechanic(int stack, MechanicType mechanicType, Fighter fighter, bool hasGuard, int guardMin)
    {
        BaseMechanic mec = CreateMechanicOfType(stack, mechanicType, fighter, hasGuard, guardMin);
        if (mec == null)
        {
            Debug.Log("could not create mechanic of type");
            return;
        }
        
        AddMechanic(mec);
    }
    
    private static BaseMechanic CreateMechanicOfType(int stack, MechanicType mechanicType, Fighter fighter, bool hasGuard = false, int guardMin = 0)
    {
        BaseMechanic mechanic = null;
        
        switch (mechanicType)
        {
            case MechanicType.STRENGTH:
                mechanic = new StrenghtMechanic(stack, fighter, hasGuard, guardMin);
                break;
            case MechanicType.BLOCK:
                mechanic = new BlockMechanic(stack, fighter, hasGuard, guardMin);
                break;
            case MechanicType.FORTIFIED:
                mechanic = new FortifiedMechanic(stack, fighter, hasGuard, guardMin);
                break;
            case MechanicType.DEXTERITY:
                mechanic = new DexterityMechanic(stack, fighter, hasGuard, guardMin);
                break;
            case MechanicType.THORNS:
                mechanic = new ThornsMechanic(stack, fighter, hasGuard, guardMin);
                break;
            case MechanicType.FRENZY:
                mechanic = new FrenzyMechanic(stack, fighter, hasGuard, guardMin);
                break;
            case MechanicType.IMPALE:
                mechanic = new ImpaleMechanic(stack, fighter, hasGuard, guardMin);
                break;
            case MechanicType.BLEED:
                mechanic = new BleedMechanic(stack, fighter, hasGuard, guardMin);
                break;
            case MechanicType.BURN:
                mechanic = new BurnMechanic(stack, fighter, hasGuard, guardMin);
                break;
            case MechanicType.DAZE:
                mechanic = new DazeMechanic(stack, fighter, hasGuard, guardMin);
                break;
            case MechanicType.STUN:
                mechanic = new StunMechanic(stack, fighter, hasGuard, guardMin);
                break;
            case MechanicType.VULNERABLE:
                mechanic = new VulnerableMechanic(stack, fighter, hasGuard, guardMin);
                break;
            case MechanicType.IMPROVISE:
                mechanic = new ImproviseMechanic(stack, fighter, hasGuard, guardMin);
                break;
            default:
                Debug.Log("Unknown mechanic type");
                break;
        }
        return mechanic;
    }


    public void ApplyMechanic(MechanicType mechanicType, Fighter.DamageContext context)
    {
        if (m_allMechanics.TryGetValue(context.Sender, out MechanicsList list))
        {
            list.ApplyMechanic(mechanicType, context);
            return;
        }
    }

    public void RemoveMechanic(Fighter fighter, MechanicType mechanicType)
    {
        if (fighter != null && m_allMechanics.ContainsKey(fighter))
        {
            m_allMechanics[fighter].Remove(mechanicType);
        }
    }

    public void ReduceMechanic(Fighter fighter, MechanicType mechanicType, int amount)
    {
        if (fighter != null && m_allMechanics.ContainsKey(fighter))
        {
            m_allMechanics[fighter].ReduceMechanic(mechanicType, amount);
        }
    }


    protected override void Init()
    {
        
    }

    public void RemoveAllMechancis(Fighter fighter)
    {
        if (m_allMechanics.ContainsKey(fighter))
        {
            m_allMechanics[fighter].RemoveAllMechanicsOfCatigory(MechanicCategory.ALL);
        }
    }

    public void RemoveAllMechanicsOfCatigory(Fighter fighter, MechanicCategory mechanicCategory)
    {
        if (m_allMechanics.ContainsKey(fighter))
        {
            m_allMechanics[fighter].RemoveAllMechanicsOfCatigory(mechanicCategory);
        }
    }

    public List<MechanicType> GetMechanicsOfCatigoryPresent(List<MechanicType> mechanics,
        MechanicCategory mechanicCategory)
    {
        List<MechanicType> present = new List<MechanicType>();
        switch (mechanicCategory)
        {
            case MechanicCategory.DEBUFF:
                foreach (MechanicType debuffMechanic in m_DebuffMechanics)
                {
                    if (mechanics.Contains(debuffMechanic))
                    {
                        present.Add(debuffMechanic);
                    }
                }
                break;
            case MechanicCategory.BUFF:
                
                break;
            case MechanicCategory.ALL:
                present.AddRange(mechanics);
                break;
                
        }

        return present;
    }


}
