using System;
using Game;
using UnityEngine;

[CreateAssetMenu(fileName = "PoisonBombs_InventAction_Level1",  menuName = "Invent/InventAction/PoisonBombs_InventAction_Level1")]
public class PoisonBombs_InventAction_Level1 : InventAction
{

    public override void Execute(IDamageable target, Action finishCallBack)
    {
        CustomDebug.Log($"executed {GetType().ToString()}", Categories.Combat.Invent.Root);
        
        if (target is IHaveMechanics owner)
        {
            GameActionHelper.AddMechanicToOwner(owner, stack:1, MechanicType.DAZE);
        }
        finishCallBack?.Invoke();
    }
}
