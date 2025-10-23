using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Genius_InventAction_Level3",  menuName = "Invent/InventAction/Genius_InventAction_Level3")]
public class Genius_InventAction_Level3 : InventAction
{
    public int CardCost;

    public override void Execute(IDamageable target, Action finishCallBack)
    {
        CustomDebug.Log($"executed {GetType().ToString()}", Categories.Combat.Invent.Root);
        
        GameActionHelper.SetNextCardToBePlayedTwice();

        CardDisplay randCard = GameInfoHelper.GetRandomCard(CardStorage.HAND);

        var modifier = new SetValueModifier<int>(CardCost);
        GameActionHelper.ModifyCardEnergy(randCard, modifier);
        finishCallBack?.Invoke();
    }
}
