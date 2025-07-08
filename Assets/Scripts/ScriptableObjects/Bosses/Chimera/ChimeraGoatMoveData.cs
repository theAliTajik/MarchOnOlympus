
using UnityEngine;

[CreateAssetMenu(fileName = "ChimeraGoatMovesData", menuName = "Bosses/Chimera Goat Move Data")]
public class ChimeraGoatMoveData : ScriptableObject
{
    [Header("Move 1 - 2: Fortify on serpent")]
    public int Move1Fortify;

    [Header("Move 3: Hit 20x poison cards in deck")]
    public BaseCardData Move2PoisonCard;
    public int Move2PoisonHitMultiplier;

    [Header("MISCS:")] 
    public int DamageThresholdForStun;

}
