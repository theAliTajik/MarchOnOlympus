using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;

[CreateAssetMenu(fileName = "ParisMovesData", menuName = "Bosses/Paris Moves Data")]
public class ParisMovesData : ScriptableObject
{
   public int HP;
   
   [Header("Move 1: hit 100")]
   public int Move1Damage;

   [Header("Move 2: Restore x to Hector")] 
   public int Move2RestoreHector;
   
   [Header("Move 3: Hit x to player and y to himself")]
   public int Move3Damage;
   public int Move3DamageToSelf;
   
   [Header("MISCS:")] 
   public FighterHP.TriggerPercentage Phase1PercentageTrigger;
   public int Phase1StrGainToHector;

}
