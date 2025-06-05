using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public interface ICardStateEnterExit
{
    void Enter();
    void Exit();
}

public interface ICardStatePointerEvents
{
    void OnPointerEnter();
    void OnPointerExit();
}

public interface ICardStateDraggable
{
    void OnBeginDrag();
    void OnDrag(PointerEventData eventData);
    void OnEndDrag();
}

public interface ICardStateClickable
{
    void OnClick();
}
