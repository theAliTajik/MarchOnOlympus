using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;

public static class GameInfoHelper
{
    
    // ### Game
    public static int GetCurrentEnergy()
    {
        return CombatManager.Instance.Energy.Current;
    }

    public static Stance GetCurrentStance()
    {
        return CombatManager.Instance.CurrentStance;
    }

    public static int GetCurrentTurn()
    {
        return CombatManager.Instance.CurrentTurn;
    }
    
    
    public static string GetCurrentDeckID()
    {
        return GameSessionParams.DeckTemplateClientId;
    }

    public static int GetHonor()
    {
        if (!GameProgress.Instance)
        {
            return 0;
        }
        
        return GameProgress.Instance.Data.Honor;
    }

    public static int GetInvent()
    {
        return 2;
    }
    
    public static int GetInventLevel()
    {
        return 1;
    }
    
    public static int GetInventionsThisMatch()
    {
        return 1;
    }
    
    public static int GetNumOfCardsInHand()
    {
        return CombatManager.Instance.Deck.CountAllCardsIn(CardStorage.HAND);
    }
    
    // ### Cards
    public static class CardsData
    {
        public static CardDisplay SelectedCard = null;
    }
    
    public static List<CardDisplay> GetAllCardsIn(CardStorage cardStorage)
    {
        return CombatManager.Instance.Deck.GetAllCardsIn(cardStorage);
    }
    
    public static CardDisplay GetTopCardIn(CardStorage cardStorage)
    {
        List<CardDisplay> cards = GetAllCardsIn(cardStorage);
        return cards[0];
    }

    public static BaseCardData GetRandomCard()
    {
        int rand = UnityEngine.Random.Range(0, CardsDb.Instance.AllCards.Count);
        BaseCardData randCard = CardsDb.Instance.AllCards[rand].CardData;
        return randCard;
    }
    
    public static int CountNumOfCardsInDeck(CardStorage cardStorage)
    {
        return CombatManager.Instance.Deck.CountAllCardsIn(cardStorage);
    }
    
    public static int CountNumOfCardsPlayed()
    {
        return CombatManager.Instance.CardsPlayed.Count;
    }
    
    public static int CountNumOfCardsPlayedThisTurn()
    {
        return CombatManager.Instance.NumberOfCardsPlayedThisTurn;
    }

    public static List<CardDisplay> GetUnusedCardsThisTurn()
    {
        return CombatManager.Instance.CardsNotUsedThisTurn;
    }
    
    public static int CountCardsByPack(CardPacks pack, CardStorage PlaceToSearch = CardStorage.HAND)
    {
        List<CardDisplay> currentHand = GetAllCardsIn(PlaceToSearch);
        int count = 0;
        foreach (CardDisplay card in currentHand)
        {
            if (card.CardInDeck.GetCardPack() == pack)
            {
                count++;
            }
        }
        return count;
    }
    
    public static int CountCardsByStance(Stance stance, CardStorage PlaceToSearch = CardStorage.HAND)
    {
        List<CardDisplay> currentHand = GetAllCardsIn(PlaceToSearch);
        int count = 0;
        foreach (CardDisplay card in currentHand)
        {
            if (card.CardInDeck.GetStance() == stance)
            {
                count++;
            }
        }
        return count;
    }
    
    public static bool CheckIfLastCardPlayedWas(string name, bool DoesIncludeThisCard = false)
    {
        List<CardDisplay> cardsPlayed = CombatManager.Instance.CardsPlayed;
        int includeCard = 2;
        if (DoesIncludeThisCard)
        {
            includeCard = 1;
        }
        CardDisplay lastCard = cardsPlayed[cardsPlayed.Count - includeCard];
        if (lastCard.CardInDeck.GetCardName() == name)
        {
            return true;
        }
        return false;
    }
    
        
    public static bool CheckIfLastCardPlayedWas(CardType type, bool DoesIncludeThisCard = false)
    {
        List<CardDisplay> cardsPlayed = CombatManager.Instance.CardsPlayed;
        int includeCard = 2;
        if (DoesIncludeThisCard)
        {
            includeCard = 1;
        }
        CardDisplay lastCard = cardsPlayed[cardsPlayed.Count - includeCard];
        if (lastCard.CardInDeck.GetCardType() == type)
        {
            return true;
        }
        return false;
    }
    
    public static CardDisplay GetLastCardPlayed()
    {
        return CombatManager.Instance.CardsPlayed[CombatManager.Instance.CardsPlayed.Count - 1];
    }

