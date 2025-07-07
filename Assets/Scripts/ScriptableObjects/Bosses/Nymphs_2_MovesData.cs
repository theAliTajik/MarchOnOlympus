using UnityEngine;

[CreateAssetMenu(fileName = "Nymphs_2_Data", menuName = "Bosses/Nymphs 2 Move Data")]
public class Nymphs_2_MovesData : ScriptableObject
{
    public int HP;
   
    [Header("Move 1: Apply Bleed")]
    public int Move1Bleed;
}