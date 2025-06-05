using UnityEngine;

public class CDS_Idle : ICardStatePointerEvents
{
    public void OnPointerEnter()
    {
        //change state to hover
        // OnPointerEntered?.Invoke(this)
    }

    public void OnPointerExit()
    {
        // does nothing
    }
}        