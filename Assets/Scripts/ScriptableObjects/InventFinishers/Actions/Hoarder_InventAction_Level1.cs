using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Hoarder_InventAction_Level1",  menuName = "Invent/InventAction/Hoarder_InventAction_Level1")]
public class Hoarder_InventAction_Level1 : InventAction
{
    public int Discard;
    public int Draw;

    public override void Execute(IDamageable target, Action finishCallBack)
    {
        CustomDebug.Log($"executed {GetType().ToString()}", Categories.Combat.Invent.Root);
        GameActionHelper.DiscardCard(Discard);
        GameActionHelper.DrawCards(Draw);
        finishCallBack?.Invoke();
    }
}
