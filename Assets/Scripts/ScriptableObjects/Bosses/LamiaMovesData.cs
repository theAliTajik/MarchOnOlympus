using UnityEngine;

[CreateAssetMenu(fileName = "LamiaMovesData", menuName = "Bosses/Lamia Move Data")]
public class LamiaMovesData : ScriptableObject
{
    public int HP;
   
    [Header("Move 1: Hit 3x5, Petrify 1 per hit")]
    public int Move1Damage;
    public int Move1NumOfAttacks;
	public int Move1Petrify;

	[Header("Move 2: Block 20, Thorns 3")]
    public int Move2Block;
	public int Move2Thorns;

	[Header("Move 3: Hit 20, ignores block, Petrifyx4")]
	public int Move3Damage;
	public int Move3PetrifyMultiply;

	[Header("Move 4: Only if petrify > 4, Hit 5xPetrify Stack")]
	public int Move4Damage;

	[Header("MISCS:")]
	public FighterHP.TriggerPercentage Phase1HPPercentageTrigger;
	public FighterHP.TriggerPercentage Phase2HPPercentageTrigger;
}