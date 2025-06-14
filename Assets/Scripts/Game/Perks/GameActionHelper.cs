using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using Game.ModifiableParam;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

public static class GameActionHelper
{
    
    // ### Game
    public static void ModifyEnergy(IParamModifier<int> modifier)
    {
        CombatManager.Instance.Energy.AddModifier(modifier);
    }

    public static void RemoveModifyEnergy(IParamModifier<int> modifier)
    {
        CombatManager.Instance.Energy.RemoveModifier(modifier);
    }
    public static void GainEnergy(int amount)
    {
        CombatManager.Instance.Energy.GainEnergy(amount);
    }

    public static void SwtichStance(Stance stance)
    {
        CombatManager.Instance.ForceChangeStance(stance);
    }

    public static void RefreshStanceCd()
    {
        CombatManager.Instance.StanceMachine.ResetStanceCdToDefault();
    }

    public static void AllowStanceChange(bool allow)
    {
        CombatManager.Instance.StanceMachine.SetStanceChangeAllowed(allow);
    }
    
    public static void EndTurn()
    {
        CombatManager.Instance.OnEndTurnButtonClicked();
    }

    
    // ### Cards
    public static void PlayCard(CardDisplay card, Fighter target)
    {
        CardsUIManager.Instance.OnCardPlay(card, target);
    }
    
    public static List<CardDisplay> DrawCards(int amount)
    {
        return CombatManager.Instance.DrawCard(amount);
    }

    public static void MoveCardToHand(CardDisplay card)
    {
        CombatManager.Instance.MoveCardToHand(card);
    }

    public static void DiscardCard(CardDisplay card)
    {
        CombatManager.Instance.DiscardCard(card);
    }

    public static void ModifyDrawAmount(IParamModifier<int> modifier)
    {
        CombatManager.Instance.Deck.SetDrawAmountOverride(modifier);
    }

    public static void RemoveDrawAmountModifier(IParamModifier<int> modifier)
    {
        CombatManager.Instance.Deck.RemoveDrawAmountOverride(modifier);
    }
    
    public static void IncreaseDrawAmount(int amount)
    {
        IParamModifier<int> addModifier = new AddValueModifier<int>(amount); 
        CombatManager.Instance.Deck.SetDrawAmountOverride(addModifier);
    }
    
    public static void DecreaseDrawAmount(int amount)
    {
        IParamModifier<int> addModifier = new AddValueModifier<int>(-amount);
        CombatManager.Instance.Deck.SetDrawAmountOverride(addModifier);
    }

    public static void SetDrawAmountOverride(int amount, bool additive)
    {
        IParamModifier<int> modifier;
        if (additive)
        {
            modifier = new AddValueModifier<int>(amount);
        }
        else
        {
            modifier = new SetValueModifier<int>(amount);
        }
        CombatManager.Instance.Deck.SetDrawAmountOverride(modifier);
    }

    public static void RemoveCardDrawAmountOverride()
    {
        CombatManager.Instance.Deck.RemoveDrawAmountOverride();
    }

    public static CardDisplay SpawnCard(CardInDeckStateMachine cardInDeck, CardStorage cardStorage)
    {
        return CombatManager.Instance.SpawnCard(cardInDeck, cardStorage);
    }
    public static CardDisplay SpawnCard(BaseCardData cardToSpawn, CardStorage cardStorage)
    {
        return CombatManager.Instance.SpawnCard(cardToSpawn, cardStorage);
    }

    public static CardDisplay SpawnCard(string cardToSpawn, CardStorage cardStorage)
    {
        return CombatManager.Instance.SpawnCard(cardToSpawn, cardStorage);
    }
    
    public static void BanishCard(CardDisplay cardDisplay)
    {
        CombatManager.Instance.BanishCard(cardDisplay);
    }

    public static void PerishCard(CardDisplay cardDisplay)
    {
        CombatManager.Instance.BanishCard(cardDisplay);
    }

    public static void SetCardToBePlayedTwice(int index)
    {
        CombatManager.Instance.SetCardToBePlayedTwice(index);
    }

    public static void SetCardToBePlayedTwice(CardPacks pack)
    {
        CombatManager.CardsToPlayTwiceData card =
            new CombatManager.CardsToPlayTwiceData(-1, String.Empty, CardType.NONE, CardPacks.IMPALE);
        CombatManager.Instance.SetCardToBePlayedTwice(card);
    }
    
    public static void RemoveCardsPlayTwice(int index)
    {
        CombatManager.Instance.RemoveCardsPlayTwice(index);
    }

    public static void ChangeCard(CardDisplay cardDisplay, BaseCardData targetCardData)
    {
        CardInDeckStateMachine cardInDeck = new CardInDeckStateMachine();
        cardInDeck.Configure(targetCardData);

        if (cardDisplay.CardInDeck.GetCardName() == cardInDeck.GetCardName())
        {
            return;
        }
                
        cardDisplay.Configure(cardInDeck);
        cardDisplay.RefreshUI();
        CombatManager.Instance.InstantiateAction(cardDisplay);
    }
    
