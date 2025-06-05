
using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DevHud : MonoBehaviour
{
    public TMP_Dropdown m_mechanicsDropdown;
    public FighterSelectorDropDown m_fighterSelectorDropDown;
    
    [SerializeField] private Toggle m_infiniteCdToggle;
    [SerializeField] private Toggle m_allowStanceChangeToggle;

    private Fighter m_FighterToApplyTo;

    public void OnAddMechanicButtonClicked()
    {
        BaseMechanic mechanic = ReadSelectedMechanic();
        m_FighterToApplyTo = m_fighterSelectorDropDown.GetSelectedFighter();
        MechanicsManager.Instance.AddMechanic(mechanic);    
    }
    
    public void OnReduceMechanicButtonClicked()
    {
        BaseMechanic mechanic = ReadSelectedMechanic();
        m_FighterToApplyTo = m_fighterSelectorDropDown.GetSelectedFighter();
        MechanicsManager.Instance.ReduceMechanic(m_FighterToApplyTo, mechanic.GetMechanicType(), 5);    
    }
    
    public BaseMechanic ReadSelectedMechanic()
    {
        int selectedIndex = m_mechanicsDropdown.value;
        string selectedText = m_mechanicsDropdown.options[selectedIndex].text;

        Debug.Log("Selected Text: " + selectedText);


        switch (selectedText)
        {
            case "Strenght":
                return new StrenghtMechanic(5, m_FighterToApplyTo);

            case "Block":
                return new BlockMechanic(5, m_FighterToApplyTo);

            case "Fortified":
                return new FortifiedMechanic(1, m_FighterToApplyTo);

            case "Dexterity":
                return new DexterityMechanic(5, m_FighterToApplyTo);

            case "Thorns":
                return new ThornsMechanic(5, m_FighterToApplyTo);

            case "Frenzy":
                return new FrenzyMechanic(5, m_FighterToApplyTo);

            case "Impale":
                return new ImpaleMechanic(5, m_FighterToApplyTo);

            case "Bleed":
                return new BleedMechanic(5, m_FighterToApplyTo);

            case "Burn":
                return new BurnMechanic(5, m_FighterToApplyTo);

            case "Daze":
                return new DazeMechanic(5, m_FighterToApplyTo);

            case "Stun":
                return new StunMechanic(1, m_FighterToApplyTo);

            case "Vulnerable":
                return new VulnerableMechanic(5, m_FighterToApplyTo);

            default:
                throw new ArgumentException($"Unknown mechanic type: {selectedText}");
                return new StrenghtMechanic(5, m_FighterToApplyTo);
                break;
        }
    }

    public void OnNavBarButtonClicked()
    {
        //Hide all menu items
    }

    public void OnSetInfiniteCdToggleClicked()
    { 
        CombatManager.Instance.StanceMachine.SetInfiniteCd(m_infiniteCdToggle.isOn);
    }

    public void OnSetAllowStanceChangeToggleClicked()
    {
        CombatManager.Instance.StanceMachine.SetStanceChangeAllowed(m_allowStanceChangeToggle.isOn);
    }

}

