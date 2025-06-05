using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;

[CreateAssetMenu(fileName = "HectorMovesData", menuName = "Bosses/Hector Moves Data")]
public class HectorMovesData : ScriptableObject
{
   public int HP;
   
   [Header("Move 1: block x")]
   public int Move1Block;

   [Header("Move 2: Gain x charge if charge > y. hit z. each player hit remoes x charge")] 
   public int Move2ChargeGain;
   public int Move2ChargeThreshold;
   public int Move2Damage;
   public int Move2ChargeReduction;
   
   [Header("Move 3: Hit x, fortify")]
   public int Move3Damage;
   public MechanicType Move3MechanicGain;
   
   [Header("Move 4: send x cards into players hand")]
   public int Move4NumOfCards;
   public BaseCardData Move4Card;

   [Header("MISCS:")] 
   public FighterHP.TriggerPercentage Phase1PercentageTrigger;
   public int Phase1StrGain;

}
