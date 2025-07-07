
using System;
using UnityEngine;

[Serializable]
public class ChimeraGoat : ChimeraHead, IHaveIntention
{
    public ChimeraGoat(Chimera mind)
    {
        m_mind = mind;
    }

    public override event Action<Intention, string> OnIntentionDetermined;
    
    [SerializeField] private ChimeraGoatMoveData m_data;

    public override void Config()
    {
        m_intentionDeterminer = IntentionDeterminerFactory.CreateDeterminer(IntentionDeterminerType.RANDOM, m_movesData);

        m_stun = new NormalStun(m_data.DamageThresholdForStun);
        m_taunt = new NormalTaunt();
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
            case "fortify":
                OnIntentionDetermined?.Invoke(Intention.BUFF, m_nextMoveData.Value.description);
                break;
            case "poison":
                OnIntentionDetermined?.Invoke(Intention.ATTACK, m_nextMoveData.Value.description);
                break;
            case "Stunned":
                OnIntentionDetermined?.Invoke(Intention.STUNED, "Stunned");
                break;
        }
    }

    public override void ExecuteIntention(Action finishCallBack)
    {
        switch (m_nextMoveData.Value.clientID)
        {
            case "fortify":
                Debug.Log("TODO: send fortify to serpent");
                break;
            case "poison":
                int numOfPoisonCards = GameInfoHelper.CountCardsWithName(m_data.Move2PoisonCard.Name, CardStorage.ALL);
                
                if (numOfPoisonCards <= 0)
                {
                    break;
                }

                int damage = numOfPoisonCards * m_data.Move2PoisonHitMultiplier;
                GameActionHelper.DamageFighter(GameInfoHelper.GetPlayer(), m_mind, damage);
                break;
            case "Stunned":
                break;
        }
        
        finishCallBack?.Invoke();
    }
}

