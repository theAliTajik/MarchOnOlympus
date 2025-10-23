using System;
using Game;
using UnityEngine;

[CreateAssetMenu(fileName = "Hoarder_InventAction_Level3",  menuName = "Invent/InventAction/Hoarder_InventAction_Level3")]
public class Hoarder_InventAction_Level3 : InventAction
{
    public int Discard;
    public int Draw;
    public int Strength;
    public int DiscardedCardsMultiplier;

    public override void Execute(IDamageable target, Action finishCallBack)
    {
        CustomDebug.Log($"executed {GetType().ToString()}", Categories.Combat.Invent.Root);
        int currenNumOfCardsInHand = GameInfoHelper.GetNumOfCardsInHand();
        
        GameActionHelper.DiscardCard(Discard);
        GameActionHelper.DrawCards(Draw);
        
        GameActionHelper.AddMechanicToPlayer(Strength, MechanicType.STRENGTH);

        int numOfDiscardedCards = Discard;
        if (numOfDiscardedCards < currenNumOfCardsInHand)
        {
            numOfDiscardedCards = currenNumOfCardsInHand;
        }
        
        int damage = numOfDiscardedCards * DiscardedCardsMultiplier;
        GameActionHelper.DamageFighter(target, GameInfoHelper.GetPlayer(), damage);
        finishCallBack?.Invoke();

    }
}
