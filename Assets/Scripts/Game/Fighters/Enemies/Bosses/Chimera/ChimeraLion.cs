using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class ChimeraLion : IChimeraHead, IHaveIntention
{
    public ChimeraLion(Chimera mind)
    {
        m_mind = mind;
    }
    
    public event Action<BaseEnemy, int> OnDamaged;
    
    [SerializeField] private ChimeraLionMovesData m_data;
    [SerializeField] private BaseEnemy.MoveData[] m_movesData;
    
    private BaseEnemy.MoveData? m_nextMove;

    private int m_turnCycleCount = 0;
    private Chimera m_mind;

    public void Config(Chimera mind)
    {
        m_mind = mind;
    }

    // // public override int takedamage(int damage, fighter sender, bool doesreturntosender, bool isarmorpiercing = false)
    // {
    //     return m_mind.TakeDamage(damage, sender, doesReturnToSender, isArmorPiercing);
    // }
    
    public void DetermineIntention()
    {
        m_turnCycleCount++;
        if (m_turnCycleCount > 3)
        {
            m_turnCycleCount = 1;
        }

        switch (m_turnCycleCount)
        {
            case 1:
                m_nextMove = m_movesData[0];
                break;
            case 2:
                m_nextMove = m_movesData[0];
                break;
            case 3:
                m_nextMove = m_movesData[1];
                break;
        }
    }

    public void ShowIntention()
    {
        if (m_nextMove == null)
        {
            Debug.Log("ERROR: tried to show intention when next move was null");
            return;
        }

        switch (m_nextMove.Value.clientID)
        {
            case "hit":
                OnIntentionDetermined?.Invoke(Intention.ATTACK, m_nextMove.Value.description);
                break;
            case "taunt":
                OnIntentionDetermined?.Invoke(Intention.CAST_DEBUFF, m_nextMove.Value.description);
                break;
        }
    }

    public void ExecuteIntention(Action finishCallBack)
    {
        switch (m_nextMove.Value.clientID)
        {
            case "hit":
                GameActionHelper.DamageFighter(GameInfoHelper.GetPlayer(), m_mind, m_data.Move1Damage);
                finishCallBack?.Invoke();
                break;
            case "taunt":
                ChimeraSerpent serpent = m_mind.Serpent;
                ChimeraGoat goat = m_mind.Goat;
                
                serpent.Stun();
                goat.Stun();
                finishCallBack?.Invoke();
                break;
        }
    }

    public event Action<Intention, string> OnIntentionDetermined;
}
