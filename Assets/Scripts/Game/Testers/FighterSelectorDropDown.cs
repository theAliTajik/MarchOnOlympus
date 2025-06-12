using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public struct ChildData
{
    public string Name;
    public Fighter Fighter;

    public ChildData(string name, Fighter fighter)
    {
        Name = name;
        Fighter = fighter;
    }
}

public class FighterSelectorDropDown : MonoBehaviour
{
    public TMP_Dropdown m_dropdown;
    

    private List<ChildData> m_childrenWithFighter = new List<ChildData>();

    public void Start()
    {
        GrabFighters();
        UpdateDropdown();
    }

    public void Refresh()
    {
        GrabFighters();
        UpdateDropdown();
    }

    public void GrabFighters()
    {
        m_childrenWithFighter.Clear();
        m_childrenWithFighter.Add(new ChildData("Player", GameInfoHelper.GetPlayer()));

        List<Fighter> allEnemies = GameInfoHelper.GetAllEnemies();
        foreach (Fighter enemy in allEnemies)
        {
            m_childrenWithFighter.Add(new ChildData(enemy.name, enemy));
        }
    }

    public void UpdateDropdown()
    {
        m_dropdown.ClearOptions();
        
        
        foreach (ChildData child in m_childrenWithFighter)
        {
            m_dropdown.options.Add(new TMP_Dropdown.OptionData(child.Name));
        }

        // Refresh the dropdown to ensure the new options are displayed
        m_dropdown.RefreshShownValue();
        
    }

    public Fighter GetSelectedFighter()
    {
        Fighter selectedFighter = m_childrenWithFighter[m_dropdown.value].Fighter;
        GrabFighters();
        UpdateDropdown();
        return selectedFighter;
    } 
}
