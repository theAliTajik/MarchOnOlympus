using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Trapmaster_InventAction_Level3",  menuName = "Invent/InventAction/Trapmaster_InventAction_Level3")]
public class Trapmaster_InventAction_Level3 : InventAction
{
    public string Perk;

    public override void Execute(IDamageable target, Action finishCallBack)
    {
        CustomDebug.Log($"executed {GetType().ToString()}", Categories.Combat.Invent.Root);

        int NumOfCardsInHand = GameInfoHelper.GetNumOfCardsInHand();
        
        GameActionHelper.DiscardAllCardsInHand();
        GameActionHelper.DrawCards(NumOfCardsInHand);
        finishCallBack?.Invoke();
    }
}
