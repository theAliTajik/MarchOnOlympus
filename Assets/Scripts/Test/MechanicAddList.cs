using System;
using System.Collections;
using System.Collections.Generic;
using Game;
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
            SelectableItemDisplayData m = new SelectableItemDisplayData()
            {
                Id = info.clientID,
                Text = info.clientID,
                Sprite = info.icon
            };
            m_list.AddItem(m);
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
        // m_list.gameObject.SetActive(false);
    }

    private void ButtonClicked()
    {
        m_list.gameObject.SetActive(true);
    }


    private int m_mechanicStack = 1;
    private BaseMechanic GetMechanic(MechanicsDb.MechanicInfo info)
    {
        return MechanicsManager.CreateMechanicOfType(m_mechanicStack,info.type, m_owner);
    }
}
