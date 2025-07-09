
using System;
using UnityEngine;

[Serializable]
public class ChimeraSerpent : ChimeraHead, IHaveIntention, IHaveMechanics
{
    #region Animation
    
	private const string ANIM_08_SERPENT_HEAD = "08_Serpent_Head";

    #endregion
    
    public ChimeraSerpent(Chimera mind)
    {
        m_mind = mind;
    }

    public override event Action<Intention, string> OnIntentionDetermined;
    
    [SerializeField] private ChimeraSerpentMoveData m_data;
    
    private MechanicsList m_mechanicsList;

    public override void Config()
    {
        m_damageable = new EnemyDamageBehaviour(this);
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
        if (m_nextMoveData == null)
        {
            Debug.Log("ERROR: tried to get animation when next move was null");
            return null;
        }

        switch (m_nextMoveData.Value.clientID)
        {
            case "card":
                return ANIM_08_SERPENT_HEAD;
                break;
                
            default:
                return null;
        }
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

