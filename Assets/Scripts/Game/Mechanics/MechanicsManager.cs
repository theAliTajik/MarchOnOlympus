using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using Unity.Burst;
using UnityEngine;

public class MechanicsManager : Singleton<MechanicsManager>
{

    private Dictionary<IHaveMechanics, MechanicsList> m_allMechanics = new Dictionary<IHaveMechanics, MechanicsList>();
    [SerializeField] private CombatManager m_combatManager;
    [SerializeField] private MechanicsDisplay m_playerMechanicsDisplay, m_enemyMechanicsDisplay;
    
    private List<MechanicType> m_DebuffMechanics = new List<MechanicType>
    {
        MechanicType.BLEED,
        MechanicType.VULNERABLE,
        MechanicType.STUN,
        MechanicType.DAZE,
        MechanicType.BURN,
        MechanicType.PETRIFY,
        MechanicType.HAUNT
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
        return GetMechanicsList(fighter as IHaveMechanics);
    }
    public MechanicsList GetMechanicsList(IHaveMechanics owner)
    {
        if (owner == null)
        {
            // Debug.Log("ERROR: tried to get mechanic of null owner");
            return null;
        }
        
        if (m_allMechanics.ContainsKey(owner))
        {
            return m_allMechanics[owner];
        }

        //Debug.LogError("MechanicsManager::GetMechanicsList: fighter not found");
        return null;
    }

    public BaseMechanic GetMechanic(Fighter fighter, MechanicType mechanicType)
    {
        if (fighter == null)
        {
            Debug.Log("WARRNING: GetMechanic failed: Null fighter passed");
            return null;
        }
        
        return GetMechanic(fighter as IHaveMechanics, mechanicType);
    }
    public BaseMechanic GetMechanic(IHaveMechanics owner, MechanicType mechanicType)
    {
        if (owner == null)
        {
            Debug.Log("WARRNING: GetMechanic failed: Null owner passed");
            return null;
        }
        
        m_allMechanics.TryGetValue(owner, out MechanicsList mechanicsList);
        if (mechanicsList == null)
        {
            Debug.Log($"WARNING: No mechanic list found for owner: {((MonoBehaviour)owner).name}");
            
            MonoBehaviour ownerBehaviour = owner as MonoBehaviour;
            
            Debug.LogError($"Mechanic lookup failed for owner '{ownerBehaviour.name}' (Instance ID: {ownerBehaviour.GetInstanceID()}). This key was not found in the dictionary.", ownerBehaviour);
            
            // Log all current keys for comparison
            foreach (var key in m_allMechanics.Keys)
            {
                var keyBehaviour = key as UnityEngine.Object;
                Debug.Log($"Available Key in Dictionary: '{keyBehaviour.name}' (Instance ID: {keyBehaviour.GetInstanceID()})");
            }
            return null;
        }
        
        return mechanicsList.GetMechanic(mechanicType);
    }

    public int GetMechanicsStack(Fighter fighter, MechanicType mechanicType)
    {
        return GetMechanicsStack(fighter as IHaveMechanics, mechanicType);
    }
    public int GetMechanicsStack(IHaveMechanics owner, MechanicType mechanicType)
    {
        if (!m_allMechanics.TryGetValue(owner, out MechanicsList mechanicsList))
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
        return Contains(fighter as IHaveMechanics, mechanic);
    }
    public bool Contains(IHaveMechanics owner, MechanicType mechanic)
    {
        if (owner == null)
        {
            // Debug.Log("WARNING: Tried to check if owner has mechanic of type. Owner was null.");
            return false;
        }
        
        if (m_allMechanics.ContainsKey(owner))
        {
            if (m_allMechanics[owner].Contains(mechanic))
            {
                return true;
            }
        }

        return false;
    }

    public bool ContainsAny(Fighter fighter, List<MechanicType> mechanics)
    {
        return ContainsAny(fighter as IHaveMechanics, mechanics);
    }
    public bool ContainsAny(IHaveMechanics owner, List<MechanicType> mechanics)
    {
        if (m_allMechanics.ContainsKey(owner))
        {
            if (m_allMechanics[owner].ContainsAny(mechanics))
            {
                return true;
            }
        }
        return false;
    }

