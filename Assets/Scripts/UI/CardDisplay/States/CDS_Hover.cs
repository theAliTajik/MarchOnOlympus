using UnityEngine;
using UnityEngine.EventSystems;

public class CDS_Hover : ICardStatePointerEvents, ICardStateDraggable
{
    
    
    public void OnPointerEnter()
    {
        // does nothing
    }

    public void OnPointerExit()
    {
        // change state to idle 
        // OnPointerExited?.Invoke(this);
    }

    public void OnBeginDrag()
    {
        //change state to dragging
    }

    public void OnDrag(PointerEventData eventData)
    {
        // does nothing
    }

    public void OnEndDrag()
    {
        // does nothing
    }
}       