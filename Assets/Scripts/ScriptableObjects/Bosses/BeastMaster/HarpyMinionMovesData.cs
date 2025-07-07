using Game;
using UnityEngine;

[CreateAssetMenu(fileName = "HarpyMinionMovesData", menuName = "Bosses/HarpyMinion Moves Data")]
public class HarpyMinionMovesData : ScriptableObject
{
	public int HP;

	[Header("Move 1: Hit 15")]
	public int Move1Damage;
}