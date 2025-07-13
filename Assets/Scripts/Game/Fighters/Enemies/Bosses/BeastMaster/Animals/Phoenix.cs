using Game;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phoenix : BaseEnemy
{
    #region Animations

    private const string ANIM_ATTACK = "Attack";
    private const string ANIM_DEATH = "Death";
    private const string ANIM_HOWL = "Howl";
    private const string ANIM_IDDLE = "Iddle";
    private const string ANIM_WOUND = "Wound";

    #endregion

    [SerializeField] protected MoveData[] m_movesDatas;
    [SerializeField] private PhoenixMovesData m_data;

    protected override void Awake()
    {
        base.Awake();

        ConfigFighterHP();

        for (int i = 0; i < m_movesDatas.Length; i++)
        {
            MoveData md = m_movesDatas[i];
            m_moves.Add(md, md.chance);
        }
    }

    private void Start()
    {
        HP.SetTrigger(m_data.Phase1HPPercentageTrigger);
        HP.SetTrigger(m_data.Phase2HPPercentageTrigger);
        HP.OnPercentageTrigger += OnHPPercentageTriggred;
    }

    private void OnHPPercentageTriggred(FighterHP.TriggerPercentage percentage)
    {
        if (percentage == m_data.Phase1HPPercentageTrigger)
        {
			Debug.Log("--> [Phoenix] %66 Hit 50 to player, Restore 50");
			GameActionHelper.DamageFighter(GameInfoHelper.GetPlayer(), this, m_data.At66Damage);
            Heal(m_data.At66Restore);
		}

		if (percentage == m_data.Phase2HPPercentageTrigger)
        {
			Debug.Log("--> [Phoenix] %33 Hit 50 to player, Use ability Restore");
			GameActionHelper.DamageFighter(GameInfoHelper.GetPlayer(), this, m_data.At33Damage);
			Heal(m_data.At33Restore);
		}

        Debug.Log("percentage triggered: :" + percentage.Percentage);
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
            case "BurnDaze":
                CallOnIntentionDetermined(Intention.CAST_DEBUFF, m_nextMove.description);
                break;
            case "BurnRestore":
                CallOnIntentionDetermined(Intention.CAST_DEBUFF, m_nextMove.description);
                break;
            case "BlockRestore":
                CallOnIntentionDetermined(Intention.BLOCK, m_nextMove.description);
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
            case "BurnDaze":
                m_animation.Play(ANIM_HOWL, finishCallback);
				Fighter player = GameInfoHelper.GetPlayer();
				GameActionHelper.AddMechanicToFighter(player, m_data.Move1Burn, MechanicType.BURN);
				GameActionHelper.AddMechanicToFighter(player, m_data.Move1Daze, MechanicType.DAZE);
				Debug.Log($"--> [Phoenix] | BURN x{m_data.Move1Burn} | DAZE x{m_data.Move1Daze}");
				break;

            case "BurnRestore":
                m_animation.Play(ANIM_HOWL, finishCallback);
				GameActionHelper.AddMechanicToFighter(GameInfoHelper.GetPlayer(), m_data.Move2Burn, MechanicType.BURN);
                Heal(m_data.Move2Restore);
                Debug.Log($"--> [Phoenix] | BURN x{m_data.Move2Burn} | Heal +{m_data.Move2Restore}");
				break;

            case "BlockRestore":
                m_animation.Play(ANIM_WOUND, finishCallback);
				GameActionHelper.AddMechanicToFighter(this, m_data.Move3Block, MechanicType.BLOCK);
				Heal(m_data.Move3Restore);
				Debug.Log($"--> [Phoenix] | BLOCK x{m_data.Move3Block} | Heal +{m_data.Move3Restore}");
				break;
        }
    }

    public override void ConfigFighterHP()
    {
        m_fighterHP.SetMax(m_data.HP);
        m_fighterHP.ResetHP();
    }
}