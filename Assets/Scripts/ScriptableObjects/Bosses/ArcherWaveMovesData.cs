using UnityEngine;

[CreateAssetMenu(fileName = "ArcherWaveMovesData", menuName = "Bosses/Archer Wave Moves Data")]
public class ArcherWaveMovesData : ScriptableObject
{
    public int HP;
   
    [Header("Move 1: hit player x damage every y turns")]
    public int Move1Damage;
    public int Move1NumOfTurns;

}