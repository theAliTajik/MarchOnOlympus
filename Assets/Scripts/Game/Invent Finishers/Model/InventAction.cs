
using System;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class InventAction : ScriptableObject
{
    public string ID;
    public string ToolTip;
    public int InventLevel;
    public TargetType TargetType;

    public virtual void Execute(IDamageable target, Action finishCallBack)
    {
        
    }
}
;