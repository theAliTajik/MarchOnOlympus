using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;


public class PerksDisplay : MonoBehaviour
{
    [SerializeField] private PerksDb m_perksDb;
    [SerializeField] private GameObject m_perksContainer;
    [SerializeField] private TMP_Text m_descText;
    [SerializeField] private GameObject m_popup;

    private List<PerksWidget> m_perksWidgets = new List<PerksWidget>();
    
    private void Awake()
    {
        m_popup.SetActive(false);
        PerksManager.Instance.OnPerkAdded += AddPerk;
        PerksManager.Instance.OnPerkRemoved += RemovePerk;
    }


    private void OnDestroy()
    {
        if (PerksManager.Instance != null)
        {
            PerksManager.Instance.OnPerkAdded -= AddPerk;
            PerksManager.Instance.OnPerkRemoved -= RemovePerk;
        }
    }

    public void AddAllPerks(List<string> perkIds)
    {
        for (var i = 0; i < perkIds.Count; i++)
        {
            AddPerk(perkIds[i]);
        }
    }

    private void AddPerk(string perkName)
    {
        if (perkName == null)
        {
            return;
        }

        PerksDb.PerksInfo perkInfo = m_perksDb.FindById(perkName);

        if (perkInfo == null)
        {
            Debug.Log("no such perk found: " + perkName);
            return;
        }

        if (perkInfo.Invisible)
        {
            return;
        }

        PerksWidget perk = PoolPerkWidget.Instance.GetItem();
        perk.transform.SetParent(m_perksContainer.transform);
        perk.transform.localScale = Vector3.one;
        perk.OnPointerEnter += OnPointerEnter;
        perk.OnPointerExit += OnPointerExit;
        perk.OnPointerClick += OnRemovePerkClicked;

        PerksSeperatorWidget perkSeperator = PoolPerkSeperatorWidget.Instance.GetItem();
        perkSeperator.transform.SetParent(m_perksContainer.transform);
        perkSeperator.transform.localScale = Vector3.one;

        perk.Config(perkInfo.ClientID, perkInfo.Icon, perkSeperator.gameObject);
        m_perksWidgets.Add(perk);
    }

    private void OnRemovePerkClicked(PerksWidget perk)
    {
        PerksManager.Instance.RemovePerk(perk.ClientID);
        RemovePerk(perk);
    }
    
    
    private void RemovePerk(PerksWidget perk)
    {
        Destroy(perk.MySeperator.gameObject);
        Destroy(perk.gameObject);
    }

    private void RemovePerk(string perkId)
    {
        PerksWidget perk = null;

        for (var i = 0; i < m_perksWidgets.Count; i++)
        {
            if (m_perksWidgets[i].ClientID == perkId)
            {
                perk = m_perksWidgets[i];
            }
        }
        
        if(perk != null)
        {
            RemovePerk(perk);
        }
        else
        {
            Debug.Log("did not find perk to remove: " + perkId);
        }
    }


    private void OnPointerEnter(PerksWidget widget)
    {
        widget.Highlight(true);

        PerksDb.PerksInfo info = PerksDb.Instance.FindById(widget.ClientID);
        m_descText.text = info.Desc;

        m_popup.transform.position = widget.transform.position;
        m_popup.SetActive(true);
    }

    private void OnPointerExit(PerksWidget widget)
    {
        widget.Highlight(false);
        m_popup.SetActive(false);
    }
}