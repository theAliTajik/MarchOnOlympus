using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using Game.ModifiableParam;
using UnityEngine;
using UnityEngine.Serialization;

public class CombatManager : Singleton<CombatManager>
{
    
    public event Action<CombatPhase> OnCombatPhaseChanged;
    public event Action<bool> OnCombatOver;
    
    [SerializeField] private Energy m_energy;
    private PlayerController m_playerController;
    [SerializeField] private EnemiesManager m_enemiesManager;
    [SerializeField] private CardsUIManager m_cardsUIManager;
    [SerializeField] private CardsUIManager m_uiManager;
    
    [SerializeField] private Deck m_deck;
    [SerializeField] private StanceStateMachine m_stance;
    
    
    
    private Dictionary<CardDisplay, BaseCardAction> m_cardActions = new Dictionary<CardDisplay, BaseCardAction>();
    
    public struct CardsToPlayTwiceData
    {
        public int Index;
        public string Name;
        public CardType Type;
        public CardPacks Packs;

        public CardsToPlayTwiceData(int index)
        {
            Index = index;
            Name = null;
            Type = CardType.NONE;
            Packs = CardPacks.NONE;
        }

        public CardsToPlayTwiceData(int index, string name, CardType type, CardPacks packs)
        {
            Index = index;
            Name = name;
            Type = type;
            Packs = packs;
        }

        public override bool Equals(object obj)
        {
            if (obj is CardsToPlayTwiceData other)
            {
                return Index == other.Index;
            }
            return false;
        }
        
        public override int GetHashCode()
        {
            return Index.GetHashCode();
        }
    }
    private List<CardsToPlayTwiceData> m_cardsToPlayTwice = new List<CardsToPlayTwiceData>{};
    private bool m_isPlayersTurn;
    private bool m_playerIsStunned;
    private CombatPhase m_combatPhase;
    private CardDisplay m_pickedCard;
    private int m_extraCardsDrawnThisTurn = 0;
    private List<CardDisplay> m_cardsNotUsedThisTurn = new List<CardDisplay>();
    
    
    // Game Stat Trackers
    private int m_numberOfCardsPlayedThisTurn;
    private List<CardDisplay> m_cardsPlayed = new List<CardDisplay>();
    private int m_damageDoneToEnemiesThisTurn;
    private int m_damageDoneToEnemiesOverAll;
    private int m_damageDoneToPlayersThisTurn;
    private bool m_isGameOver = false;
    private int m_currentTurn = 0;
    private int m_numOfStartingCards;

    // Game Stat Trackers accesors
    public int NumberOfCardsPlayedThisTurn => m_numberOfCardsPlayedThisTurn;
    public List<CardDisplay> CardsPlayed => m_cardsPlayed;
    public int DamageDoneToEnemiesThisTurn => m_damageDoneToEnemiesThisTurn;
    public int DamageDoneToEnemiesOverAll => m_damageDoneToEnemiesOverAll;
    public bool IsGameOver => m_isGameOver;
    public int damageDoneToPlayersThisTurn => m_damageDoneToPlayersThisTurn;
    public int NumOfStartingCards => m_numOfStartingCards;
    
    public Energy Energy => m_energy;

    public Deck Deck => m_deck;
    public Stance CurrentStance => m_stance.CurrentStance;
    public StanceStateMachine StanceMachine => m_stance;
    public bool IsPlayersTurn => m_isPlayersTurn;
    public PlayerController Player => m_playerController;

    public int ExtraCardsDrawThisTurn { get {return m_extraCardsDrawnThisTurn;} }
    public int CurrentTurn { get {return m_currentTurn;} }
    public List<CardDisplay> CardsNotUsedThisTurn { get {return m_cardsNotUsedThisTurn;} }
    
    private void Start()
    {
        GameplayEvents.StanceChanged += OnStanceSelected;
        GameplayEvents.OnNewWaveOfEnemies += StartOnNewWaveOfEnemies;
    }

    private void StartOnNewWaveOfEnemies()
    {
        StartCoroutine(OnNewWaveOfEnemies());
    }

    private IEnumerator OnNewWaveOfEnemies()
    {
        yield return EndPlayerTurn();
        
        yield return new WaitForSeconds(0.5f);
        
        StartPlayerTurn();
    }

