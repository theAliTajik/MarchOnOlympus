using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Warrior1Data", menuName = "Bosses/Warrior1 Move Data")]
public class Warrior1MovesData : ScriptableObject
{
    public int HP;
   
    [Header("Move 1: Block 10 to all allies")]
    public int Move1Block;

    [Header("Move 2: Hit 10")]
    public int Move2Damage;
}