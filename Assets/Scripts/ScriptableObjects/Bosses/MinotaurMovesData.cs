using UnityEngine;

[CreateAssetMenu(fileName = "MinotaurData", menuName = "Bosses/Minotaur Move Data")]
public class MinotaurMovesData : ScriptableObject
{
    public int HP;

	[Header("Move 1: ")]
    public int Move1Damage;

	[Header("MISCS:")]
	public FighterHP.TriggerPercentage Phase1HPPercentageTrigger;
	public FighterHP.TriggerPercentage Phase2HPPercentageTrigger;
}