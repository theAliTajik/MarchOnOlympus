using UnityEngine;

[CreateAssetMenu(fileName = "WarriorLevel2Data", menuName = "Bosses/WarriorLevel2 Move Data")]
public class WarriorLevel2MovesData : ScriptableObject
{
    public int HP;
   
    [Header("Move 1: Taunt")]
    public int Move1Taunt;

    [Header("Move 2: Hit 60 - Alive Ally Countx20")]
    public int Move2Damage;
}