using System;
using Game;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Cyclops : BaseEnemy
{
	#region Animations

	private const string ANIM_001_DANCE = "001_Dance";
	private const string ANIM_DEATH = "Death";
	private const string ANIM_IDLE = "Idle";
	private readonly string[] ANIM_ATTACK123 = new string[] { "Attack", "Attack2", "Attack3", "Attack_4_1", "Attack_4_2" };
	private readonly string[] ANIM_WOUNDEDS = new string[] { "Wounded1", "Wounded2", "Wounded3" };

	/*private const string ANIM_ATTACK_4_1 = "Attack_4_1";
	private const string ANIM_ATTACK_4_2 = "Attack_4_2";
	private const string ANIM_WOUNDED1 = "Wounded1";
    private const string ANIM_WOUNDED2 = "Wounded2";
	private const string ANIM_WOUNDED3 = "Wounded3";*/

	#endregion

	[SerializeField] protected MoveData[] m_movesDatas;
    [SerializeField] private CyclopsMovesData m_data;
    private int m_turnCounter = 0;

    protected override void Awake()
    {
        base.Awake();

        for (int i = 0; i < m_movesDatas.Length; i++)
        {
            MoveData md = m_movesDatas[i];
            m_moves.Add(md, md.chance);
        }
        
        ConfigFighterHP();
    }

    protected override void OnTookDamage(int damage, bool isCritical)
    {
        if (CombatManager.Instance.IsGameOver)
        {
            return;
        }
        base.OnTookDamage(damage, isCritical);
        m_animation.Play(RandomWOUNDEDClip());
    }

    protected override void OnDeath()
    {
        if (CombatManager.Instance.IsGameOver)
        {
            return;
        }
        base.OnDeath();
        m_animation.Play(ANIM_DEATH);
    }

    public override void DetermineIntention()
    {
        m_turnCounter++;
        if (m_turnCounter > 4)
        {
            m_nextMove = m_moves[2];
            m_turnCounter = 0;
            ShowIntention();
            return;
        }
        
        
        RandomIntentionPicker(m_moves);
        ShowIntention();
    }

    public override void ShowIntention()
    {
        base.ShowIntention();
        switch (m_nextMove.clientID)
        {
            case "HitBuff":
                CallOnIntentionDetermined(Intention.ATTACK, m_nextMove.description);
                break;
            case "Hit":
                CallOnIntentionDetermined(Intention.ATTACK, m_nextMove.description);
                break;
            case "RemoveAllDebuffs":
                CallOnIntentionDetermined(Intention.BUFF, m_nextMove.description);
                break;
        }
    }

	public override void ExecuteAction(Action finishCallback)
    {
        // play intention
        base.ExecuteAction(finishCallback);

        // Debug.Log("this action is played: " + m_nextMove.clientID);
        StartCoroutine(WaitAndExecute(finishCallback));
    }
    
    private IEnumerator WaitAndExecute(Action finishCallback)
    {
        if (m_stuned)
        {
            m_stuned = false;
            finishCallback?.Invoke();
            yield break;
        }

		Fighter player = GameInfoHelper.GetPlayer();

        switch (m_nextMove.clientID)
        {
            case "HitBuff":
                //Hit
                for (int i = 1; i <= m_data.Move1NumOfAttacks; i++)
                {
                    yield return WaitForAnimation(RandomAttackClip());
                    GameActionHelper.DamageFighter(player, this, m_data.Move1Damage);
                }

                //Remove all buffs on player
                GameActionHelper.RemoveAllMechanicOfCategory(player, MechanicCategory.BUFF);

                finishCallback?.Invoke();
                break;
            case "Hit":
                for (int i = 1; i <= m_data.Move2NumOfAttacks; i++)
                {
                    yield return WaitForAnimation(RandomAttackClip());
                    GameActionHelper.DamageFighter(player, this, m_data.Move2Damage);
                }
                finishCallback?.Invoke();
                break;
            case "RemoveAllDebuffs":
                //Every 4th turn, removes all debuffs on (This)
                GameActionHelper.RemoveAllMechanicOfCategory(this, MechanicCategory.DEBUFF);
                
                //Fortify
                GameActionHelper.AddMechanicToFighter(this, m_data.Move3Fortify, MechanicType.FORTIFIED);
                break;
        }

        yield return null;
    }

    public override void ConfigFighterHP()
    {
        m_fighterHP.SetMax(m_data.HP);
        m_fighterHP.ResetHP();
    }

    private string RandomAttackClip()
    {
        int rand = Random.Range(0, ANIM_ATTACK123.Length);
        return ANIM_ATTACK123[rand];
    }

	private string RandomWOUNDEDClip()
	{
		int rand = Random.Range(0, ANIM_WOUNDEDS.Length);
		return ANIM_WOUNDEDS[rand];
	}
}