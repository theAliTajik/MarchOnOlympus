using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class PerksWidget : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public event System.Action<PerksWidget> OnPointerEnter, OnPointerExit, OnPointerClick;

    [SerializeField] private Image m_icon;
    [SerializeField] private GameObject m_highlight;

    private string m_clientId;
    private GameObject m_mySeperator;

    public string ClientID => m_clientId;
    public GameObject MySeperator => m_mySeperator;


    public void Config(string clientId, Sprite icon, GameObject mySeperator)
    {
        m_clientId = clientId;
        m_icon.sprite = icon;
        m_mySeperator = mySeperator;
    }

    public void Highlight(bool value)
    {
        m_highlight.SetActive(value);
    }

    public void OnClick()
    {
        OnPointerClick(this);
    }
    
    
    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        OnPointerEnter?.Invoke(this);
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        OnPointerExit?.Invoke(this);
    }
}
