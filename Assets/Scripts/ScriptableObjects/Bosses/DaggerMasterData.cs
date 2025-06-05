using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DaggerMasterData", menuName = "Bosses/Dagger Master Data")]
public class DaggerMasterData : ScriptableObject
{
   public int HP;

   [Header("Move 1: Hit, send card")] 
   public int Move1Damage;
   public BaseCardData Move1Card;

   [Header("Move 2: hit 2x")] 
   public int Move2BleedPerDagger;

   [Header("Move 3: hit, apply vulnerable")]
   public int Move3DamagePerDagger;
   
   [Header("MISCS:")] 
   public FighterHP.TriggerPercentage Phase1PercentageTrigger;
   public FighterHP.TriggerPercentage Phase2PercentageTrigger;
   
   public int AddedDaggersEachTurn;
   public int AddedDaggersInPhase2;
   public int MaxDaggers;
   public int FirstPhaseStr;

}
