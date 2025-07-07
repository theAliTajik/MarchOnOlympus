using UnityEngine;

public interface ITauntBehaviour
{
    public void ReceiveTaunt(Collider2D collider);
    
    public void TurnChanged();
}