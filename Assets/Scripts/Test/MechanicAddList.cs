using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MechanicAddList : MonoBehaviour
{
    [SerializeField] private ClickableList m_list;
    [SerializeField] private IHaveMechanics m_owner;

    private bool m_isInitialized;

    private void Awake()
    {
        Button button = GetComponent<Button>();
        button.onClick.AddListener(ButtonClicked);
    }

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        if (m_isInitialized)
        {
            return;
        }

        foreach (MechanicsDb.MechanicInfo info in MechanicsDb.Instance.allMechanics)
        {
            m_list.AddItem(info.clientID, info.clientID, info.icon);
        }

        m_list.ItemClicked += ItemClicked;

        m_isInitialized = true;
    }

    public void Config(IHaveMechanics owner)
    {
        m_owner = owner;
    }

    private void ItemClicked(string clientId)
    {
        MechanicsDb.MechanicInfo info = MechanicsDb.Instance.FindById(clientId);
        BaseMechanic mechanic = GetMechanic(info);

        MechanicsManager.Instance.AddMechanic(mechanic);
        m_list.gameObject.SetActive(false);
    }

    private void ButtonClicked()
    {
        m_list.gameObject.SetActive(true);
    }


    private int m_mechanicStack = 1;
    private BaseMechanic GetMechanic(MechanicsDb.MechanicInfo info)
    {
        switch (info.type)
        {
            default:
            case Game.MechanicType.STRENGTH:
                return new StrenghtMechanic(m_mechanicStack, m_owner);
            case Game.MechanicType.BLOCK:
                return new BlockMechanic(m_mechanicStack, m_owner);
            case Game.MechanicType.FORTIFIED:
                return new FortifiedMechanic(m_mechanicStack, m_owner);
            case Game.MechanicType.DEXTERITY:
                return new DexterityMechanic(m_mechanicStack, m_owner);
            case Game.MechanicType.THORNS:
                return new ThornsMechanic(m_mechanicStack, m_owner);
            case Game.MechanicType.FRENZY:
                return new FrenzyMechanic(m_mechanicStack, m_owner);
            case Game.MechanicType.IMPALE:
                return new ImpaleMechanic(m_mechanicStack, m_owner);
            case Game.MechanicType.BLEED:
                return new BleedMechanic(m_mechanicStack, m_owner);
            case Game.MechanicType.BURN:
                return new BurnMechanic(m_mechanicStack, m_owner);
            case Game.MechanicType.DAZE:
                return new DazeMechanic(m_mechanicStack, m_owner);
            case Game.MechanicType.STUN:
                return new StunMechanic(m_mechanicStack, m_owner);
            case Game.MechanicType.VULNERABLE:
                return new VulnerableMechanic(m_mechanicStack, m_owner);
            case Game.MechanicType.EXPLODE:
                return new ExplodeMechanic(m_mechanicStack, m_owner);

                // not implemented yet
            case Game.MechanicType.INGENIUS:
                return new BlockMechanic(1, m_owner);
            case Game.MechanicType.IMPROVISE:
                return new BlockMechanic(1, m_owner);
        }
    }
}
