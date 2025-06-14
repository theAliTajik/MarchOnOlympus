using UnityEngine;

[CreateAssetMenu(fileName = "WarHeroData", menuName = "Bosses/WarHero Move Data")]
public class WarHeroMovesData : ScriptableObject
{
    public int HP = 200;
   
    [Header("Move 1: Hit2x5, Apply Haunt 2 per Attack that hits Player (if no block)")]
    public int Move1Damage;
	public int Move1NumOfAttacks;

	[Header("MISCS:")]
	public FighterHP.TriggerPercentage Phase1PercentageTrigger;
	public FighterHP.TriggerPercentage Phase2PercentageTrigger;
	public int PhaseOneUntilThisPrecentOfHP;
	public int PhaseTwoUntilThisPrecentOfHP;
	public int EachCardUsedByPlayerDamageToPlayer;
}