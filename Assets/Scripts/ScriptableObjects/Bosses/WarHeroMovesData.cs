using UnityEngine;

[CreateAssetMenu(fileName = "WarHeroData", menuName = "Bosses/WarHero Move Data")]
public class WarHeroMovesData : ScriptableObject
{
    public int HP = 200;
   
    [Header("Move 1: Hit2x5, Apply Haunt 2 per Attack that hits Player (if no block)")]
    public int Move1Damage;
	public int Move1NumOfAttacks;
	public int Move1Haunt;

	[Header("Move 2: Panic")]
	public int Move2Damage_PanicGreater1;
	public int Move2Damage_PanicGreater2;
	public int Move2Restore;

	[Header("MISCS:")]
	public FighterHP.TriggerPercentage Phase1HPPercentageTrigger;
	public FighterHP.TriggerPercentage Phase2HPPercentageTrigger;
}