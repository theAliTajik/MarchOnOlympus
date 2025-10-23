using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Genius_InventAction_Level1",  menuName = "Invent/InventAction/Genius_InventAction_Level1")]
public class Genius_InventAction_Level1 : InventAction
{

    public override void Execute(IDamageable target, Action finishCallBack)
    {
        CustomDebug.Log($"executed {GetType().ToString()}", Categories.Combat.Invent.Root);

        GameActionHelper.SetNextCardToBePlayedTwice();
        finishCallBack?.Invoke();
    }
}
