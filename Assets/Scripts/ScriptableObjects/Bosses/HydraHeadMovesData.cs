
using UnityEngine;

[CreateAssetMenu(fileName = "HydraHeadData", menuName = "Bosses/Hydra Head Move Data")]
public class HydraHeadMovesData : ScriptableObject
{
    public int HP;

	[Header("Move 1: Hit 10 + (5 x Missing Head Count")]
    public int Move1Damage;
    public int Move1DamageMultiplier;
    
    [Header("Move 2: Hit 5 x Acid Dot Count")]
    public int Move2AcidDotCountDamageMultiplier;

    [Header("Move 3: Acid Dot")] public int Move3AcidDotAmount;
}
