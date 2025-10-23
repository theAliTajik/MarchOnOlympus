using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Genius_InventAction_Level2",  menuName = "Invent/InventAction/Genius_InventAction_Level2")]
public class Genius_InventAction_Level2 : InventAction
{
    public int CardCost;

    private CardDisplay m_modifiedCard;

    public override void Execute(IDamageable target, Action finishCallBack)
    {
        CustomDebug.Log($"executed {GetType().ToString()}", Categories.Combat.Invent.Root);
        
        GameActionHelper.SetNextCardToBePlayedTwice();

        CardDisplay randCard = GameInfoHelper.GetRandomCard(CardStorage.HAND);

        var modifier = new SetValueModifier<int>(CardCost);
        GameActionHelper.ModifyCardEnergy(randCard, modifier);
        m_modifiedCard = randCard;
        GameplayEvents.GamePhaseChanged += OnPhaseChange;
        finishCallBack?.Invoke();
    }

    private void OnPhaseChange(EGamePhase phase)
    {
        if (phase != EGamePhase.PLAYER_TURN_START) return;
        
        GameActionHelper.RemoveCardEnergyOverride(m_modifiedCard, ECardInDeckState.NORMAL);
        GameActionHelper.RemoveCardEnergyOverride(m_modifiedCard, ECardInDeckState.STANCE);
    }
}
