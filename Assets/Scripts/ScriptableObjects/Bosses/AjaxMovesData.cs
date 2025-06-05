using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "AjaxData", menuName = "Bosses/Ajax Moves Data")]
public class AjaxMovesData : ScriptableObject
{
   public int HP;
   
   [Header("Move 1: pump, next turn fortifie")]
   public int FortifiedStackAmount;
   public int Move1Damage;

   [Header("Move 2: hit 2x")] 
   public int Hit2xDamage;
   
   [Header("Move 3: hit, apply vulnerable")]
   public int vulnerableStackAmount;
   public int Move3Damage;
   
   [Header("Move 4: Block")]
   public int BlockStackAmount;

   [Header("MISCS:")] 
   public FighterHP.TriggerPercentage Phase1PercentageUnTrigger;
   public FighterHP.TriggerPercentage Phase1PercentageTrigger;
   public FighterHP.TriggerPercentage Phase2PercentageTrigger;
   [FormerlySerializedAs("DamageBonusAt66PrecentOfHP")] public int DamageBonusAtPhase1;
   [FormerlySerializedAs("BlockGainedAt33PrecentOfHP")] public int BlockGainedAtPhase2;

}
