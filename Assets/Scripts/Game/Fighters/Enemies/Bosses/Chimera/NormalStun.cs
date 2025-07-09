
using UnityEngine;

public class NormalStun : IChimeraHeadStunBehaviour
{
    private int m_damageThreshold;
    private int m_damageThisTurn;

    public NormalStun(int damageThreshold)
    {
        m_damageThreshold = damageThreshold;
    }
    
    public bool Stun(int damage, ChimeraHead head)
    {
        m_damageThisTurn += damage;
        // Debug.Log($"got damaged. total:{m_damageThisTurn}");

        if (m_damageThisTurn >= m_damageThreshold)
        {
            head.Stun();
            return true;
        }
        
        return false;
    }

    public void TurnChanged()
    {
        // Debug.Log("reset damage");
        m_damageThisTurn = 0;
    }
}
