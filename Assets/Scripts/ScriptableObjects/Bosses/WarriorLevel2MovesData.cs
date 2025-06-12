using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "WarriorLevel2Data", menuName = "Bosses/WarriorLevel2 Move Data")]
public class WarriorLevel2MovesData : ScriptableObject
{
    public int HP;
   
    [Header("Move 1: Taunt")]
    public int Move1Taunt;

    [Header("Move 2: Hit 60 - Alive Ally Countx20")]
    public int Move2Damage;

    public int Move2AllyDamageMultiplier;
    public int Move2MaxNumOfAllies;
}