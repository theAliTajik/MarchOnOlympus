using Game;
using System;
using System.Collections;
using UnityEngine;

public class FearMinion : BaseEnemy
{
        #region Anims

        private const string ANIM_ATTACK = "Attack";
        private const string ANIM_DEATH = "Death";
        private const string ANIM_HOWL = "Howl";
        private const string ANIM_IDDLE = "Iddle";
        private const string ANIM_WOUND = "Wound";

    #endregion
    
    
    [SerializeField] protected MoveData[] m_movesDatas;
    [SerializeField] private FearMinionMovesData m_data;

    private bool m_dead = false;
    public bool isDead => m_dead;

    public Action<FearMinion> OnDead;


	protected override void Awake()
    {
        base.Awake();

        ConfigFighterHP();

        SetMoves(m_movesDatas);
    }
    
    
    protected override void OnTookDamage(int damage, bool isCritical)
    {
        if (CombatManager.Instance.IsGameOver)
        {
            return;
        }
        base.OnTookDamage(damage, isCritical);
        m_animation.Play(ANIM_WOUND);
    }
    
    protected override void OnDeath()
    {
        if (CombatManager.Instance.IsGameOver)
        {
            return;
        }
        base.OnDeath();
        m_animation.Play(ANIM_DEATH);
        m_dead = true;

		OnDead?.Invoke(this);
	}
    
    
    public override void DetermineIntention()
    {
        RandomIntentionPicker();
        ShowIntention();
    }

    public override void ShowIntention()
    {
        base.ShowIntention();
        switch (m_nextMove.clientID)
        {
            case "Haunt":
                CallOnIntentionDetermined(Intention.CAST_DEBUFF, m_nextMove.description);
                break;
        }
    }

    public override void ExecuteAction(Action finishCallback)
    {
        // play intention
        base.ExecuteAction(finishCallback);

        //Debug.Log("this action is played: " + m_nextMove.clientID);
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
        switch (m_nextMove.clientID)
        {
            case "Haunt":
				yield return WaitForAnimation(ANIM_ATTACK, finishCallback);
                Debug.Log($"[{gameObject.name}] : Apply Haunt x{m_data.Move1Haunt} to Player");
				GameActionHelper.AddMechanicToFighter(GameInfoHelper.GetPlayer(), m_data.Move1Haunt, MechanicType.HAUNT);
				break;
        }
    }

    public override void ConfigFighterHP()
    {
        m_fighterHP.SetMax(m_data.HP);
        m_fighterHP.ResetHP();
    }
}