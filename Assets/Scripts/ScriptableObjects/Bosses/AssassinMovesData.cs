using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AssassinData", menuName = "Bosses/Assassin Move Data")]
public class AssassinMovesData : ScriptableObject
{
    public int HP;
   
    [Header("Move 1: Hit 20")]
    public int Move1Damage;

    [Header("Move 2: Bleed 3, Armor 5")]
    public int Move2Bleed;
    public int Move2Block;
    
    [Header("Move 3: Send 3 cards into the players deck (Card: 1 Mana - Perish : If you don’t use it, it deals 3 damage to player)")]
    public BaseCardData Move3Card;
    public int Move3NumOfCards;

    [Header("MISCS:")] 
    public FighterHP.TriggerPercentage Phase1PercentageTrigger;
    public FighterHP.TriggerPercentage Phase2PercentageTrigger;
    public int PhaseOneUntilThisPrecentOfHP;
    public int PhaseTwoUntilThisPrecentOfHP;
    public int EachCardUsedByPlayerDamageToPlayer;


}