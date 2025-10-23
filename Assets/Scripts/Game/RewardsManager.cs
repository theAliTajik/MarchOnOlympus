using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class RewardsManager : Singleton<RewardsManager>
{
    [SerializeField] private GameObject m_rewardsPanel;
    [SerializeField] private GameObject m_cardRewardsContainer;
    [SerializeField] private PoolCardClickableItem m_cardPool;
    
    
    private int m_honorAmount = 10;

    private const int numOfRewards = 3;

    protected override void Awake()
    {
        base.Awake();
        m_rewardsPanel.SetActive(false);
    }

    public void GiveReward()
    {
        GiveRandomCardReward();
        GiveHonorReward();
    }

    private void GiveRandomCardReward()
    {
        List<BaseCardData> randomCards = new();
        for (int i = 0; i < numOfRewards; i++)
        {
            randomCards.Add(SelectRandomCard());
        }
        GiveCardReward(randomCards);
    }

    public void GiveCardReward(List<BaseCardData> cards)
    {
        if (cards == null || cards.Count == 0)
        {
            CustomDebug.LogWarning("Give card reward was called with no cards", Categories.Rewards.Root);
            return;
        }
        
        List<CardClickableItem> rewards = new List<CardClickableItem>();
        for (int i = 0; i < cards.Count; i++)
        {
            rewards.Add(m_cardPool.GetItem());
            
            BaseCardData cardData = cards[i];
            
            rewards[i].Configure(cardData);
            rewards[i].OnClick += OnCardRewardSelected;
            rewards[i].transform.SetParent(m_cardRewardsContainer.transform, false);
            rewards[i].transform.localScale = Vector3.one;
            rewards[i].RefreshUI();
        }
        m_rewardsPanel.gameObject.SetActive(true);
    }

    private void DisplaySelection(List<BaseCardData> rewards)
    {
        List<CardClickableItem> clickableCardItems = new List<CardClickableItem>();
        for (int i = 0; i < rewards.Count; i++)
        {
            clickableCardItems.Add(m_cardPool.GetItem());

            BaseCardData cardData = rewards[i];
            
            clickableCardItems[i].Configure(cardData);
            clickableCardItems[i].OnClick += OnCardRewardSelected;
            clickableCardItems[i].transform.SetParent(m_cardRewardsContainer.transform, false);
            clickableCardItems[i].transform.localScale = Vector3.one;
            clickableCardItems[i].RefreshUI();
        }
    }

    private void GiveHonorReward()
    {
        GameProgress.Instance.Data.Honor += m_honorAmount;
    }
    
    
    private BaseCardData SelectRandomCard()
    {
        BaseCardData randCard = CardsDb.Instance.GetRandom();
        return randCard;
    }

    private void OnCardRewardSelected(CardClickableItem item)
    {
        Debug.Log("card clicked: " + item.CardInDeck.GetCardName());
         
        GameplayEvents.SendRewarderCardSelected(item.CardInDeck.GetCardData());
        GameplayEvents.SendOnCardRewardSelected(item.CardInDeck.GetCardData());
        
        m_rewardsPanel.gameObject.SetActive(false);
        // SceneController.Instance.LoadScene(Scenes.Map);
    }

    protected override void Init()
    {
        
    }
}
