using UnityEngine;

[CreateAssetMenu(fileName = "Nymphs_3_Data", menuName = "Bosses/Nymphs 3 Move Data")]
public class Nymphs_3_MovesData : ScriptableObject
{
    public int HP;
   
    [Header("Move 1: Hit 15")]
    public int Move1Damage;
}