
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventView : MonoBehaviour
{
    public event Action<string> OnClick;

    [SerializeField] private TMP_Text m_inventText;
    [SerializeField] private TMP_Text m_inventLevelText;

    [SerializeField] private FillBar m_inventFillBar;
    
    [SerializeField] private List<InventViewButton> m_buttons;

    private int m_maxInventLevel;
    private int m_inventToLevelConversionRate;

    public void Configure(int maxInventLevel, int inventToLevelConversionRate)
    {
        m_maxInventLevel = maxInventLevel;
        m_inventToLevelConversionRate = inventToLevelConversionRate;
        m_inventFillBar.Config(maxInventLevel * inventToLevelConversionRate);
    }

    public void ButtonClicked(int Button)
    {
        string id = m_buttons[Button].Id;
        OnClick?.Invoke(id);
    }
    
    public void DisableAllButtons()
    {
        foreach (InventViewButton button in m_buttons)
        {
            button.SetActive(false);
        }
    }

    public void EnableButton(int id)
    {
        
    }

    public void UpdateButtons(List<InventActionViewData> viewData)
    {
        if (viewData.Count > m_buttons.Count)
        {
            CustomDebug.LogWarning("More view Data than buttons was passed.", Categories.Combat.Invent.Root);
            int excess = viewData.Count - m_buttons.Count;
            viewData.RemoveRange(m_buttons.Count - 1, excess); 
        }

        for (int i = 0; i < viewData.Count; i++)
        {
            m_buttons[i].Config(viewData[i]);
        }
    }

    public void OnInventChanged(int invent)
    {
        m_inventText.text = invent.ToString();
        m_inventFillBar.SetValue(invent);
    }

    public void OnInventLevelChanged(int invent)
    {
        m_inventLevelText.text = invent.ToString();
    }

    public void ReEnableAllButtons()
    {
        foreach (InventViewButton button in m_buttons)
        {
            button.SetActive();
        }
    }
}