    public static void TransformCard(CardDisplay cardDisplay, string cardName)
    {
        BaseCardData cardData = Resources.Load<BaseCardData>("CardsData/" + cardName);
        if (cardData == null)
        {
            Debug.LogWarning("Card Data is null");
            return;
        }

        if (cardDisplay.CardInDeck.GetCardName() == cardData.Name)
        {
            return;
        }

        CardInDeckStateMachine cardInDeck = new CardInDeckStateMachine();
        cardInDeck.Configure(cardData);
        cardDisplay.Configure(cardInDeck);
        cardDisplay.RefreshUI();
        CombatManager.Instance.InstantiateAction(cardDisplay);
    }

    public static void SetCardEnergyOverride(CardDisplay cardDisplay, ECardInDeckState state, int newEnergy, bool additive = false)
    {
        IParamModifier<int> energyModifier;
        if (additive)
        {
            energyModifier = new AddValueModifier<int>(newEnergy);
        }
        else
        {
            energyModifier = new SetValueModifier<int>(newEnergy);
        }
        
        switch (state)
        {
            case ECardInDeckState.NORMAL:
                cardDisplay.CardInDeck.NormalState.SetEnergyOverride(energyModifier);
                break;
            case ECardInDeckState.STANCE:
                cardDisplay.CardInDeck.StanceState.SetEnergyOverride(energyModifier);
                break;
            default:
                Debug.Log("invalid state");
                break;
        }
    }

    public static void RemoveCardEnergyOverride(CardDisplay cardDisplay, ECardInDeckState state)
    {
        switch (state)
        {
            case ECardInDeckState.NORMAL:
                cardDisplay.CardInDeck.NormalState.RemoveAllEnergyOverrides();
                break;
            case ECardInDeckState.STANCE:
                cardDisplay.CardInDeck.StanceState.RemoveAllEnergyOverrides();
                break;
            default:
                Debug.Log("invalid state");
                break;
        }
    }

    public static void SetCardDescriptionOverride(CardDisplay cardDisplay, ECardInDeckState state, string description, bool additive = false)
    {
        if (state == ECardInDeckState.NORMAL)
        {
            cardDisplay.CardInDeck.NormalState.SetDescriptionOverride(description, additive);
        }
        
        if (state == ECardInDeckState.STANCE)
        {
            cardDisplay.CardInDeck.StanceState.SetDescriptionOverride(description, additive);
        }
    }

    public static void RemoveCardDescriptionOverride(CardDisplay cardDisplay, ECardInDeckState state)
    {
        if (state == ECardInDeckState.NORMAL)
        {
            cardDisplay.CardInDeck.NormalState.RemoveDescriptionOverride();
        }
        
        if (state == ECardInDeckState.STANCE)
        {
            cardDisplay.CardInDeck.StanceState.RemoveDescriptionOverride();
        }
    }
    
    public static void SetCardTargetingTypeOverride(CardDisplay cardDisplay, ECardInDeckState state, TargetType targetType)
    {
        switch (state)
        {
            case ECardInDeckState.NORMAL:
                cardDisplay.CardInDeck.NormalState.SetTargetTypeOverride(targetType);
                break;
            case ECardInDeckState.STANCE:
                cardDisplay.CardInDeck.StanceState.SetTargetTypeOverride(targetType);
                break;
        }
    }

    public static void RemoveCardTargetingTypeOverride(CardDisplay cardDisplay, ECardInDeckState state)
    {
        switch (state)
        {
            case ECardInDeckState.NORMAL:
                cardDisplay.CardInDeck.NormalState.RemoveTargetTypeOverride();
                break;
            case ECardInDeckState.STANCE:
                cardDisplay.CardInDeck.StanceState.RemoveTargetTypeOverride();
                break;
        }
    }

    public static void SetCardPerishOverrride(CardDisplay cardDisplay, ECardInDeckState state, bool doesPerish)
    {
        switch (state)
        {
            case ECardInDeckState.NORMAL:
                cardDisplay.CardInDeck.NormalState.SetDoesPerishOverride(doesPerish);
                break;
            case ECardInDeckState.STANCE:
                cardDisplay.CardInDeck.StanceState.SetDoesPerishOverride(doesPerish);
                break;
            default:
                Debug.Log("invalid state");
                break;
        }
    }
    
    public static void RemoveCardPerishOverride(CardDisplay cardDisplay, ECardInDeckState state)
    {
        switch (state)
        {
            case ECardInDeckState.NORMAL:
                cardDisplay.CardInDeck.NormalState.RemoveDoesPerishOverride();
                break;
            case ECardInDeckState.STANCE:
                cardDisplay.CardInDeck.StanceState.RemoveDoesPerishOverride();
                break;
            default:
                Debug.Log("invalid state");
                break;
        }
        cardDisplay.RefreshUI();
    }
    
