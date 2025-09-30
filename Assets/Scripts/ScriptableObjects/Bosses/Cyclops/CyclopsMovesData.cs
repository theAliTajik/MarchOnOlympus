using UnityEngine;

[CreateAssetMenu(fileName = "CyclopsMoveData", menuName = "Bosses/Cyclops Move Data")]
public class CyclopsMovesData : ScriptableObject
{
    public int HP;
	public int ClubHP;
	public int EyeHP;

	[Header("Always: If no club, Get a new Club")]
	[Header("--------")]

	[Header("Move 1: Hit 5x2, Remove all buffs on player")]
    public int Move1Damage;
	public int Move1NumOfAttacks;

	[Header("Move 2: Hit 10x2")]
	public int Move2Damage;
	public int Move2NumOfAttacks;

	[Header("Move 3: Every 4th turn, removes all debuffs and Fortify 2")]
	public int Move3Fortify;

	[Header("MISC:")] 
	public int StrGainIfClubAlive;
}