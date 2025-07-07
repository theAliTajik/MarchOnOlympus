using UnityEngine;

[CreateAssetMenu(fileName = "Nymphs_1_Data", menuName = "Bosses/Nymphs 1 Move Data")]
public class Nymphs_1_MovesData : ScriptableObject
{
    public int HP;
   
    [Header("Move 1: Apply Vulnerable")]
    public int Move1Vulnerable;
}