using System;
using Game;
using System.Collections;
using UnityEngine;

public class Chimera : BaseEnemy
{
	#region Animations

	private const string ANIM_01_IDLE = "01_Idle";
	private const string ANIM_02_WOUND = "02_Wound";
	private const string ANIM_03_IDLE_WOUND = "03_Idle_Wound";
	private const string ANIM_04_LION_HEAD_ATTACK = "04_Lion_head_Attack";
	private const string ANIM_05_LION_HEAD_GROW = "05_Lion_head_Grow";
	private const string ANIM_06_GOAT_HEAD_SHOUT_CRIT = "06_Goat_Head_Shout_Crit";
	private const string ANIM_06_GOAT_HEAD_SHOUT_NORMAL = "06_Goat_Head_Shout_Normal";
	private const string ANIM_08_SERPENT_HEAD = "08_Serpent_Head";
	private const string ANIM_09_DEATH_ = "09_Death_";

	#endregion

	[SerializeField] protected MoveData[] m_movesDatas;
    [SerializeField] private ChimeraMovesData m_data;

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
        m_animation.Play(ANIM_02_WOUND);
    }

    protected override void OnDeath()
    {
        if (CombatManager.Instance.IsGameOver)
        {
            return;
        }
        base.OnDeath();
        m_animation.Play(ANIM_09_DEATH_);
    }

    public override void DetermineIntention()
    {
        RandomIntentionPicker(m_moves);
        ShowIntention();
    }

    public override void ShowIntention()
    {
        base.ShowIntention();
        switch (m_nextMove.clientID)
        {
            case "0":
                //CallOnIntentionDetermined(Intention.CAST_DEBUFF, m_nextMove.description);
                break;
            case "1":

                break;
        }
    }

    public override void ExecuteAction(Action finishCallback)
    {
        // play intention
        base.ExecuteAction(finishCallback);

        Debug.Log("this action is played: " + m_nextMove.clientID);
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
            case "0":
                m_animation.Play(ANIM_04_LION_HEAD_ATTACK, finishCallback);
                break;
            case "1":
                m_animation.Play(ANIM_04_LION_HEAD_ATTACK, finishCallback);
				break;
        }

        yield return null;
    }

    public override void ConfigFighterHP()
    {
        m_fighterHP.SetMax(m_data.HP);
        m_fighterHP.ResetHP();
    }
}