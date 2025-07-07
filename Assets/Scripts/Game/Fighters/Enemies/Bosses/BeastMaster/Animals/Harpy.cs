using Game;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Harpy : BaseEnemy
{
    #region Animations

    private const string ANIM_ATTACK = "Attack";
    private const string ANIM_DEATH = "Death";
    private const string ANIM_HOWL = "Howl";
    private const string ANIM_IDDLE = "Iddle";
    private const string ANIM_WOUND = "Wound";

    #endregion

    [SerializeField] protected MoveData[] m_movesDatas;
    [SerializeField] protected MoveData[] m_movesDatasAirbone;
    [SerializeField] private HarpyMovesData m_data;

    private List<HarpyMinion> m_minions = new List<HarpyMinion>();
    private int m_turnCounter = 0;
    private bool m_isOnAirbone = false, m_take100PercentDamage = false;

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
            Debug.Log("Airbone at 66");
            m_turnCounter = 0;
            SetAirbone(true);
            ReleaseAnimal();
        }

        if (percentage == m_data.Phase2HPPercentageTrigger)
        {
            Debug.Log("Screech at 33");
            Screech();
        }

        Debug.Log("percentage triggered: :" + percentage.Percentage);
    }

    protected override void OnTookDamage(int damage, bool isCritical)
    {
        if (CombatManager.Instance.IsGameOver)
        {
            return;
        }

		if (m_take100PercentDamage)
		{
            damage *= 2;
			m_take100PercentDamage = false;
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
        if (m_isOnAirbone)
        {
            m_turnCounter++;
            Debug.Log("Turn OnAirbone: " + m_turnCounter);

            if (m_turnCounter < 3)
            {
                m_nextMove = m_moves[0];
                ShowIntention();
                return;
            }
            else
                SetAirbone(false);
        }

        RandomIntentionPicker(m_moves);
        ShowIntention();
    }

    public override void ShowIntention()
    {
        base.ShowIntention();

        switch (m_nextMove.clientID)
        {
            case "HitBleed":
                CallOnIntentionDetermined(Intention.ATTACK, m_nextMove.description);
                break;
            case "ScreechDaze":
                CallOnIntentionDetermined(Intention.CAST_DEBUFF, m_nextMove.description);
                break;

            case "Airbone":
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
            case "HitBleed":
                m_animation.Play(ANIM_HOWL, finishCallback);
                Fighter player = GameInfoHelper.GetPlayer();
                GameActionHelper.DamageFighter(player, this, m_data.Move1Damage);
                GameActionHelper.AddMechanicToFighter(this, m_data.Move1Bleed, MechanicType.BLEED);
                break;
            case "ScreechDaze":
                Screech();
                finishCallback?.Invoke();
                break;

            case "Airbone":
                m_animation.Play(ANIM_WOUND, finishCallback);
                break;
        }
    }

    public override void ConfigFighterHP()
    {
        m_fighterHP.SetMax(m_data.HP);
        m_fighterHP.ResetHP();
    }

    public void ReleaseAnimal()
    {
        for (int i = 0; i < 2; i++)
        {
            HarpyMinion minion = EnemiesManager.Instance.SpawnBoss("HarpyMinion")[0] as HarpyMinion;

            if (minion != null)
            {
                m_minions.Add(minion);
                minion.DetermineIntention();
            }
        }
    }

    private void SetAirbone(bool value)
    {
        m_isOnAirbone = value;
        SetCanBeTarget(!value);
        SetMoves(value ? m_movesDatasAirbone : m_movesDatas);

        if (!value)
            AirbonDone(IsMinionsDead());
	}

    private void Screech()
    {
        m_animation.Play(ANIM_ATTACK);
        GameActionHelper.AddMechanicToFighter(this, m_data.Move2Daze, MechanicType.DAZE);
    }

    private bool IsMinionsDead()
    {
        bool IsMinionsDead = true;

        foreach (HarpyMinion m in m_minions)
        {
            if (!m.isDead)
                IsMinionsDead = false;
        }

        return IsMinionsDead;
    }

    private void AirbonDone(bool minionsDead)
    {
        if (minionsDead) //100% Damage
        {
            m_take100PercentDamage = true;
			return;
        }

        //40 Damage to player
        Fighter player = GameInfoHelper.GetPlayer();
        GameActionHelper.DamageFighter(player, this, 40);
    }
}