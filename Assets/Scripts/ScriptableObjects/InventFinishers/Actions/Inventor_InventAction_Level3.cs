using System;
using System.Collections.Generic;
using Game;
using UnityEngine;

[CreateAssetMenu(fileName = "Inventor_InventAction_Level3",  menuName = "Invent/InventAction/Inventor_InventAction_Level3")]
public class Inventor_InventAction_Level3 : InventAction
{
    public string Perk;
    public int Invent;
    public int Bleed;

    public override void Execute(IDamageable target, Action finishCallBack)
    {
        CustomDebug.Log($"executed {GetType().ToString()}", Categories.Combat.Invent.Root);
        
        GameActionHelper.AddPerk(Perk);
        GameActionHelper.GainInvent(Invent);

        List<Fighter> allEnemies = GameInfoHelper.GetAllEnemies();
        foreach (Fighter enemy in allEnemies)
        {
            GameActionHelper.AddMechanicToFighter(enemy, Bleed, MechanicType.BLEED);
        }
        finishCallBack?.Invoke();
    }
}