    public static void AddExtraActionToCards(MonoBehaviour owner, Action<CardDisplay, Fighter> action)
    {
        CardsQueue.Instance.AddExtraAction(owner, action);
    }

    
    // ### Fighter
    public static void DamageFighter(Fighter target, Fighter sender, int damage, bool doesReturnToSender = true,
        bool isArmorPiercing = false)
    {
        target.TakeDamage(damage, sender, doesReturnToSender, isArmorPiercing);
    }

    public static void SetFighterMaxDamagePercentage(Fighter fighter, int percentage)
    {
        fighter.HP.SetDamageGuard(percentage);
    }

    public static void RemoveFighterMaxDamagePercentage(Fighter fighter)
    {
        fighter.HP.RemoveDamageGuard();
    }

    public static void HealFighter(Fighter fighter, int amount)
    {
        if (amount <= 0)
        {
            Debug.Log("invalid amount passed to heal fighter");
            return;
        }
        
        fighter.Heal(amount);
    }

    public static void RemoveAllMechanicOfCategory(Fighter fighter, MechanicCategory catigory)
    {
        MechanicsList mechanics = MechanicsManager.Instance.GetMechanicsList(fighter);
        mechanics.RemoveAllMechanicsOfCategory(catigory);
    }
    
    // ### Player
    public static void IncreasePlayerMaxHP(int amount)
    {
        CombatManager.Instance.Player.HP.IncreaseMaxHP(amount);
        CombatManager.Instance.Player.Heal(amount);
    }

    public static void DecreasePlayerMaxHP(int amount)
    {
        CombatManager.Instance.Player.HP.DecreaseMaxHP(amount);
    }
    
    public static void HealPlayer(int amount)
    {
        CombatManager.Instance.Player.Heal(amount);
    }
    

    
    // ### Enemeies
    public static void MakeEnemyPlayTwice(Fighter enemy)
    {
        EnemiesManager.Instance.MakeEnemyPlayTwice(enemy);   
    }
    
    // ### Mechanics
    public static void AddMechanicToPlayer(int stack, MechanicType mechanicType, bool hasGuard = false, int guardMin = 0)
    {
        MechanicsManager.Instance.AddMechanic(stack, mechanicType, CombatManager.Instance.Player, hasGuard, guardMin);
    }
    
    public static void RemoveMechanicFromPlayer(MechanicType mechanicType)
    {
        MechanicsManager.Instance.RemoveMechanic(CombatManager.Instance.Player, mechanicType);
    }
    
    public static void AddMechanicToFighter(Fighter fighter, int stack, MechanicType mechanicType,
        bool hasGuard = false, int guardMin = 0)
    {
        MechanicsManager.Instance.AddMechanic(stack, mechanicType, fighter, hasGuard, guardMin);
    }

    public static void ReduceMechanicStack(Fighter fighter, int amount, MechanicType mechanicType)
    {
        MechanicsManager.Instance.ReduceMechanic(fighter, mechanicType, amount);
    }
    
    public static void RemovePlayerMechanicGuard(MechanicType mechanicType)
    {
        RemoveMechanicGuard(GameInfoHelper.GetPlayer(), mechanicType);
    }

    public static void RemoveMechanicGuard(Fighter fighter, MechanicType mechanicType)
    {
        BaseMechanic mechanic = MechanicsManager.Instance.GetMechanic(fighter, mechanicType);
        if (mechanic != null)
        {
            mechanic.RemoveGuard();
        }
    }
    
    public static void SetMechanicGuard(MechanicType mechanicType, int guradMin = 0)
    {
        
        BaseMechanic mechanic = MechanicsManager.Instance.GetMechanic(CombatManager.Instance.Player, mechanicType);
        if (mechanic != null)
        {
            if (guradMin <= 0)
            {
                mechanic.SetGuard(mechanic.Stack);
            }
            else
            {
                mechanic.SetGuard(guradMin);
            }
        }
    }

    public static void AddStrNextTurn(Fighter target, int amount)
    {
        GameInfoHelper.MechanicsData.StrToAddNextTurn[target] += amount;
        GameplayEvents.GamePhaseChanged += OnPhaseChange;
    }

    private static void OnPhaseChange(EGamePhase phase)
    {
        if (phase == EGamePhase.CARD_DRAW_FINISHED)
        {
            Dictionary<Fighter, int> FighterStr = GameInfoHelper.MechanicsData.StrToAddNextTurn;
            foreach (Fighter fighter in FighterStr.Keys)
            {
                AddMechanicToFighter(fighter, FighterStr[fighter], MechanicType.STRENGTH);
            }
            GameInfoHelper.MechanicsData.StrToAddNextTurn.Clear();
        }
    }

}