    public static List<CardDisplay> GetCardsBy(Stance stance, CardStorage PlaceToSearch)
    {
        List<CardDisplay> cards = GetAllCardsIn(PlaceToSearch);
        List<CardDisplay> cardsOfStance = new List<CardDisplay>();
        for (var i = 0; i < cards.Count; i++)
        {
            if (cards[i].CardInDeck.GetStance() == stance)
            {
                cardsOfStance.Add(cards[i]);
            }
        }
        return cardsOfStance;
    }
    
    public static List<CardDisplay> GetCardsBy(CardActionType cardActionType, CardStorage PlaceToSearch)
    {
        List<CardDisplay> cards = GetAllCardsIn(PlaceToSearch);
        List<CardDisplay> cardsOfAction = new List<CardDisplay>();
        for (var i = 0; i < cards.Count; i++)
        {
            if (IsCard(cards[i], cardActionType))
            {
                cardsOfAction.Add(cards[i]);
            }
        }
        return cardsOfAction;
    }
    
    public static bool IsCard(CardDisplay card, CardType cardType)
    {
        if (card.CardInDeck.GetCardType() == cardType)
        {
            return true;
        }
        
        return false;
    }
    
    public static bool IsCard(CardDisplay card, CardPacks cardPack)
    {
        if (card.CardInDeck.GetCardPack() == cardPack)
        {
            return true;
        }
        
        return false;
    }
    
    public static bool IsCard(CardDisplay card, CardActionType cardType)
    {
        List<CardActionType> NormalCardActionTypes = card.CardInDeck.NormalState.GetActionsTypes();
        List<CardActionType> StanceCardActionTypes = card.CardInDeck.StanceState.GetActionsTypes();
        
        //normal data:
        
        foreach (CardActionType cardActionType in NormalCardActionTypes)
        {
            if (cardActionType == cardType)
            {
                return true;
            }    
        }
        
        foreach (CardActionType cardActionType in StanceCardActionTypes)
        {
            if (cardActionType == cardType)
            {
                return true;
            }    
        }
        return false;
    }


    public static int GetExtraDrawnCards()
    {
        return CombatManager.Instance.ExtraCardsDrawThisTurn;
    }

    public static int GetExtraDrawnCardsThisTurn()
    {
        return CombatManager.Instance.ExtraCardsDrawThisTurn;
    }

    public static int GetStartingDeckSize()
    {
       return CombatManager.Instance.NumOfStartingCards;
    }

    public static int GetCardsEnergy(CardDisplay cardDisplay)
    {
        return cardDisplay.CardInDeck.CurrentState.GetEnergy();
    }

    public static CardDisplay GetRandomCard(CardStorage cardStorage)
    {
        List<CardDisplay> cards;
        cards = CombatManager.Instance.Deck.GetAllCardsIn(cardStorage);
        if (cards.Count <= 0)
        {
            return null;
        }
        int randIndex = Random.Range(0, cards.Count);
        return cards[randIndex];
    }

    public static int CountCardsWithName(string name, CardStorage PlaceToSearch = CardStorage.HAND)
    {
        List<CardDisplay> currentHand = GetAllCardsIn(PlaceToSearch);
        int count = 0;
        foreach (CardDisplay cardDisplay in currentHand)
        {
            if (cardDisplay.CardInDeck.GetCardName() == name)
            {
                count++;
            }
        }
        return count;
    }
    
    public static List<CardDisplay> GetCardsWithName(string name, bool contains = false)
    {
        List<CardDisplay> currentHand = CombatManager.Instance.Deck.CardPiles[CardStorage.HAND].Cards;
        List<CardDisplay> SelectedCards = new List<CardDisplay>();
        
        foreach (CardDisplay cardDisplay in currentHand)
        {
            if (contains)
            {
                if (cardDisplay.CardInDeck.GetCardName().Contains(name))
                {
                    SelectedCards.Add(cardDisplay);
                }

                continue;
            }
            
            if (cardDisplay.CardInDeck.GetCardName() == name)
            {
                SelectedCards.Add(cardDisplay);
            }
        }
        return SelectedCards;
    }

    public static List<BaseCardData> GetCardsWithNameFromDB(string name, bool contains = false)
    {
        return CardsDb.Instance.GetCardsWithName(name, contains);
    }
    
    
    public static List<CardDisplay> GetAllCardsOfPack(CardPacks pack, CardStorage placeToSearch)
    {
        List<CardDisplay> allCards = GetAllCardsIn(placeToSearch);
        List<CardDisplay> cardsOfPack = new List<CardDisplay>();
        foreach (CardDisplay cardDisplay in allCards)
        {
            if (cardDisplay.CardInDeck.GetCardPack() == pack)
            {
                cardsOfPack.Add(cardDisplay);
            }
        }
        
        return cardsOfPack;
    }
    
    
    public static CardPile GetCardPile(CardStorage cardStorage)
    {
        return CombatManager.Instance.Deck.GetCardPile(cardStorage);
    }

