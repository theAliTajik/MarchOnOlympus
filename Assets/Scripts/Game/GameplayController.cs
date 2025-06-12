using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameplayController : Singleton<GameplayController>
{
    
    [SerializeField] private CardSpawner m_cardSpawner;
    [SerializeField] private CardsUIManager m_cardsUIManager;
    [SerializeField] private PerksManager m_perksManager;
    [SerializeField] private CombatManager m_combatManager;
    [SerializeField] private HUD m_HUD;
    [SerializeField] private RewardsManager m_rewardsManager;
    [SerializeField] private EnemiesManager m_enemiesManager;
    [SerializeField] private Deck m_deck;
    [SerializeField] private string m_deckClientIdTest;
    [SerializeField] private PerksDisplay m_perksDisplay;

    private List<CardInDeckStateMachine> m_startingCards;
    
    public List<CardInDeckStateMachine> StartingCards {
        get { return m_startingCards; }
    }

    private string m_currentDeck;

    private void Start()
    {
        DeckTemplates.Deck template;
        bool isDevTestingTemplate = false;
        DeckTemplates.LoadAllDecks();
        if (!string.IsNullOrEmpty(GameSessionParams.DeckTemplateClientId))
        {
            template = DeckTemplates.FindById(GameSessionParams.DeckTemplateClientId);
            if (GameSessionParams.DeckTemplateClientId.ToLower().StartsWith("Dev Testing"))
            {
                isDevTestingTemplate = true;
            }
            m_currentDeck = GameSessionParams.DeckTemplateClientId;
        }
        else
        {
            template = DeckTemplatesDb.Instance.FindById(m_deckClientIdTest);
            isDevTestingTemplate = true;
            m_currentDeck = m_deckClientIdTest;
        }


        m_startingCards = new List<CardInDeckStateMachine>(template.CardsInDeck);
        if (!isDevTestingTemplate)
        {
            ShuffleList(m_startingCards);
        }

#if UNITY_EDITOR
        if (isDevTestingTemplate)
        {

            // make two more of fisrt card for testing :
            for (int i = 0; i < 2; i++)
            {
                if (m_startingCards[0] == null)
                {
                    Debug.LogError("null card");
                }

                CardDisplay cardInstance = m_cardSpawner.SpawnCard(m_startingCards[0]);
                m_deck.CardPiles[CardStorage.DRAW_PILE].Cards.Add(cardInstance);
                m_combatManager.InstantiateAction(cardInstance);
                m_cardsUIManager.AddCard(cardInstance);
            }
        }
#endif        

        foreach (CardInDeckStateMachine card in m_startingCards)
        {
            if (card == null)
            {
                Debug.LogError("null card");
            }

            CardDisplay cardInstance = m_cardSpawner.SpawnCard(card);
            m_deck.CardPiles[CardStorage.DRAW_PILE].Cards.Add(cardInstance);
            m_combatManager.InstantiateAction(cardInstance);
            m_cardsUIManager.AddCard(cardInstance);
        }


        m_cardsUIManager.CalculateWidth();
        
        m_perksManager.LoadPerks();
        m_combatManager.StartCombat();
        m_combatManager.OnCombatOver += GameOver;
        GameplayEvents.RewardedCardSelected += AddRewardedCard;
    }

    private void AddRewardedCard(BaseCardData cardData)
    {
        string cardId = cardData.Name.Replace(" ", "");
        DeckTemplates.AddCardToDeck(m_currentDeck, cardId, -1);
        Debug.Log("saveed this card: " + cardId + " in this deck: " + m_currentDeck);
    }

    private void ShuffleList<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            // Pick a random index from 0 to i
            int randomIndex = Random.Range(0, i + 1);

            // Swap elements
            T temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
    
        
    public void GameOver(bool victory)
    {
        if (victory)
        {
            MapNodeSaveManager.MarkNodeCompleted();
        }
        
        m_cardsUIManager.EndGame();
        m_HUD.GameOver(victory);
        
        m_rewardsManager.GiveReward();
        Debug.Log("Game Over. victory: " + victory);
    }
    
    


    protected override void Init()
    {
        
    }
}
