using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponsMasterData", menuName = "Bosses/Weapons Master Moves Data")]
public class WeaponsMasterMovesData : ScriptableObject
{
    public int HP;
   
    [Header("Phase 1")]
    
    [Header("Move 1: Hit")]
    public int Move1Damage;

    [Header("Move 2: Send 3 cards")] 
    public BaseCardData Move2Card;
    public int Move2NumOfCards;
   
    [Header("Phase 2")]
    
    [Header("Move 3: Hit random amount")]
    public int[] Move3Damages = new int[5];
   
    
    [Header("Phase 3")]

    [Header("Move 4: Hit, Block")]
    public int Move4Damage;
    public int Move4Block;
    
    [Header("Move 5: Restore, Block")]
    public int Move5Restore;
    public int Move5Block;

    [Header("MISCS:")] 
    public FighterHP.TriggerPercentage Phase2PercentageTrigger;
    public FighterHP.TriggerPercentage Phase3PercentageTrigger;


}