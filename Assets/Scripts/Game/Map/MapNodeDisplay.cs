using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MapNodeDisplay : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Action<MapNode> OnClick;
    
    private readonly Color m_nodeCompleteColor = Color.grey;
    private readonly Color m_nodeNormalColor = Color.white;
    private readonly Color m_nodeHighlightColor = Color.yellow;
    
    [SerializeField] private Image m_image;
    

    private MapNode m_node;
    private List<MapNodeDisplay> m_connectedNodes = new List<MapNodeDisplay>();
    
    private const float m_animDuration = 0.4f;
    
    public List<MapNodeDisplay> ConnectedNodes => m_connectedNodes;
    public MapNode Node => m_node;
    
    
    
    public void Configure(MapNode node, Sprite image)
    {
        m_node = node;
        m_image.sprite = image;
        
        if (node.IsComplete)
        {
            SetColor(m_nodeCompleteColor);
        }
        else
        {
            SetColor(m_nodeNormalColor);
        }

        if (node.IsSelectable)
        {
            SetColor(m_nodeHighlightColor);
            transform.DOScale(1.5f, m_animDuration).SetLoops(-1, LoopType.Yoyo);
        }
        else
        {
            DOTween.Kill(transform);
        }
    }

    public void SetColor(Color color)
    {
        m_image.color = color;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnClick?.Invoke(m_node);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (Node.IsComplete || !Node.IsSelectable)
        {
            return;
        }
        DOTween.Kill(transform);
        transform.DOScale(1.5f, m_animDuration);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (Node.IsComplete || !Node.IsSelectable)
        {
            return;
        }
        transform.localScale = Vector3.one;
        transform.DOScale(1.5f, m_animDuration).SetLoops(-1, LoopType.Yoyo);
    }
}