    private void OnDestroy()
    {
        GameplayEvents.SendGamePhaseChanged(EGamePhase.COMBAT_END);
        GameplayEvents.StanceChanged -= OnStanceSelected;
        GameplayEvents.OnNewWaveOfEnemies -= StartOnNewWaveOfEnemies;
        m_playerController.HP.OnTookDamage -= OnPlayerDamaged;
    }
    
    
    public void StartCombat()
    {
        m_uiManager.OnCardPick += OnCardPicked;
        m_enemiesManager.OnAllEnemiesDestroyed += OnAllEnemiesDead;
        m_playerController.Death += OnPlayerDead;
        
        m_energy.SetMaxEnergy(GameData.Instance.StartingEnergy);
        MechanicsList list = MechanicsManager.Instance.CreateMechanicsList(m_playerController);
        HUD.Instance.SpawnMechanicsDisplay(m_playerController, list);
        GameplayEvents.SendGamePhaseChanged(EGamePhase.COMBAT_START);
        m_playerController.HP.OnTookDamage += OnPlayerDamaged;
        m_numOfStartingCards = m_deck.CountAllCardsIn(CardStorage.ALL);
        StartPlayerTurn();
    }

    public void OnStanceSelected(Stance stance)
    {
        m_stance.ChangeState(stance);
        GameplayEvents.SendGamePhaseChanged(EGamePhase.STANCE_CHANGED);
    }

    public void ForceChangeStance(Stance stance)
    {
        m_stance.ChangeState(stance, true);
        GameplayEvents.SendGamePhaseChanged(EGamePhase.STANCE_CHANGED);
    }

    public void OnEndTurnButtonClicked()
    {
        StartCoroutine(EndPlayerTurnThenStartEnemiesTurn());
    }


    public void StartPlayerTurn()
    {
        m_isPlayersTurn = true;
        
        m_enemiesManager.DetermineAllEnemiesIntentions();


        m_currentTurn++;

        GameplayEvents.SendGamePhaseChanged(EGamePhase.PLAYER_TURN_START);
        m_combatPhase = CombatPhase.TURN_START;
        OnCombatPhaseChanged?.Invoke(m_combatPhase);

        if (m_playerIsStunned)
        {
            m_playerIsStunned = false;
            StartCoroutine(EndPlayerTurnThenStartEnemiesTurn());
            return;
        }

        if (MechanicsManager.Instance.Contains(m_playerController, MechanicType.IMPROVISE))
        {
            int drawExtra = MechanicsManager.Instance.GetMechanicsStack(m_playerController, MechanicType.IMPROVISE);
            IParamModifier<int> modifier = new AddValueModifier<int>(drawExtra);
            m_deck.SetDrawAmountOverride(modifier);

        }
        m_deck.DrawCards();


        HUD.Instance.SetEnergyWidgetAnimation(true);
        HUD.Instance.SetEndTurnWidgetAnimation(true);


        m_energy.ResetEnergy();
        StartCoroutine(m_uiManager.DisplayCurrentHand(m_deck.GetAllCardsIn(CardStorage.HAND)));
        foreach (CardDisplay cardDisplay in m_deck.GetAllCardsIn(CardStorage.HAND))
        {
            m_cardActions[cardDisplay].CardDrawn(cardDisplay.CardInDeck.GetCardData());
        }
    }

    public IEnumerator EndPlayerTurn()
    {
        m_cardsNotUsedThisTurn = m_deck.GetAllCardsIn(CardStorage.HAND);
        
        HUD.Instance.SetEnergyWidgetAnimation(false);
        HUD.Instance.SetEndTurnWidgetAnimation(false);
        GameplayEvents.SendGamePhaseChanged(EGamePhase.PLAYER_TURN_END);
        m_combatPhase = CombatPhase.TURN_END;
        OnCombatPhaseChanged?.Invoke(m_combatPhase);
        
        m_damageDoneToEnemiesThisTurn = 0;
        m_numberOfCardsPlayedThisTurn = 0; 
        m_extraCardsDrawnThisTurn = 0;
        m_isPlayersTurn = false;
        
        yield return new WaitForSeconds(0.5f);
        foreach (CardDisplay cardDisplay in m_deck.GetAllCardsIn(CardStorage.HAND))
        {
            m_cardActions[cardDisplay].NotUsed(cardDisplay.CardInDeck.GetCardData());
        }
        yield return StartCoroutine(m_uiManager.OnEndTurn());
        m_deck.DiscardCurrentHand();

        yield return new WaitUntil(() => !CardsQueue.Instance.IsProcessing);
    }

