using UnityEngine;

[CreateAssetMenu(fileName = "DefenceTentacleMovesData", menuName = "Bosses/DefenceTentacle Move Data")]
public class DefenceTentacleMovesData : ScriptableObject
{
    public int HP;
   
    [Header("Move 1: Block 10 + (10.boss phase)")]
    public int Move1Block;

	[Header("If dead, boss gains 5 dex")]
	public int DeadDexterity;
}