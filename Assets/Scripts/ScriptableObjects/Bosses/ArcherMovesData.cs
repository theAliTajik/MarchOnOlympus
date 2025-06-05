using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ArcherData", menuName = "Bosses/Archer Move Data")]
public class ArcherMovesData : ScriptableObject
{
    public int HP;
   
    [Header("Move 1: Hit And Bleed")]
    public int Move1Damage;
    public int Move1Bleed;

    [Header("Move 2: Hit Twice")]
    public int Move2Damage;

    [Header("MISCS:")] 
    public int StrRecievedWhenWarriorsDeath;

    public string DiomedesLevel2Id;

}