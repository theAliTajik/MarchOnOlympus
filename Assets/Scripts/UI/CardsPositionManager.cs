using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 
    CardsPositionManager : MonoBehaviour
{
    [SerializeField] private Vector3 m_centerPoint = new Vector3(0, 0, 0);
    [SerializeField] private float m_distance = 50;
    [SerializeField] private float m_angleAddition = 5;
    [SerializeField] private float m_normaldCardScale = 0.6f;
    [SerializeField] private float m_selectedCardScale = 0.75f;
    [SerializeField] private float m_selectedCardOffsetY;
    [SerializeField] private float m_centerCardOffsetY;
    [SerializeField] private float m_unselectedOffset;
    [SerializeField] private float m_speed;


    private List<CardDisplay> m_cards;
    private int m_selected = -1;
    private Vector3 m_defaultScale;
    private Vector3 m_selectedScale;

    public void SetCardsList(List<CardDisplay> cards)
    {
        m_defaultScale = Vector3.one * m_normaldCardScale;
        m_selectedScale = Vector3.one * m_selectedCardScale;

        m_cards = cards;
    }

    public void AddCardToList(CardDisplay card)
    {
        m_cards.Add(card);
    }

    public void SetSelectedIndex(int index)
    {
        if (m_cards == null || m_cards.Count <= 0)
        {
            return;
        }
        if (index != -1 && index < m_cards.Count)
        {
            Vector2 selectedPos = GetPosition(index);
            selectedPos.y = m_selectedCardOffsetY;
            m_cards[index].RectTransform.DOAnchorPos(selectedPos, 0.1f);
            m_cards[index].RectTransform.DOLocalRotate(Vector3.zero, 0.1f);
        }
        m_selected = index;
    }

    float m_center;
    float m_startX;
    private void Update()
    {
        if (m_cards == null || m_cards.Count <= 0)
        {
            return;
        }

        m_center = (m_cards.Count - 1) / 2f;
        m_startX = m_center * -m_distance;
        
        for (int i = 0; i < m_cards.Count; i++)
        {
            CardDisplay card = m_cards[i];
            Transform t = card.transform;


            bool isSelected = i == m_selected;


            float tt = (float)i / (m_cards.Count - 1);
            
            if (!isSelected)
            {
                Vector3 v = GetPosition(i);
                t.localPosition = Vector3.Lerp(t.localPosition, v, Time.deltaTime * m_speed);

                t.localScale = Vector3.Lerp(t.localScale, m_defaultScale, Time.deltaTime * m_speed);

                float angle = (i - m_center) * -m_angleAddition;
                t.localRotation = Quaternion.Lerp(t.localRotation, Quaternion.Euler(0, 0, angle), Time.deltaTime * m_speed);
            }
            else
            {
                t.localScale = Vector3.Lerp(t.localScale, m_selectedScale, Time.deltaTime * m_speed);
            }
        }
    }

    private Vector3 GetPosition(int index)
    {
        float distanceToCenter = m_center - Mathf.Abs(index - m_center);
        Vector3 v = new Vector3(m_startX + index * m_distance, m_centerPoint.y + distanceToCenter * m_centerCardOffsetY, m_centerPoint.z);

        if (m_selected != -1)
        {
            int distanceToSelected = index - m_selected;
            float sign = Mathf.Sign(distanceToSelected);
            float d = 1 * sign + (distanceToSelected * 0.2f);
            v.x += d * m_unselectedOffset;
        }
        return v;
    }
}