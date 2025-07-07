using UnityEngine;

[CreateAssetMenu(fileName = "PhoenixMovesData", menuName = "Bosses/Phoenix Moves Data")]
public class PhoenixMovesData : ScriptableObject
{
   public int HP;

	[Header("Move 1: Burn 8, Daze 1")]
	public int Move1Burn;
	public int Move1Daze;

	[Header("Move 2: Burn 4, Restore 25")]
	public int Move2Burn;
	public int Move2Restore;

	[Header("Move 3: Block 50, Restore 50")]
	public int Move3Block;
	public int Move3Restore;

	[Header("At 66")]
	public int At66Damage;
	public int At66Restore;

	[Header("At 33")]
	public int At33Damage;
	public int At33Restore;

	[Header("MISCS:")]
	public FighterHP.TriggerPercentage Phase1HPPercentageTrigger;
	public FighterHP.TriggerPercentage Phase2HPPercentageTrigger;
}