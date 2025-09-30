using System;
using System.Collections.Generic;
using UnityEngine;

public struct SelectableItemDisplayData
{
    public string Id;
    public string Text;
    public Sprite Sprite;
}

public struct SelectionData
{
    public SelectionData(IEnumerable<SelectableItemDisplayData> enumerable, bool efficientMode)
    {
        this.CurrentlySelected = new SelectableItemDisplayData();
        this.Enumerable = enumerable;
        this.EfficientMode = efficientMode;
    }
    
    public SelectionData(SelectableItemDisplayData currentlySelected, IEnumerable<SelectableItemDisplayData> enumerable, bool efficientMode)
    {
        this.CurrentlySelected = currentlySelected;
        this.Enumerable = enumerable;
        this.EfficientMode = efficientMode;
    }
    public SelectableItemDisplayData CurrentlySelected;
    public IEnumerable<SelectableItemDisplayData> Enumerable;
    public bool EfficientMode;
}

public abstract class ISelector<TID> : MonoBehaviour where TID : IComparable
{
    public event Action<TID> OnSelect;
    public abstract void StartSelect(SelectionData data);

    public virtual void UpdateData(SelectionData data)
    {
        
    }

    public virtual void StopSelect()
    {
        
    }
    
    public void RaiseOnSelect(TID id) => OnSelect?.Invoke(id);
}