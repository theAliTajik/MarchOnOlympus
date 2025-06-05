using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class RewardsManager : MonoBehaviour
{
    [SerializeField] private GameObject m_rewardsPanel;
    [SerializeField] private GameObject m_cardRewardsContainer;
    [SerializeField] private PoolCardClickableItem m_cardPool;
    
    
    
    private int m_honorAmount = 10;

    private const int numOfRewards = 3;
    public void GiveReward()
    {
        GiveCardReward();
        GiveHonorReward();
        m_rewardsPanel.gameObject.SetActive(true);
    }

    private void GiveCardReward()
    {
        List<CardClickableItem> rewards = new List<CardClickableItem>();
        for (int i = 0; i < numOfRewards; i++)
        {
            rewards.Add(m_cardPool.GetItem());
            
            BaseCardData cardData = SelectRandomCard();
            
            rewards[i].Configure(cardData);
            rewards[i].OnClick += OnCardRewardSelected;
            rewards[i].transform.SetParent(m_cardRewardsContainer.transform, false);
            rewards[i].transform.localScale = Vector3.one;
            rewards[i].RefreshUI();
        }
    }

    private void GiveHonorReward()
    {
        GameProgress.Instance.Data.Honor += m_honorAmount;
    }
    
    
    private BaseCardData SelectRandomCard()
    { 
        CardsDb.CardsInfo randCard = CardsDb.Instance.AllCards[Random.Range(0, CardsDb.Instance.AllCards.Count)];
        while (!randCard.IsImplemented)
        {
            randCard = CardsDb.Instance.AllCards[Random.Range(0, CardsDb.Instance.AllCards.Count)];
        }
        return randCard.CardData;
    }

    private void OnCardRewardSelected(CardClickableItem item)
    {
        Debug.Log("card clicked: " + item.CardInDeck.GetCardName());
         
        GameplayEvents.SendRewarderCardSelected(item.CardInDeck.GetCardData());
        
        SceneController.Instance.LoadScene(Scenes.Map);
    }
}
