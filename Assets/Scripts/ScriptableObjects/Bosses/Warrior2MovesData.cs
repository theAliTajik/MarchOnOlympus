using UnityEngine;

[CreateAssetMenu(fileName = "Warrior2Data", menuName = "Bosses/Warrior2 Move Data")]
public class Warrior2MovesData : ScriptableObject
{
    public int HP;
   
    [Header("Move 1: Fortify to all allies")]
    public int Move1Fortify;

    [Header("Move 2: Hit 10")]
    public int Move2Damage;
}