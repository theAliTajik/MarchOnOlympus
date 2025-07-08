using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class ChimeraLion : ChimeraHead
{
    public ChimeraLion(Chimera mind)
    {
        m_mind = mind;
    }

    public override event Action<Intention, string> OnIntentionDetermined;
    
    [SerializeField] private ChimeraLionMovesData m_data;

    public override void Config()
    {
        // moves cycle
        BaseEnemy.MoveData[] cycle = new BaseEnemy.MoveData[3];
        cycle[0] = m_movesData[0];
        cycle[1] = m_movesData[0];
        cycle[2] = m_movesData[1];

        m_intentionDeterminer = IntentionDeterminerFactory.CreateDeterminer(IntentionDeterminerType.CYCLIC, cycle);

        m_stun = new NormalStun(m_data.DamageThresholdForStun);
        m_taunt = new NoTaunt();
    }
    
    public override void ShowIntention()
    {
        if (m_nextMoveData == null)
        {
            Debug.Log("ERROR: tried to show intention when next move was null");
            return;
        }

        switch (m_nextMoveData.Value.clientID)
        {
            case "hit":
                OnIntentionDetermined?.Invoke(Intention.ATTACK, m_nextMoveData.Value.description);
                break;
            case "taunt":
                OnIntentionDetermined?.Invoke(Intention.CAST_DEBUFF, m_nextMoveData.Value.description);
                break;
            case "Stunned":
                OnIntentionDetermined?.Invoke(Intention.STUNED, "Stunned");
                break;
        }
    }

    public override string GetAnimation()
    {
        return null;
    }

    public override void ExecuteIntention(Action finishCallBack)
    {
        switch (m_nextMoveData.Value.clientID)
        {
            case "hit":
                GameActionHelper.DamageFighter(GameInfoHelper.GetPlayer(), m_mind, m_data.Move1Damage);
                finishCallBack?.Invoke();
                break;
            case "taunt":
                m_mind.TauntHeads();
                
                finishCallBack?.Invoke();
                break;
            case "Stunned":
                finishCallBack?.Invoke();
                break;
        }
    }

}
