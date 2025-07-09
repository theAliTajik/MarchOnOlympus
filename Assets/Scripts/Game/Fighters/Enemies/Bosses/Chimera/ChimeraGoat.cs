
using System;
using Game;
using UnityEngine;

[Serializable]
public class ChimeraGoat : ChimeraHead, IHaveIntention
{
    #region Animations
    
    private const string ANIM_06_GOAT_HEAD_SHOUT_CRIT = "06_Goat_Head_Shout_Crit";
    private const string ANIM_06_GOAT_HEAD_SHOUT_NORMAL = "06_Goat_Head_Shout_Normal";

    #endregion
    
    public ChimeraGoat(Chimera mind)
    {
        m_mind = mind;
    }

    public override event Action<Intention, string> OnIntentionDetermined;
    
    [SerializeField] private ChimeraGoatMoveData m_data;

    public override void Config()
    {
        // moves cycle
        BaseEnemy.MoveData[] cycle = new BaseEnemy.MoveData[3];
        cycle[0] = m_movesData[0];
        cycle[1] = m_movesData[0];
        cycle[2] = m_movesData[1];

        m_damageable = new EnemyDamageBehaviour();
        m_intentionDeterminer = IntentionDeterminerFactory.CreateDeterminer(IntentionDeterminerType.CYCLIC, cycle);

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
            case "hit":
                OnIntentionDetermined?.Invoke(Intention.ATTACK, m_nextMoveData.Value.description);
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
            case "fortify":
                return ANIM_06_GOAT_HEAD_SHOUT_NORMAL;
                
            case "hit":
                return ANIM_06_GOAT_HEAD_SHOUT_CRIT;
            
            default:
                return null;
        }
    }

    public override void ExecuteIntention(Action finishCallBack)
    {
        switch (m_nextMoveData.Value.clientID)
        {
            case "fortify":
                FortifySerpent();
                break;
            case "hit":
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

    private IHaveMechanics GetSerpentMechanics()
    {
        foreach (var head in m_mind.Heads)
        {
            if (head is ChimeraSerpent serpent)
            {
                return serpent;
            }
        }
        
        return null;
    }

    private void FortifySerpent()
    {
        IHaveMechanics serpent = GetSerpentMechanics();
        MechanicsManager.Instance.AddMechanic(1, MechanicType.FORTIFIED, serpent);
    }
}

