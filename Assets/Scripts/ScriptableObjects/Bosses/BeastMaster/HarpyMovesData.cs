using UnityEngine;

[CreateAssetMenu(fileName = "HarpyMovesData", menuName = "Bosses/Harpy Moves Data")]
public class HarpyMovesData : ScriptableObject
{
   public int HP;

	[Header("Move 1: Hit 10, Apply Bleed 5")]
	public int Move1Damage;
	public int Move1Bleed;

	[Header("Move 2: Screech Give Daze 2")]
	public int Move2Daze;

	[Header("MISCS:")]
	public FighterHP.TriggerPercentage Phase1HPPercentageTrigger;
	public FighterHP.TriggerPercentage Phase2HPPercentageTrigger;
}