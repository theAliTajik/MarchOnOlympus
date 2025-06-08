using UnityEngine;

[CreateAssetMenu(fileName = "Archer1Data", menuName = "Bosses/Archer 1 Move Data")]
public class Archer1MovesData : ScriptableObject
{
    public int HP;
   
    [Header("Move 1: Hit")]
    public int Move1Damage;

    [Header("Move 2: 3 str to all allies")]
    public int Move2Str;
}