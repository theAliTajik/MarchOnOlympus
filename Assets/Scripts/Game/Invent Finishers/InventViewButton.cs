
using UnityEngine;
using UnityEngine.UI;

public class InventViewButton : MonoBehaviour
{
    public string Id;
    private string ToolTip;
    private bool m_isActive;
    private InventActionViewData m_data;
    
    [SerializeField] private Button m_button;

    public void Config(InventActionViewData data)
    {
        Id = data.Id;
        ToolTip = data.ToolTip;
        SetActive(data.Active);
        m_data = data;
    }

    public void SetActive(bool isActive)
    {
        SetActiveInternal(isActive);
    }

    public void SetActive()
    {
        SetActive(m_data.Active);
    }
    
    private void SetActiveInternal(bool isActive)
    {
        m_isActive = isActive;
        if (m_button != null)
            m_button.interactable = isActive;
    }    
    
    
    
}