    public bool AnyEnemyContainsAny(List<MechanicType> mechanics)
    {
        foreach (KeyValuePair<IHaveMechanics,MechanicsList> Item in m_allMechanics)
        {
            Fighter possiblePlayer = Item.Key as Fighter;
            if (possiblePlayer != null && possiblePlayer.CompareTag("Player"))
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
        return GetMechanicsCount(fighter as IHaveMechanics, mechanicCategory);
    }
    public int GetMechanicsCount(IHaveMechanics owner, MechanicCategory mechanicCategory)
    {
        if (!m_allMechanics.TryGetValue(owner, out MechanicsList mechanicsList))
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

    public MechanicsList CreateMechanicsList(Fighter fighter)
    {
        return CreateMechanicsList(fighter as IHaveMechanics);
    }
    public MechanicsList CreateMechanicsList(IHaveMechanics owner)
    {
        if (m_allMechanics.ContainsKey(owner))
        {
            Debug.Log($"Fighter {owner.GetType()} already has mechanics list");
            return null;
        }

        bool isPlayer = false;
        if (owner is Fighter fighter)
        {
            isPlayer = fighter.CompareTag("Player");
        }


        MechanicsList list = new MechanicsList(isPlayer);
        m_allMechanics.Add(owner, list);
        // Debug.Log($"Made mechanics list for {owner.GetType()}");
        return list;
    }
    public void RemoveMechanicsList(Fighter fighter)
    {
        m_allMechanics.Remove(fighter);
    }

    public void AddMechanic(BaseMechanic mechanic, Fighter sender = null)
    {
        AddMechanic(mechanic, sender as IHaveMechanics);
    }
    public void AddMechanic(BaseMechanic mechanic, IHaveMechanics sender)
    {
        if (sender == null)
        {
            sender = GameInfoHelper.GetPlayer();
        }

        IHaveMechanics owner = mechanic.GetMechanicOwner();
        if (owner == null)
        {
            return;
        }
        
        MechanicType mechanicType = mechanic.GetMechanicType();
        
        if (mechanic.Stack < 1)
        {
            return;
        }

        if (m_allMechanics.TryGetValue(owner, out MechanicsList list))
        {
            list.Add(mechanic);
        }
        else
        {
            CreateMechanicsList(owner);
            m_allMechanics[owner].Add(mechanic);
        }

        if (owner is Fighter fighter && sender is Fighter senderfighter)
        {
            GameplayEvents.SendMechanicAddedToFighter(fighter, senderfighter, m_allMechanics[owner].GetMechanic(mechanicType));
        }
    }

    public void AddMechanic(int stack, MechanicType mechanicType, Fighter fighter, int guardMin = 0)
    {
        AddMechanic(stack, mechanicType, fighter as IHaveMechanics, guardMin);
    }
    public void AddMechanic(int stack, MechanicType mechanicType, IHaveMechanics owner, int guardMin = 0)
    {
        BaseMechanic mec = CreateMechanicOfType(stack, mechanicType, owner, guardMin);
        if (mec == null)
        {
            Debug.Log("could not create mechanic of type");
            return;
        }
        
        AddMechanic(mec);
    }

    public static BaseMechanic CreateMechanicOfType(int stack, MechanicType mechanicType, Fighter fighter,
        int guardMin = 0)
    {
        return CreateMechanicOfType(stack, mechanicType, fighter as IHaveMechanics, guardMin);
    }
    public static BaseMechanic CreateMechanicOfType(int stack, MechanicType mechanicType, IHaveMechanics owner,
        int guardMin = 0)
    {
        BaseMechanic mechanic = null;
        
        switch (mechanicType)
        {
            case MechanicType.STRENGTH:
                mechanic = new StrenghtMechanic(stack, owner, guardMin);
                break;
            case MechanicType.BLOCK:
                mechanic = new BlockMechanic(stack, owner, guardMin);
                break;
            case MechanicType.FORTIFIED:
                mechanic = new FortifiedMechanic(stack, owner, guardMin);
                break;
            case MechanicType.DEXTERITY:
                mechanic = new DexterityMechanic(stack, owner, guardMin);
                break;
            case MechanicType.THORNS:
                mechanic = new ThornsMechanic(stack, owner, guardMin);
                break;
            case MechanicType.FRENZY:
                mechanic = new FrenzyMechanic(stack, owner, guardMin);
                break;
            case MechanicType.IMPALE:
                mechanic = new ImpaleMechanic(stack, owner, guardMin);
                break;
            case MechanicType.BLEED:
                mechanic = new BleedMechanic(stack, owner, guardMin);
                break;
            case MechanicType.BURN:
                mechanic = new BurnMechanic(stack, owner, guardMin);
                break;
            case MechanicType.DAZE:
                mechanic = new DazeMechanic(stack, owner, guardMin);
                break;
            case MechanicType.STUN:
                mechanic = new StunMechanic(stack, owner, guardMin);
                break;
            case MechanicType.VULNERABLE:
                mechanic = new VulnerableMechanic(stack, owner, guardMin);
                break;
            case MechanicType.IMPROVISE:
                mechanic = new ImproviseMechanic(stack, owner, guardMin);
                break;
            case MechanicType.PETRIFY:
                return new PetrifyMechanic(stack, owner, guardMin);
                break;
            case MechanicType.DOUBLEDAMAGE:
                return new DoubleDamageMechanic(stack, owner, guardMin);
                break;
            case MechanicType.PANIC:
                return new PanicMechanic(stack, owner, guardMin);
            case MechanicType.HAUNT:
                return new HauntMechanic(stack, owner, guardMin);
            case MechanicType.EXPLODE:
                return new ExplodeMechanic(stack, owner);
            case MechanicType.ACIDIC:
                return new AcidicMechanic(stack, owner, guardMin);
            case MechanicType.ACIDICDOT:
                return new AcidicDotMechanic(stack, owner, guardMin);
            case MechanicType.ACIDICDOTTWO:
                return new AcidicDotTwoMechanic(stack, owner, guardMin);
            
            default:
                Debug.Log($"Unknown mechanic type {mechanicType.ToString()}");
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
        RemoveMechanic(fighter as IHaveMechanics, mechanicType);
    }
    public void RemoveMechanic(IHaveMechanics owner, MechanicType mechanicType)
    {
        if (owner != null && m_allMechanics.ContainsKey(owner))
        {
            m_allMechanics[owner].Remove(mechanicType);
        }
    }

    public void ReduceMechanic(Fighter fighter, MechanicType mechanicType, int amount)
    {
        ReduceMechanic(fighter as IHaveMechanics, mechanicType, amount);
    }
    public void ReduceMechanic(IHaveMechanics owner, MechanicType mechanicType, int amount)
    {
        if (owner != null && m_allMechanics.ContainsKey(owner))
        {
            m_allMechanics[owner].ReduceMechanic(mechanicType, amount);
        }
    }


    protected override void Init()
    {
        
    }

    public void RemoveAllMechancis(Fighter fighter)
    {
        RemoveAllMechancis(fighter as IHaveMechanics);
    }
    public void RemoveAllMechancis(IHaveMechanics owner)
    {
        if (m_allMechanics.ContainsKey(owner))
        {
            m_allMechanics[owner].RemoveAllMechanicsOfCategory(MechanicCategory.ALL);
        }
    }

    public void RemoveAllMechanicsOfCatigory(Fighter fighter, MechanicCategory mechanicCategory)
    {
        RemoveAllMechanicsOfCatigory(fighter as IHaveMechanics, mechanicCategory);
    }
    public void RemoveAllMechanicsOfCatigory(IHaveMechanics owner, MechanicCategory mechanicCategory)
    {
        if (m_allMechanics.ContainsKey(owner))
        {
            m_allMechanics[owner].RemoveAllMechanicsOfCategory(mechanicCategory);
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

    public int GetDebuffsCount(Fighter fighter)
    {
        if (fighter is IHaveMechanics owner)
        {
            return GetDebuffsCount(owner);
        }
        
        CustomDebug.LogWarning("Fighter was not of type IHaveMechanics", Categories.Mechanics.Root);
        return 0;
    }

    public int GetDebuffsCount(IHaveMechanics owner)
    {
        if (m_allMechanics.ContainsKey(owner))
        {
            return m_allMechanics[owner].GetDebuffsCount();
        }

        CustomDebug.LogWarning("Did not find mechanic owner", Categories.Mechanics.Root);
        return 0;
    }


}
