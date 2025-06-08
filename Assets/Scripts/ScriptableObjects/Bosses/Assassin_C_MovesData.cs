using UnityEngine;

[CreateAssetMenu(fileName = "Assassin_C_Data", menuName = "Bosses/Assassin C Move Data")]
public class Assassin_C_MovesData : ScriptableObject
{
    public int HP;
   
    [Header("Move 1: Thorns 3 to all allies")]
    public int Move1Thorns;

	[Header("Move 2: Hit 5, Block 15")]
    public int Move2Damage;
	public int Move2Block;
}