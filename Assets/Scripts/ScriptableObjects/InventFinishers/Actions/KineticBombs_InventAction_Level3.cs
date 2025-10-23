using System;
using Game;
using UnityEngine;

[CreateAssetMenu(fileName = "KineticBombs_InventAction_Level3",  menuName = "Invent/InventAction/KineticBombs_InventAction_Level3")]
public class KineticBombs_InventAction_Level3 : InventAction
{
    public int Bleed;
    public string Perk;
    public string Perk2;

    public override void Execute(IDamageable target, Action finishCallBack)
    {
        CustomDebug.Log($"executed {GetType().ToString()}", Categories.Combat.Invent.Root);
        
        if (target is IHaveMechanics owner)
        {
            GameActionHelper.AddMechanicToOwner(owner, Bleed, MechanicType.BLEED);
        }
        
        GameActionHelper.AddPerk(Perk);
        GameActionHelper.AddPerk(Perk2);
        finishCallBack?.Invoke();
    }
}
