
using Game;
using UnityEngine;

public class LastNymphAliveAction
{
    public void Execute(LastNymphAliveMoveData data, Fighter sender)
    {
        GameActionHelper.AddMechanicToPlayer(data.Vulnerable, MechanicType.VULNERABLE);
        GameActionHelper.AddMechanicToPlayer(data.Bleed, MechanicType.BLEED);
        
        GameActionHelper.DamageFighter(GameInfoHelper.GetPlayer(), sender, data.Damage);
        GameActionHelper.HealFighter(sender, data.Restore);
    }
}
