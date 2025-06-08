using UnityEngine;

[CreateAssetMenu(fileName = "PriestMovesData", menuName = "Bosses/Priest Moves Data")]
public class PriestMovesData : ScriptableObject
{
    public int HP;

    [Header("Move 1: Restore 15 to the lowest hp target")] 
    public int Move1Restore;

    [Header("Move 2: Rejuvenation on Warrior level 2")] 
    public int Move2Rejuvenation;
}