    public static int GetNumOfStartingDeckChunks(int Chunk)
    {
        if (Chunk < 1)
        {
            CustomDebug.LogWarning("Cannot devide by less than one", Categories.Combat.Cards);
            return 0;
        }
        
        int startingDeckSize = GetStartingDeckSize();
        return startingDeckSize / Chunk;
    }

    
    
    // ### Fighter
    
    public static bool CheckIfDamageToFighterIsFatal(Fighter target, int damage)
    {
        return target.HP.IsDamageFatal(damage);
    }
    
    // ### Player
    public static Fighter GetPlayer()
    {
        return CombatManager.Instance.Player;
    }

    public static bool IsPlayerTurn()
    {
        return CombatManager.Instance.IsPlayersTurn;
    }

    public static int GetPlayerHPPrecentage()
    {
        FighterHP playerHP = CombatManager.Instance.Player.HP;
        int hpPercentage = (int)((float)playerHP.Current / playerHP.Max * 100);
        return hpPercentage;
    }
    
    public static bool CheckIfPlayerHPIsUnderACertainPrecent(int precent)
    {
        if (precent < 1)
        {
            Debug.Log("precent of hp unvalid");
            return false;
        }

        if (precent > 100)
        {
            precent = 100;
        }
        float precentConverted = precent / 100f;

        if (CombatManager.Instance.Player.HP.Current <= CombatManager.Instance.Player.HP.Max * precentConverted)
        {
            return true;
        }
        return false;

    }

    public static int GetDamageDoneToPlayerThisTurn()
    {
        return CombatManager.Instance.damageDoneToPlayersThisTurn;
    }

    public static bool CompareFighterToPlayer(Fighter fighter)
    {
        if (fighter == null)
        {
            Debug.Log("fighter was null");
            return false;
        }

        if (fighter == CombatManager.Instance.Player)
        {
            return true;
        }
        
        return false;
    }
    
    


    
    // ### Enemeies
    public static int GetNumOfEnemies()
    {
        return EnemiesManager.Instance.GetNumOfEnemies();
    }

    public static Fighter GetRandomEnemy()
    {
        return EnemiesManager.Instance.GetRandomEnemy();
    }

    public static List<Fighter> GetAllEnemies()
    {
        if (!EnemiesManager.Instance)
        {
            return null;
        }
        return EnemiesManager.Instance.GetAllEnemies();
    }

    public static int GetDamageDoneToEnemiesOverAll()
    {
        return CombatManager.Instance.DamageDoneToEnemiesOverAll;
    }

    public static bool CheckIfAnyEnemyIsAttackingThisTurn()
    {
        CustomDebug.LogWarning("Enemy attacking check needs implementation", Categories.Fighters.Enemies.Root);
        return true;
    }
   
    // ### Mechanics
    public static class MechanicsData
    {
        public static Fighter MechanicsSender = null;
        public static Fighter MechanicsTarget = null;
        public static BaseMechanic AddedMechanic = null;
        
        public static Dictionary<Fighter, int> StrToAddNextTurn = new Dictionary<Fighter, int>();
    }
    
    public static int GetMechanicStack(Fighter fighter, MechanicType mechanicType)
    {
        return MechanicsManager.Instance.GetMechanicsStack(fighter, mechanicType);
    }

    public static int GetMechanicStack(IHaveMechanics owner, MechanicType mechanicType)
    {
        return MechanicsManager.Instance.GetMechanicsStack(owner, mechanicType);
    }
    
    public static bool CheckIfFighterHasMechanic(Fighter fighter, MechanicType mechanicType)
    {
        return MechanicsManager.Instance.Contains(fighter, mechanicType);
    }

    public static bool CheckIfMechanicIsDebuff(MechanicType mechanicType)
    {
        List<MechanicType> debuffs = GameInfoHelper.GetAllDebuffMechanics();
        if (debuffs.Contains(mechanicType))
        {
            return true;
        }
        
        return false;
    } 
        
    public static List<MechanicType> GetDamageOverTimeMechanics()
    {
        if (MechanicsManager.Instance.DamageOverTimeMechanics != null)
        {
            return MechanicsManager.Instance.DamageOverTimeMechanics;
        }
       
        Debug.Log("DamageOverTimeMechanics not found");
        return null;
    }

    public static List<MechanicType> GetAllDebuffMechanics()
    {
        return MechanicsManager.Instance.DebuffMechanics;
    }


   public static int GetNumOfDebuffMechanics(Fighter target)
   {
       return MechanicsManager.Instance.GetDebuffsCount(target);
   }
}
