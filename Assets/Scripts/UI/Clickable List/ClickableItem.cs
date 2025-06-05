using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClickableItem : MonoBehaviour
{
    [SerializeField] private TMP_Text m_text;
    [SerializeField] private Image m_image;
    [SerializeField] private Button m_button;


    private string m_clientId;
    private System.Action<string> m_callback;


    private void Awake()
    {
        if (m_button != null )
        {
            m_button.onClick.AddListener(Clicked);
        }
    }

    public void Config(string clientId, string text, Sprite image, System.Action<string> callback)
    {
        m_clientId = clientId;
        if (m_text != null)
        {
            m_text.text = text;
        }

        if (m_image != null)
        {
            m_image.sprite = image;
        }

        m_callback = callback;
    }

    private void Clicked()
    {
        m_callback?.Invoke(m_clientId);
    }

}
