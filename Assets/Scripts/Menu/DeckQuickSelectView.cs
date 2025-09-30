
using UnityEngine;

public class DeckQuickSelectView : ISelector<int>
{
    
    
    public override void StartSelect(SelectionData data)
    {
        
    }

    public void Select(int id)
    {
        RaiseOnSelect(id);
    }
}