    public IEnumerator EndPlayerTurnThenStartEnemiesTurn()
    {
        yield return EndPlayerTurn();
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(StartEnemeiesTurn());
    }
    
    private IEnumerator StartEnemeiesTurn()
    {
        yield return new WaitForSeconds(0.5f);
        m_damageDoneToPlayersThisTurn = 0;
        
        m_combatPhase = CombatPhase.TURN_START;
        OnCombatPhaseChanged?.Invoke(m_combatPhase);
        GameplayEvents.SendGamePhaseChanged(EGamePhase.ENEMY_TURN_START);
        
        m_enemiesManager.PlayEnemiesTurns(OnEnemiesFinishedTurn);
    }
    
    
    public void OnEnemiesFinishedTurn()
    {
        GameplayEvents.SendGamePhaseChanged(EGamePhase.ENEMY_TURN_END);
        m_combatPhase = CombatPhase.TURN_END;
        OnCombatPhaseChanged?.Invoke(m_combatPhase);
        
        StartPlayerTurn();
    }

    public List<CardDisplay> DrawCard(int amount)
    {
        List<CardDisplay> cards = m_deck.DrawCards(amount);
        StartCoroutine(m_uiManager.DisplayCurrentHand(m_deck.GetAllCardsIn(CardStorage.HAND)));
        m_extraCardsDrawnThisTurn += amount;
        GameplayEvents.SendExtraCardDrawn();
        return cards;
    }

    public void MoveCardToHand(CardDisplay card)
    {
        m_deck.MoveCardTo(card, CardStorage.HAND);
        StartCoroutine(m_uiManager.DisplayCurrentHand(m_deck.GetAllCardsIn(CardStorage.HAND)));
    }

    public void DiscardCard(int amount)
    {
        if (amount <= 0)
        {
            Debug.Log("amound less than or equal to 0");
            return;
        }
        for (int i = 0; i < amount; i++)
        {
            CardDisplay card = Deck.DicardCard();
            if (card != null)
            {
                m_uiManager.DiscardCard(card);
            }
            //GameplayEvents.SendGamePhaseChanged(EGamePhase.CARD_DISCARDED);
        }
        
    }

    public void DiscardCard(CardDisplay card)
    {
        Deck.MoveCardTo(card, CardStorage.DISCARD_PILE);
        m_uiManager.DiscardCard(card);
        GameplayEvents.SendOnCardDiscarded(card);
        //GameplayEvents.SendGamePhaseChanged(EGamePhase.CARD_DISCARDED);
    }

    public void DiscardAllCardsInHand()
    {
        int numOfCardsInHand = m_deck.CountAllCardsIn(CardStorage.HAND);
        DiscardCard(numOfCardsInHand);
    }
    
    public CardDisplay SpawnCard(string name, CardStorage pileType)
    {
        CardDisplay cardInstance = CardSpawner.Instance.SpawnCardByName(name);
        if (cardInstance == null)
        {
            Debug.Log("null card");
            return null;
        }

        SetupCard(cardInstance, pileType);

        return cardInstance;
    }

    public CardDisplay SpawnCard(CardInDeckStateMachine card, CardStorage pileType)
    {
         CardDisplay cardInstance = CardSpawner.Instance.SpawnCard(card);
         if (cardInstance == null)
         {
             Debug.Log("null card");
             return null;
         }
         
         SetupCard(cardInstance, pileType);
         
         return cardInstance;       
    }
    
    public CardDisplay SpawnCard(BaseCardData card, CardStorage pileType)
    {
        CardDisplay cardInstance = CardSpawner.Instance.SpawnCard(card);
        if (cardInstance == null)
        {
            Debug.Log("null card");
            return null;
        }
        
        SetupCard(cardInstance, pileType);
        
        return cardInstance;
    }

