using UnityEngine;

[CreateAssetMenu(fileName = "Assassin_A_Data", menuName = "Bosses/Assassin A Move Data")]
public class Assassin_A_MovesData : ScriptableObject
{
    public int HP;
   
    [Header("Move 1: Block 5 , Gain 2 str")]
    public int Move1Block;
    public int Move1Str;

	[Header("Move 2: Hit 3x3")]
    public int Move2Damage;
	public int Move2NumOfAttacks;
}