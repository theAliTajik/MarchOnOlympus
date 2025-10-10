using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;
using UnityEngine.EventSystems;

public class MentalQuickness_STARCardAction : BaseCardAction
{
    private MentalQuickness_STARCard m_data;
    
    public override void Play(BaseCardData cardData, Action finishCallback, Fighter target, CardDisplay cardDisplay)
    {
        StartCoroutine(WaitAndExecute(finishCallback, 2f,cardData, target, cardDisplay));
    }

    private IEnumerator WaitAndExecute(Action finishCallback, float delay, BaseCardData cardData, Fighter target, CardDisplay cardDisplay)
    {
        m_data = (MentalQuickness_STARCard)cardData;

        int invent = GameInfoHelper.GetInvent();
        int block = invent / m_data.InventDivisor;
        
        GameActionHelper.AddMechanicToPlayer(block, MechanicType.BLOCK);


        List<BaseCardData> cards = GameInfoHelper.GetCardsWithNameFromDB("Arrow", contains: true);
        int randCardIndex = UnityEngine.Random.Range(0, cards.Count);
        BaseCardData card = cards[randCardIndex];
        GameActionHelper.SpawnCard(card, CardStorage.DRAW_PILE);
        
        if (CombatManager.Instance.CurrentStance == cardData.MStance)
        {
            
        }
        
        finishCallback?.Invoke();
        yield break;
    }

}