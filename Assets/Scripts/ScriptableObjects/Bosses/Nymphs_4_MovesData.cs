using UnityEngine;

[CreateAssetMenu(fileName = "Nymphs_4_Data", menuName = "Bosses/Nymphs 4 Move Data")]
public class Nymphs_4_MovesData : ScriptableObject
{
    public int HP;
   
    [Header("Move 1: Restore 10 Each")]
    public int Move1Restore;
}