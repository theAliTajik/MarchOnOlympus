using System;
using Game;
using UnityEngine;

[CreateAssetMenu(fileName = "Barricade_InventAction_Level2",  menuName = "Invent/InventAction/Barricade_InventAction_Level2")]
public class Barricade_InventAction_Level2 : InventAction
{
    public int Block;
    public int Thorns;

    public override void Execute(IDamageable target, Action finishCallBack)
    {
        CustomDebug.Log($"executed {GetType().ToString()}", Categories.Combat.Invent.Root);
        GameActionHelper.AddMechanicToPlayer(Block, MechanicType.BLOCK);
        GameActionHelper.AddMechanicToPlayer(Thorns, MechanicType.THORNS);
        finishCallBack?.Invoke();
    }
}
