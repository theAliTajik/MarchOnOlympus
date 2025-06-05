using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannibal : BaseEnemy
{
    #region Anims

    private const string ANIM_IDDLE = "Iddle";
    private const string ANIM_WOUNDED = "Wounded";
    private const string ANIM_EAT_ANIMATION = "Eat_animation";
    private const string ANIM_DEAD = "Dead";
    private const string ANIM_ATTACK = "Attack";

    #endregion
    
    [SerializeField] protected MoveData[] m_Leve1movesDatas;
    [SerializeField] protected MoveData[] m_Leve2movesDatas;
    [SerializeField] protected MoveData[] m_Leve3movesDatas;

    [SerializeField] private CannibalsMovesData m_data;

    private int m_level = 1;
    private int m_maxLevel = 3;
    
    public int Level
    {
        get { return m_level; }
    }

    
    protected override void Awake()
    {
        base.Awake();

        ConfigFighterHP();

        SetLevelData(m_level);
        CannibalsHelper.AddCannibal(this);
    }


    public void LevelUp(int foodLevel, Action finishCallBack)
    {
        m_animation.Play(ANIM_EAT_ANIMATION, finishCallBack);
        m_level++;
        if (foodLevel >= m_level)
        {
            m_level = foodLevel + 1;
        }

        if (m_level > m_maxLevel)
        {
            m_level = m_maxLevel;
        }
        SetLevelData(m_level);
    }

    private void SetLevelData(int level)
    {
        MoveData[] m_selectedMoves = null;
        switch (level)
        {
            case 1:
                m_selectedMoves = m_Leve1movesDatas;
                m_fighterHP.SetMax(m_data.HP);
                break;
            case 2:
                m_selectedMoves = m_Leve2movesDatas;
                m_fighterHP.SetMax(m_data.Level2HP);
                break;
            case 3:
                m_selectedMoves = m_Leve3movesDatas;
                m_fighterHP.SetMax(m_data.Level3HP);
                break;
            default:
                Debug.Log("invalid level for setting moves of cannibal");
                return;
                break;
        }
        m_fighterHP.ResetHP();
        
        m_moves.Clear();
        for (int i = 0; i < m_selectedMoves.Length; i++)
        {
            MoveData md = m_selectedMoves[i];
            m_moves.Add(md, md.chance);
        }
    }
    
    protected override void OnTookDamage(int damage, bool isCritical)
    {
        if (CombatManager.Instance.IsGameOver)
        {
            return;
        }
        base.OnTookDamage(damage, isCritical);
        m_animation.Play(ANIM_WOUNDED);
    }
    
    protected override void OnDeath()
    {
        if (CombatManager.Instance.IsGameOver)
        {
            return;
        }
        base.OnDeath();
        m_animation.Play(ANIM_DEAD);
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
            case "Hit":
                CallOnIntentionDetermined(Intention.ATTACK, m_nextMove.description);
                break;
            case "HitBoth":
                CallOnIntentionDetermined(Intention.MULTI_ATTACK, m_nextMove.description);
                break;
            case "HitLevel2":
                CallOnIntentionDetermined(Intention.ATTACK, m_nextMove.description);
                break;
            case "HitBothLevel2":
                CallOnIntentionDetermined(Intention.MULTI_ATTACK, m_nextMove.description);
                break;
            case "HitBothLevel3":
                CallOnIntentionDetermined(Intention.MULTI_ATTACK, m_nextMove.description);
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

        yield return new WaitForSeconds(0.2f);
        bool attackAnimFinished = false;
        switch (m_nextMove.clientID)
        {
            case "Hit":
                m_animation.Play(ANIM_ATTACK, finishCallback);
                GameActionHelper.DamageFighter(GameInfoHelper.GetPlayer(), this, m_data.Move1Damage);
                break;
            case "HitBoth":
                attackAnimFinished = false;
                m_animation.Play(ANIM_ATTACK, () => { attackAnimFinished = true;});
                GameActionHelper.DamageFighter(GameInfoHelper.GetPlayer(), this, m_data.Move2Damage);
                yield return new WaitUntil(() => attackAnimFinished);
                GameActionHelper.DamageFighter(this, this, m_data.Move2DamageToSelf, false);
                finishCallback?.Invoke();
                break;
            case "HitLevel2":
                m_animation.Play(ANIM_ATTACK, finishCallback);
                GameActionHelper.DamageFighter(GameInfoHelper.GetPlayer(), this, m_data.Move3Damage);
                break;
            case "HitBothLevel2":
                attackAnimFinished = false;
                m_animation.Play(ANIM_ATTACK, () => { attackAnimFinished = true;});
                GameActionHelper.DamageFighter(GameInfoHelper.GetPlayer(), this, m_data.Move4Damage);
                yield return new WaitUntil(() => attackAnimFinished);
                GameActionHelper.DamageFighter(this, this, m_data.Move4DamageToSelf);
                finishCallback?.Invoke();
                break;
            case "HitBothLevel3":
                attackAnimFinished = false;
                m_animation.Play(ANIM_ATTACK, () => { attackAnimFinished = true;});
                GameActionHelper.DamageFighter(GameInfoHelper.GetPlayer(), this, m_data.Move5Damage);
                
                yield return new WaitUntil(() => attackAnimFinished);
                GameActionHelper.DamageFighter(this, this, m_data.Move5DamageToSelf);
                finishCallback?.Invoke();
                break;
        }
    }
    
    public override void ConfigFighterHP()
    {
        m_fighterHP.SetMax(m_data.HP);
        m_fighterHP.ResetHP();
    }

}
