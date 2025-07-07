using UnityEngine;

[CreateAssetMenu(fileName = "ScyllaMovesData", menuName = "Bosses/Scylla Move Data")]
public class ScyllaMovesData : ScriptableObject
{
    public int HP;
   
    [Header("Move 1: Hit 3x3")]
    public int Move1Damage;
    public int Move1NumOfAttacks;

	[Header("Move 2: Block 3x3")]
    public int Move2Block;
	public int Move2NumOfBlocks;

	[Header("Move 3:  Restore 25 to ALL tentacles, Hit 15 x Dead Tentacle Count")]
	public int Move3Restore;
	public int Move3Damage;

	[Header("MISCS:")]
	public FighterHP.TriggerPercentage Phase1HPPercentageTrigger;
	public FighterHP.TriggerPercentage Phase2HPPercentageTrigger;
}