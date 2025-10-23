using System;
using Game;
using UnityEngine;

[CreateAssetMenu(fileName = "Hoarder_InventAction_Level2",  menuName = "Invent/InventAction/Hoarder_InventAction_Level2")]
public class Hoarder_InventAction_Level2 : InventAction
{
    public int Discard;
    public int Draw;

    public override void Execute(IDamageable target, Action finishCallBack)
    {
        CustomDebug.Log($"executed {GetType().ToString()}", Categories.Combat.Invent.Root);
        GameActionHelper.DiscardCard(Discard);
        GameActionHelper.DrawCards(Draw);

        Fighter player = GameInfoHelper.GetPlayer();
        int currentFortified = GameInfoHelper.GetMechanicStack(player, MechanicType.FORTIFIED);
        
        GameActionHelper.AddMechanicToPlayer(currentFortified, MechanicType.FORTIFIED);
        finishCallBack?.Invoke();
    }
}
