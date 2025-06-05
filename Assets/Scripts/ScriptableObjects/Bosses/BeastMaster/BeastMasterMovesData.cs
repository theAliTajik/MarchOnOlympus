using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BeastMasterMovesData", menuName = "Bosses/BeastMaster Moves Data")]
public class BeastMasterMovesData : ScriptableObject
{
   public int HP;
   
   [Header("Move 1: Hit x")]
   public int Move1Damage;

   [Header("Move 2: hit x times animal count")] 
   public int Move2Damage;
   
   [Header("MISCS:")] 
   public int Phase1HPPercentageTrigger;
   public BaseAnimal Phase1Animal;
   public int Phase2HPPercentageTrigger;
   public BaseAnimal Phase2Animal;
   public int Phase3HPPercentageTrigger;
   public BaseAnimal Phase3Animal;
   
}
