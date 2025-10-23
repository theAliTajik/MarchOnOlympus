using System;
using Game;
using UnityEngine;

[CreateAssetMenu(fileName = "KineticBombs_InventAction_Level1",  menuName = "Invent/InventAction/KineticBombs_InventAction_Level1")]
public class KineticBombs_InventAction_Level1 : InventAction
{
    public int Bleed;

    public override void Execute(IDamageable target, Action finishCallBack)
    {
        CustomDebug.Log($"executed {GetType().ToString()}", Categories.Combat.Invent.Root);

        if (target is IHaveMechanics owner)
        {
            GameActionHelper.AddMechanicToOwner(owner, Bleed, MechanicType.BLEED);
        }
        finishCallBack?.Invoke();
    }
}
