using System;
using Game;
using UnityEngine;

[CreateAssetMenu(fileName = "FireBombs_InventAction_Level3",  menuName = "Invent/InventAction/FireBombs_InventAction_Level3")]
public class FireBombs_InventAction_Level3 : InventAction
{
    public int Burn;

    public override void Execute(IDamageable target, Action finishCallBack)
    {
        CustomDebug.Log($"executed {GetType().ToString()}", Categories.Combat.Invent.Root);
        
        if (target is IHaveMechanics owner)
        {
            GameActionHelper.AddMechanicToOwner(owner, Burn, MechanicType.BURN);
        }
        finishCallBack?.Invoke();
    }
}
