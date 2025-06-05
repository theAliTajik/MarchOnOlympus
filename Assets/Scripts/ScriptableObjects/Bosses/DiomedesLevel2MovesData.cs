using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DiomedesLevel2MovesData", menuName = "Bosses/Diomedes Level2 Moves Data")]
public class DiomedesLevel2MovesData : ScriptableObject
{
    public int HP;
   
    [Header("Move 1: Hit")]
    public int Move1Damage;
    public int Move1Bleed;

    [Header("Move 2: Hit, Block")]
    public int Move2Damage;
    public int Move2Block;
    public int Move2Restore;

    //[Header("MISCS:")] 

}