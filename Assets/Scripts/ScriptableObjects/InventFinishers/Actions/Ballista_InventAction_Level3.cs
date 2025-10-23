using System;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "Ballista_InventAction_Level3",  menuName = "Invent/InventAction/Ballista_InventAction_Level3")]
public class Ballista_InventAction_Level3 : InventAction
{
    public int LowestDamage;
    public int HighestDamage;
    public string Perk;

    public override void Execute(IDamageable target, Action finishCallBack)
    {
        CustomDebug.Log($"executed {GetType().ToString()}", Categories.Combat.Invent.Root);
        int damage =  Random.Range(LowestDamage, HighestDamage+1);
        GameActionHelper.DamageFighter(target, GameInfoHelper.GetPlayer(), damage, false);
        GameActionHelper.AddPerk(Perk);
        finishCallBack?.Invoke();
    }
}
