using System;
using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AssassinClone : BaseEnemy
{
    #region Animations
    
    private const string ANIM_01_IDLE = "01_idle";
    private const string ANIM_02_WOUNDED = "02_Wounded";
    private const string ANIM_03_WOUNDED_IDLE = "03_Wounded_idle";
    private const string ANIM_04_DEATH = "04_Death";
    private const string ANIM_05_ATTACK = "05_Attack";
    private const string ANIM_CAST_CLON = "Cast_Clon";
    private const string ANIM_IDLECLON = "idleClon";
    private const string ANIM_WOUNDED_CLON = "Wounded_Clon";
    private const string ANIM_WOUNDED_IDLE_CLON = "Wounded_idle_Clon";
    
    #endregion
    
    [SerializeField] private AssassinMovesData m_data;


    private void Start()
    {
        GameplayEvents.MechanicAddedToFighter += OnMechanicAdded;
        ConfigFighterHP();
    }
    
    private void OnDestroy()
    {
        GameplayEvents.MechanicAddedToFighter -= OnMechanicAdded;
    }
    
    private void OnMechanicAdded(Fighter fighter, BaseMechanic mechanic)
    {
        if (mechanic.GetMechanicType() != MechanicType.BLEED)
        {
            return;
        }

        if (MechanicsManager.Instance.Contains(this, MechanicType.BLEED))
        {
            MechanicsManager.Instance.RemoveMechanic(this, MechanicType.BLEED);
        }
    }

    protected override void OnTookDamage(int damage, bool isCritical)
    {
        if (CombatManager.Instance.IsGameOver)
        {
            return;
        }
        base.OnTookDamage(damage, isCritical);
        m_animation.Play(ANIM_02_WOUNDED);
    }

    protected override void OnDeath()
    {
        if (CombatManager.Instance.IsGameOver)
        {
            return;
        }
        base.OnDeath();
        m_animation.Play(ANIM_04_DEATH);
    }
    

    public void SetIntention(MoveData move)
    {
        m_nextMove = move;
        ShowIntention();
    }
    
    public override void DetermineIntention()
    {
        //grab intention from original
        //RandomIntentionPicker(m_moves);
        //ShowIntention();
    }

    public override void ShowIntention()
    {
        base.ShowIntention();
        switch (m_nextMove.clientID)
        {
            case "Hit20":
                CallOnIntentionDetermined(Intention.ATTACK, m_nextMove.description);
                break;
            case "Bleed3":
                CallOnIntentionDetermined(Intention.CAST_DEBUFF, m_nextMove.description);
                break;
            case "Send3Cards":
                CallOnIntentionDetermined(Intention.CAST_DEBUFF, m_nextMove.description);
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
            case "Hit20":
                CombatManager.Instance.Player.TakeDamage(m_data.Move1Damage, this, true);
                m_animation.Play(ANIM_05_ATTACK, finishCallback);
                break;
            case "Bleed3":
                MechanicsManager.Instance.AddMechanic(new BleedMechanic(m_data.Move2Bleed, CombatManager.Instance.Player), this);
                MechanicsManager.Instance.AddMechanic(new BlockMechanic(m_data.Move2Block, this), this);
                m_animation.Play(ANIM_05_ATTACK, finishCallback);
                break;
            case "Send3Cards":
                for (int i = 0; i < m_data.Move3NumOfCards; i++)
                {
                    CombatManager.Instance.SpawnCard(m_data.Move3Card, CardStorage.DRAW_PILE);
                }
                CombatManager.Instance.ShuffleDeckDrawPile();
                m_animation.Play(ANIM_05_ATTACK, finishCallback);
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
