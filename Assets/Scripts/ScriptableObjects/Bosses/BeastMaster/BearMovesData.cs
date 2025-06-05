using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BearMovesData", menuName = "Bosses/Bear Moves Data")]
public class BearMovesData : ScriptableObject
{
   public int HP;
   
   [Header("Move 1: give x armor to boss and animals")]
   public int Move1Armor;

   [Header("Move 2: hit x")] 
   public int Move2Damage;
   
}
