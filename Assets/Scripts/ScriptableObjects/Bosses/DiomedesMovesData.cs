using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DiomedesData", menuName = "Bosses/Diomedes Move Data")]
public class DiomedesMovesData : ScriptableObject
{
    public int HP;
   
    [Header("Move 1: Hit")]
    public int Move1Damage;

    [Header("Move 2: Hit, Block")]
    public int Move2Damage;
    public int Move2Block;
    
    [Header("MISCS:")] 
    public FighterHP.TriggerPercentage Phase1Trigger;
    public int Phase1Damage;
}