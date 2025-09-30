using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonTextContrastInvertor : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [SerializeField] private TMP_Text m_text;
    
    private Button m_button;

    private Color m_textStartColor;
    private Color m_textContrastColor;
    
    private void Start()
    {
        m_button = GetComponent<Button>();
        if (m_button == null)
        {
            Debug.Log($"WARNING: Missing Button: Button of object {this.gameObject.name} is missing");
            return;
        }

        if (m_text == null)
        {
            Debug.Log($"WARNING: Missing Text: Button Text Color Inverter of button: {m_button.name}");
            return;
        }
        m_textStartColor = m_text.color;
        m_textContrastColor = new Color(1 - m_text.color.r, 1 - m_text.color.g, 1 - m_text.color.b, m_text.color.a);
    }

    public void OnSelect(BaseEventData eventData)
    {
        if(m_text != null)
            m_text.color = m_textContrastColor;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        if(m_text != null)
            m_text.color = m_textStartColor;
    }
}