    private void SetupCard(CardDisplay cardInstance, CardStorage pileType)
    {
        InstantiateAction(cardInstance);
        m_cardsUIManager.AddCard(cardInstance);

        if (pileType == CardStorage.HAND)
        {
            m_deck.InsertCard(cardInstance, CardStorage.DRAW_PILE, 0);
            DrawCard(1);
        }
        else
        {
            m_deck.InsertCard(cardInstance, pileType, 0);
        }
        
                
        // TODO: THIS IS NOT A GOOD SOLUTION CARD_DISPLAY SHOULD BE STATA MACHNIE
        if (cardInstance.CardInDeck.CurrentState.GetEnergy() >= m_energy.Current)
        {
            cardInstance.SetEnoughMana(true);
        }
        else
        {
            cardInstance.SetEnoughMana(true);
        }
        
        if (pileType == CardStorage.DISCARD_PILE)
        {
            //GameplayEvents.SendGamePhaseChanged(EGamePhase.CARD_DISCARDED);
        }

        GameplayEvents.SendGamePhaseChanged(EGamePhase.CARD_SPAWNED);
    }

    public void BanishCard(CardDisplay card)
    {
        m_deck.MoveCardTo(card, CardStorage.PERISHED_PILE);
        m_uiManager.DiscardCard(card);
    }

    public void PerishCard(CardDisplay card)
    {
        m_deck.MoveCardTo(card, CardStorage.PERISHED_PILE);
        m_uiManager.DiscardCard(card);
    }
    
    public void ShuffleDeckDrawPile()
    {
        m_deck.Shuffle(CardStorage.DRAW_PILE);
    }
    
    
    private void OnCardPicked(CardDisplay cardDisplay, Fighter target)
    {
        GameplayEvents.SendCardSelected(cardDisplay);
        bool cardPlaysTwice = DetermineIfCardGetsPlayedTwice(cardDisplay, target);
        StartCoroutine(PlayCard(cardDisplay, target, cardPlaysTwice));

    }

    private bool DetermineIfCardGetsPlayedTwice(CardDisplay cardDisplay, Fighter target)
    {
        for (int i = 0; i < m_cardsToPlayTwice.Count; i++)
        {
            //cehck index
            if (m_cardsToPlayTwice[i].Index == m_cardsPlayed.Count + 1)
            {
                return true;
            }
            //check name
            if (m_cardsToPlayTwice[i].Name == cardDisplay.CardInDeck.GetCardName())
            {
                return true;
            }
            //check type
            if (m_cardsToPlayTwice[i].Type != CardType.NONE && m_cardsToPlayTwice[i].Type == cardDisplay.CardInDeck.GetCardType())
            {
                return true;
            }
            //check pack
            if (m_cardsToPlayTwice[i].Packs != CardPacks.NONE && m_cardsToPlayTwice[i].Packs == cardDisplay.CardInDeck.GetCardPack())
            {
                return true;
            }
        }
        
        return false;
    }
    
    private IEnumerator PlayCard(CardDisplay cardDisplay, Fighter target, bool CardPlaysTwice)
    {
        m_pickedCard = cardDisplay;
        m_cardsPlayed.Add(m_pickedCard);
        m_energy.UseEnergy(m_pickedCard.CardInDeck.CurrentState.GetEnergy());
        m_deck.MoveCardTo(cardDisplay, CardStorage.PLAYING);
        
        BaseCardAction action = m_cardActions[cardDisplay];

        
        if (CardPlaysTwice)
        {
            CardsQueue.Instance.AddToQueue(() => {}, action, cardDisplay, target );
            CardsQueue.Instance.AddToQueue(() => { OnCardPlayFinished(cardDisplay); }, action, cardDisplay, target );
        }
        else
        {
            CardsQueue.Instance.AddToQueue(() => { OnCardPlayFinished(cardDisplay); }, action, cardDisplay, target );
        }

        yield break;
    }
    
    
    private void OnCardPlayFinished(CardDisplay cardDisplay)
    {
        m_numberOfCardsPlayedThisTurn++;
        if (cardDisplay.CardInDeck.CurrentState.DoesPerish())
        {
            GameplayEvents.SendGamePhaseChanged(EGamePhase.CARD_PERISHED);
            m_deck.MoveCardTo(cardDisplay, CardStorage.PERISHED_PILE);
        }
        else
        {
            m_deck.MoveCardTo(cardDisplay, CardStorage.DISCARD_PILE);
            GameplayEvents.SendGamePhaseChanged(EGamePhase.CARD_DISCARDED);
        }
        GameplayEvents.SendCardPlayFinished(cardDisplay);
        GameplayEvents.SendGamePhaseChanged(EGamePhase.CARD_PLAYED);
        OnCombatPhaseChanged?.Invoke(CombatPhase.CARD_PLAYED);
    }
    

