using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Trapmaster_InventAction_Level2",  menuName = "Invent/InventAction/Trapmaster_InventAction_Level2")]
public class Trapmaster_InventAction_Level2 : InventAction
{
    public string Perk;
    public int Discard;
    public int Draw;

    public override void Execute(IDamageable target, Action finishCallBack)
    {
        CustomDebug.Log($"executed {GetType().ToString()}", Categories.Combat.Invent.Root);
        
        GameActionHelper.AddPerk(Perk);
        GameActionHelper.DiscardCard(Discard);
        GameActionHelper.DrawCards(Draw);
        finishCallBack?.Invoke();
    }
}
