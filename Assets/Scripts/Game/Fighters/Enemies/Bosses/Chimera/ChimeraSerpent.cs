
using System;
using UnityEngine;

[Serializable]
public class ChimeraSerpent : ChimeraHead, IHaveIntention, IHaveMechanics
{
    public ChimeraSerpent(Chimera mind)
    {
        m_mind = mind;
    }

    public override event Action<Intention, string> OnIntentionDetermined;
    
    [SerializeField] private ChimeraSerpentMoveData m_data;
    
    private MechanicsList m_mechanicsList;

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
            case "card":
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
            case "card":
                GameActionHelper.SpawnCard(m_data.Card, CardStorage.DRAW_PILE);
                finishCallBack?.Invoke();
                break;
            case "Stunned":
                finishCallBack?.Invoke();
                break;
        }
    }

    public MechanicsList GetMechanicsList()
    {
        return m_mechanicsList;
    }
}

