using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseTransition : MonoBehaviour
{
    public abstract bool IsIn();
    public abstract void ComeIn(System.Action callback);
    public abstract void GoOut(System.Action callback);
}