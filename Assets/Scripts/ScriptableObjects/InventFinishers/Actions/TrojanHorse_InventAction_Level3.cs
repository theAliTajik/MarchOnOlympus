using System;
using Game;
using UnityEngine;

[CreateAssetMenu(fileName = "TrojanHorse_InventAction_Level3",  menuName = "Invent/InventAction/TrojanHorse_InventAction_Level3")]
public class TrojanHorse_InventAction_Level3 : InventAction
{
    public int Block;
    public int Damage;
    public string Perk;

    private IDamageable m_target;
    public override void Execute(IDamageable target, Action finishCallBack)
    {
        CustomDebug.Log($"executed {GetType().ToString()}", Categories.Combat.Invent.Root);
        GameActionHelper.AddMechanicToPlayer(Block, MechanicType.BLOCK);
        m_target = target;
        GameplayEvents.GamePhaseChanged += OnPhaseChange;
        GameActionHelper.AddPerk(Perk);
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
