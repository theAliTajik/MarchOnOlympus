using System;
using Game;
using UnityEngine;

[CreateAssetMenu(fileName = "FireBombs_InventAction_Level1",  menuName = "Invent/InventAction/FireBombs_InventAction_Level1")]
public class FireBombs_InventAction_Level1 : InventAction
{
    public int Damage;
    public int Burn;

    public override void Execute(IDamageable target, Action finishCallBack)
    {
        CustomDebug.Log($"executed {GetType().ToString()}", Categories.Combat.Invent.Root);
        GameActionHelper.DamageFighter(target, GameInfoHelper.GetPlayer(), Damage);
        GameActionHelper.AddMechanicToPlayer(Burn, MechanicType.BURN);
        
        if (target is IHaveMechanics owner)
        {
            GameActionHelper.AddMechanicToOwner(owner, Burn, MechanicType.BURN);
        }
        finishCallBack?.Invoke();
    }
}
