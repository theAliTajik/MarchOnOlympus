using UnityEngine;

[CreateAssetMenu(fileName = "HydraData", menuName = "Bosses/Hydra Move Data")]
public class HydraMovesData : ScriptableObject
{
    public int HP;
    public int HPCount;

	[Header("Move 1: ")]
    public int Move1Damage;
}