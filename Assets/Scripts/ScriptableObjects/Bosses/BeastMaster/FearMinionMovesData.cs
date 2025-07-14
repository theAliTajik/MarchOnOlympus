using Game;
using UnityEngine;

[CreateAssetMenu(fileName = "FearMinionMovesData", menuName = "Bosses/FearMinion Moves Data")]
public class FearMinionMovesData : ScriptableObject
{
	public int HP;

	[Header("Move 1: Single Ability: Apply Haunt 3")]
	public int Move1Haunt;
}