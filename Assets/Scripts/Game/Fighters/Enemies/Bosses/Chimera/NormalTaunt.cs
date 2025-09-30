
using UnityEngine;

public class NormalTaunt : ITauntBehaviour
{
    private Collider2D m_collider;
    public void ReceiveTaunt(Collider2D collider)
    {
        collider.enabled = false;
        m_collider = collider;
    }

    public void TurnChanged()
    {
        if (m_collider != null) m_collider.enabled = true;
    }
}
