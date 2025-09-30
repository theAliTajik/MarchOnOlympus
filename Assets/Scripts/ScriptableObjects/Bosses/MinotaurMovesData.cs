using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "MinotaurData", menuName = "Bosses/Minotaur Move Data")]
public class MinotaurMovesData : ScriptableObject
{
    public int HP;

	[Header("Move 1: hit 5, gain 2 str")]
    public int Move1Damage;
	public int Move1Str;

	[Header("Move 2: if str > 1, hit 20 + str count * 4")]
	public int Move2StrThreshold;
	public int Move2Damage;
	public int Move2StrMultiplier;

	[Header("Move 3: ShoutOne if player hp > 51 gain 5 str remove 5 str from player")]
	public int Move3PlayerHPPercentage;
	public int Move3Str;
	[FormerlySerializedAs("Move3StrRemove")] public int Move3StrReduce;
	
	[Header("Move 4: ShoutTwo if player hp < 50, restore 50")]
	public int Move4PlayerHPPercentage;
	public int Move4Restore;
	
	[Header("Always: if str > 20, hit 100")]
	public int Move5StrThreshold;
	public int Move5Damage;
	
	[Header("MISCS:")]
	public FighterHP.TriggerPercentage GainStrHPPercentageTrigger;
	public int PercentageTriggerStrGain;
	
	public FighterHP.TriggerPercentage UseShoutsHPPercentageTrigger;

	public int OnDeathStrThreshold;
	public string OnDeathPerkName;
}