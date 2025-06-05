using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SoulMasterMovesData", menuName = "Bosses/Soul Master Moves Data")]
public class SoulMasterMovesData : ScriptableObject
{
    public int HP;

    [Header("Move 1: summon x souls")] 
    public int Move1NumOfSoulsToSpawn;

    [Header("Move 2: Buff self x str, Restore y")] 
    public int Move2StrGain;
    public int Move2Restore;

    [Header("Move 3: bleed x")]
    public int Move3BleedGain;
   
    [Header("Move 4: hit x piercing")]
    public int Move4Damage;
    
    [Header("Move 5: hit x, apply daze")]
    public int Move5Damage;
    public int Move5DazeGain;
    
    [Header("MISCS:")] 
    public FighterHP.TriggerPercentage Phase1PercentageTrigger;
    public FighterHP.TriggerPercentage Phase2PercentageTrigger;


    public int MaxTurnWhenSoulAreLessThanOne;
    public int MaxNumOfSouls;
}
