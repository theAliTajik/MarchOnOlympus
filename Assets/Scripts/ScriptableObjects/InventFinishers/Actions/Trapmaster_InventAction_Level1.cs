using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Trapmaster_InventAction_Level1",  menuName = "Invent/InventAction/Trapmaster_InventAction_Level1")]
public class Trapmaster_InventAction_Level1 : InventAction
{
    public string Perk;

    public override void Execute(IDamageable target, Action finishCallBack)
    {
        CustomDebug.Log($"executed {GetType().ToString()}", Categories.Combat.Invent.Root);
        
        GameActionHelper.AddPerk(Perk);
        finishCallBack?.Invoke();
    }
}
