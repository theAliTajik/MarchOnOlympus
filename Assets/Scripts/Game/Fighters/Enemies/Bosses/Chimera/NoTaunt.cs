
using UnityEngine;

public class NoTaunt : ITauntBehaviour
{
    public void ReceiveTaunt(Collider2D collider)
    {
        collider.enabled = true;
    }

    public void TurnChanged()
    {
    }
}
