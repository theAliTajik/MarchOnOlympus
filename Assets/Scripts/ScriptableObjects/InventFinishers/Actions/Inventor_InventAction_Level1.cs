using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Inventor_InventAction_Level1",  menuName = "Invent/InventAction/Inventor_InventAction_Level1")]
public class Inventor_InventAction_Level1 : InventAction
{
    public string Perk;

    public override void Execute(IDamageable target, Action finishCallBack)
    {
        CustomDebug.Log($"executed {GetType().ToString()}", Categories.Combat.Invent.Root);
        
        GameActionHelper.AddPerk(Perk);
        finishCallBack?.Invoke();
    }
}
