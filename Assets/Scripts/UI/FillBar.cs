
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FillBar : MonoBehaviour
{
    [SerializeField] private Vector3 m_offset;
    
    [SerializeField] private Image m_bar;
    [SerializeField] private RectTransform m_barCap;
    [SerializeField] private TMP_Text m_currentText;
    [SerializeField] private TMP_Text m_maxText;

    private int m_lastHealth;
    
    private int m_max;
    private int m_current;


    public void Config(int max)
    {
        setMaxHealth(max);
    }
    
    public void SetValue(int value)
    {
        m_current = value;
        m_bar.fillAmount = (float)m_max / m_current;
        if (m_barCap != null)
        {
            Vector2 v = m_barCap.anchoredPosition;
            v.x = m_bar.rectTransform.rect.width * m_bar.fillAmount;
        }

        m_currentText.text = m_current.ToString();
        m_maxText.text = m_max.ToString();
    }

    public void setMaxHealth(int value)
    {
        m_max = value;
    }
    
    public Vector3 GetOffset()
    {
        return m_offset;
    }
}
