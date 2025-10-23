using System;
using Game;
using UnityEngine;

[CreateAssetMenu(fileName = "Barricade_InventAction_Level1",  menuName = "Invent/InventAction/Barricade_InventAction_Level1")]
public class Barricade_InventAction_Level1 : InventAction
{
    public int Block;

    public override void Execute(IDamageable target, Action finishCallBack)
    {
        CustomDebug.Log($"executed {GetType().ToString()}", Categories.Combat.Invent.Root);
        GameActionHelper.AddMechanicToPlayer(Block, MechanicType.BLOCK);
        finishCallBack?.Invoke();
    }
}
