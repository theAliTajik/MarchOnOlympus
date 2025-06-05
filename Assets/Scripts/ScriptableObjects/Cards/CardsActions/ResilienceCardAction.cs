using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;
using UnityEngine.EventSystems;

public class ResilienceCardAction : BaseCardAction
{

    private bool m_stanceIsActive;
    public override void Play(BaseCardData cardData, Action finishCallback, Fighter target, CardDisplay cardDisplay)
    {
        StartCoroutine(WaitAndExecute(finishCallback, 2f,cardData, target, cardDisplay));
    }

    private IEnumerator WaitAndExecute(Action finishCallback, float delay, BaseCardData cardData, Fighter target, CardDisplay cardDisplay)
    {
        ResilienceCard c = (ResilienceCard)cardData;
        
        // only damage no debuffs on player this turn
        
        if (CombatManager.Instance.CurrentStance == cardData.MStance)
        {
            // reflect what ever effect wanted to be put on you this turn
            m_stanceIsActive = true;
        }
        
        GameplayEvents.GamePhaseChanged += OnPhaseChanged;

        finishCallback?.Invoke();
        yield break;
    }

    private void OnPhaseChanged(EGamePhase phase)
    {
        switch (phase)
        {
            case EGamePhase.MECHANIC_ADDED:
                bool isPlayer = GameInfoHelper.CompareFighterToPlayer(GameInfoHelper.MechanicsData.MechanicsTarget);
                if (isPlayer)
                {
                    // remove debufss
                    BaseMechanic mechanic = GameInfoHelper.MechanicsData.AddedMechanic;
                    MechanicType mechanicType = mechanic.GetMechanicType();
                    
                    if (!GameInfoHelper.CheckIfMechanicIsDebuff(mechanicType))
                    {
                        return;
                    }
                    
                    GameActionHelper.RemoveMechanicFromPlayer(mechanicType);

                    // if stance: reflect that debuff to enemy
                    if (m_stanceIsActive)
                    {
                        Fighter mechanicsSender = GameInfoHelper.MechanicsData.MechanicsSender;
                        GameActionHelper.AddMechanicToFighter(mechanicsSender, mechanic.Stack, mechanicType);
                    }
                }
                break;
            case EGamePhase.CARD_DRAW_FINISHED:
                GameplayEvents.GamePhaseChanged -= OnPhaseChanged;
                break;
        }

    }

    private void OnDestroy()
    {
        GameplayEvents.GamePhaseChanged -= OnPhaseChanged;
    }
}