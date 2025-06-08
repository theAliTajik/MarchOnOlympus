using UnityEngine;

[CreateAssetMenu(fileName = "Archer2Data", menuName = "Bosses/Archer 2 Move Data")]
public class Archer2MovesData : ScriptableObject
{
    public int HP;
   
    [Header("Move 1: 5 Thorns to Warrior Level 2")]
    public int Move1Thorns;

    [Header("Move 2: Hit 15")]
    public int Move2Damage;
}