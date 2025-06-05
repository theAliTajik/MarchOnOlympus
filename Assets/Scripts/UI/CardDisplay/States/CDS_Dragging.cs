using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class CDS_Dragging : ICardStateDraggable
{
    private CardDisplay m_cardDisplay;

    private RectTransform m_rectTransform;
    private float m_canvasScaleFactor;
    private float m_selectY;
    
    
    public void Configure(global::CardDisplay context)
    {
        m_cardDisplay = context;
        m_rectTransform = m_cardDisplay.RectTransform;
        m_canvasScaleFactor = m_cardDisplay.CanvasScaleFactor;
        m_selectY = m_cardDisplay.SelectY;
    }


    public void OnBeginDrag() {}

    public void OnDrag(PointerEventData eventData)
    {
        TargetType currentTargetingType = m_cardDisplay.CardInDeck.CurrentState.GetTargetingType(); 
        switch (currentTargetingType)
        {
            case TargetType.PLAYER:
                m_rectTransform.anchoredPosition += eventData.delta / m_canvasScaleFactor;
                if (m_rectTransform.anchoredPosition.y >= m_selectY)
                {
                    if (m_cardDisplay.ManaIsEnough)
                    {
                        //change state to selected
                        //OnPlay?.Invoke(this);
                    }
                } 
                // else if (m_cardState == CardState.SELECTED && RectTransform.anchoredPosition.y <= m_selectY)
                // {
                //     OnDeselect?.Invoke(this);
                // }
                break;
            case TargetType.PLAYER_ENEMY:
            case TargetType.ENEMY:
                if (m_rectTransform.anchoredPosition.y >= m_selectY)
                {
                    if (m_cardDisplay.ManaIsEnough)
                    {
                        //change card state to selected
                        
                        //OnPlay?.Invoke(this);
                        
                        m_cardDisplay.RectTransform.DOAnchorPos(new Vector2(30, 250), 0.1f);
                        return;
                    }
                    //OnPlay?.Invoke(this);
                }
                m_cardDisplay.RectTransform.anchoredPosition += eventData.delta / m_canvasScaleFactor;
                break;
        }
    }


    public void OnEndDrag()
    {
        // switch to another state
    }
}