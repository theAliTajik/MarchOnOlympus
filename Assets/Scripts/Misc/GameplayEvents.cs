using Game;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EGamePhase
{
    COMBAT_START,
    COMBAT_END,
    MECHANIC_ADDED,
    MECHANICS_FINISHED_TRY_REDUCE,
    PLAYER_TURN_START,
    CARD_DRAW_FINISHED,
    CARD_PERISHED,
    CARD_SPAWNED,
    CARD_DISCARDED,
    ENEMY_KILLED,
    STANCE_CHANGED,
    PLAYER_TURN_END,
    ENEMY_TURN_START,
    PLAYER_DAMAGED,
    PLAYER_HEALED,
    ENEMY_TURN_END,
    CARD_SELECTED,
    CARD_PLAYED,
    DAMAGE_DEALT,
    DAMAGE_RECEIVED,
    ENEMY_DAMAGED,
    FIGHTER_HEALED
}

public class GameplayEvents
{
    public static event System.Action<int> OnEnergyChanged;
    public static void SendOnEnergyChanged(int energy)
    {
        OnEnergyChanged?.Invoke(energy);
    }


    public static event System.Action<Stance> StanceChanged;
    internal static void SendStanceChanged(Stance selectedStanceIndex)
    {
        StanceChanged?.Invoke(selectedStanceIndex);
    }
    
    public static event System.Action<EGamePhase> GamePhaseChanged;
    internal static void SendGamePhaseChanged(EGamePhase selectedPhase)
    {
        GamePhaseChanged?.Invoke(selectedPhase);
    }
    
    public static event System.Action<string> PerkRemoved;

    internal static void SendPerkRemoved(string perkClientId)
    {
        PerkRemoved?.Invoke(perkClientId);
    }
    
    public static event System.Action ExtraCardDrawn;
    internal static void SendExtraCardDrawn()
    {
        ExtraCardDrawn?.Invoke();
    }

    public static event System.Action<DaggerMasterCard> DaggerMasterCardPlayed;

    internal static void SendDaggerMasterCardPlayed(DaggerMasterCard daggerMasterCard)
    {
        DaggerMasterCardPlayed?.Invoke(daggerMasterCard);
    }
    
    public static event System.Action<CardDisplay> CardPlayFinished;

    public static void SendCardPlayFinished(CardDisplay cardDisplay)
    {
        CardPlayFinished?.Invoke(cardDisplay);
    }
    
    public static event System.Action<bool> ChangeEndTurnButtonState;

    public static void SendChangeEndTurnButtonState(bool endTurnButtonState)
    {
        ChangeEndTurnButtonState?.Invoke(endTurnButtonState);
    }
    
    public static event System.Action<Fighter, BaseMechanic> MechanicAddedToFighter;

    public static void SendMechanicAddedToFighter(Fighter target, Fighter sender, BaseMechanic mechanic)
    {
        MechanicAddedToFighter?.Invoke(target, mechanic);
        GameInfoHelper.MechanicsData.MechanicsTarget = target;
        GameInfoHelper.MechanicsData.MechanicsSender = sender;
        GameInfoHelper.MechanicsData.AddedMechanic = mechanic;
        SendGamePhaseChanged(EGamePhase.MECHANIC_ADDED);

    }

    public static void SendCardSelected(CardDisplay cardDisplay)
    {
        GameInfoHelper.CardsData.SelectedCard = cardDisplay;
        SendGamePhaseChanged(EGamePhase.CARD_SELECTED);
    }
    
    
    public static event System.Action<Fighter, int> FighterRestoredHP;

    public static void SendFighterRestoredHP(Fighter fighter, int restoredHP)
    {
        FighterRestoredHP?.Invoke(fighter, restoredHP);
    }
    
    public static event System.Action<int> OnStanceCdChanged;

    public static void SendOnStanceCdChanged(int amount)
    {
        OnStanceCdChanged?.Invoke(amount);
    }
    
    public static event System.Action<string> SpawnBoss;

    public static void SendSpawnBoss(string bossID)
    {
        SpawnBoss?.Invoke(bossID);
    }
    
    public static event System.Action<CardPile> ShowCards;

    public static void SendShowCardsForSelecting(CardPile cardPile)
    {
        ShowCards?.Invoke(cardPile);
    }

    public static event System.Action<List<CardInDeckStateMachine>> ShowCardsByData;
    public static void SendShowCardsForSelecting(List<CardInDeckStateMachine> cards)
    {
        ShowCardsByData?.Invoke(cards);
    }
    
    public static event System.Action<CardDisplay> CardSelectedByPlayer;

    public static void SendCardSelectedByPlayer(CardDisplay selectedCardDisplay)
    {
        CardSelectedByPlayer?.Invoke(selectedCardDisplay);
    }
    
    public static event System.Action CardNotSelected;

    public static void SendCardNotSelected()
    {
        CardNotSelected?.Invoke();
    }
    
    public static event System.Action<BaseCardData> RewardedCardSelected;

    public static void SendRewarderCardSelected(BaseCardData cardData)
    {
        RewardedCardSelected?.Invoke(cardData);
    }

}
