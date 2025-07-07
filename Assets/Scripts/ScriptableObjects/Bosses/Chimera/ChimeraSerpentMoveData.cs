
using UnityEngine;

[CreateAssetMenu(fileName = "ChimeraSerpentMoveData", menuName = "Bosses/Chimera Serpent Move Data")]
public class ChimeraSerpentMoveData : ScriptableObject
{
    [Header("Move 1: send poison card")] 
    public BaseCardData Card;

    [Header("MISCS:")] 
    public int DamageThresholdForStun;
}