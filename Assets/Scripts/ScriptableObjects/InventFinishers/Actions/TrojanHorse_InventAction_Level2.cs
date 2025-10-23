using System;
using Game;
using UnityEngine;

[CreateAssetMenu(fileName = "TrojanHorse_InventAction_Level2",  menuName = "Invent/InventAction/TrojanHorse_InventAction_Level2")]
public class TrojanHorse_InventAction_Level2 : InventAction
{
    public int Block;
    public int Damage;

    private IDamageable m_target;
    public override void Execute(IDamageable target, Action finishCallBack)
    {
        CustomDebug.Log($"executed {GetType().ToString()}", Categories.Combat.Invent.Root);
        GameActionHelper.AddMechanicToPlayer(Block, MechanicType.BLOCK);
        m_target = target;
        GameplayEvents.GamePhaseChanged += OnPhaseChange;
        finishCallBack?.Invoke();
    }

    private void OnPhaseChange(EGamePhase phase)
    {
        if(phase != EGamePhase.PLAYER_TURN_END) return;
        
        OnNextTurnEnd();
    }
    
    private void OnNextTurnEnd()
    {
        GameActionHelper.DamageFighter(m_target, GameInfoHelper.GetPlayer(), Damage, false);
    }
}