    public void SubscribeToEnemyDamage(Fighter enemy)
    {
        enemy.HP.OnTookDamage += OnEnemyDamaged;
    }
    
    public void UnsubscribeFromEnemyDamage(Fighter enemy)
    {
        enemy.HP.OnTookDamage -= OnEnemyDamaged;
    }

    private void OnEnemyDamaged(int damage, bool Hasdied)
    {
        m_damageDoneToEnemiesThisTurn += damage;
        m_damageDoneToEnemiesOverAll += damage;
    }

    private void OnPlayerDamaged(int damage, bool Hasdied)
    {
        m_damageDoneToPlayersThisTurn += damage;
    }

    public void InstantiateAction(CardDisplay card)
    {
        //BaseCardAction actionInstance = Instantiate(card.MCardInDeck.m_cardData.ActionPrefab, this.transform);
        BaseCardAction actionInstance = card.CardInDeck.InstantiateCardAction();
        actionInstance.transform.SetParent(transform);
        actionInstance.Config(card);
        if (m_cardActions.ContainsKey(card))
        {
            m_cardActions[card] = actionInstance;
            return;
        }
        
        m_cardActions.Add(card, actionInstance);
    }

    public void OnAllEnemiesDead()
    {
        OnCombatOver?.Invoke(true);
    }
    
    public void OnPlayerDead(Fighter fighter)
    {
        OnCombatOver?.Invoke(false);
    }


    public void GodMode(bool isActive)
    {
        if (isActive)
        {
            m_playerController.HP.SetMax(500);
            m_playerController.Heal(500);
        }
    }

    public void SetCardToBePlayedTwice(int index)
    {
        CardsToPlayTwiceData data = new CardsToPlayTwiceData(index);
        m_cardsToPlayTwice.Add(data);
    }
    public void SetCardToBePlayedTwice(CardsToPlayTwiceData data)
    {
        m_cardsToPlayTwice.Add(data);
    }

    public void RemoveCardsPlayTwice(int index)
    {
        CardsToPlayTwiceData data = new CardsToPlayTwiceData(index);
        m_cardsToPlayTwice.Remove(data);
    }
    
    
    #if UNITY_EDITOR
    CardDisplay m_randCard = null;
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.B))
        {
            OnStanceSelected(Stance.BATTLE);
        }
        if(Input.GetKeyDown(KeyCode.E))
        {
            OnStanceSelected(Stance.BERSERKER);
        }
        if(Input.GetKeyDown(KeyCode.D))
        {
            OnStanceSelected(Stance.DEFENCIVE);
        }
        if(Input.GetKeyDown(KeyCode.N))
        {
            OnStanceSelected(Stance.NONE);
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            CardDisplay randCard = GameInfoHelper.GetRandomCard(CardStorage.HAND);
            GameActionHelper.SetCardEnergyOverride(randCard, ECardInDeckState.NORMAL, 0);
            
            m_randCard = randCard;
            //DrawCard(1);
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            if (m_randCard != null)
            {
                GameActionHelper.RemoveCardEnergyOverride(m_randCard, ECardInDeckState.NORMAL);
            }
        }

    }
    #endif
    protected override void Init()
    {
        
    }

    public void StunPlayer()
    {
        m_playerIsStunned = true;
        StartCoroutine(EndPlayerTurnThenStartEnemiesTurn());
    }

    public void SetPlayerReference(Fighter player)
    {
        if (player is not PlayerController playerController)
        {
            CustomDebug.LogError("Fighter is not a PlayerController", Categories.Fighters.Player.Root);
            return;
        }
        
        m_playerController = playerController;
    }

}