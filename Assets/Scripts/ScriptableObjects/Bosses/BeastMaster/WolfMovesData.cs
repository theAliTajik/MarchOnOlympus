using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WolfMovesData", menuName = "Bosses/Wolf Moves Data")]
public class WolfMovesData : ScriptableObject
{
   public int HP;
   
   [Header("Move 1: give x strenght to boss and animals")]
   public int Move1Str;

   [Header("Move 2: hit x")] 
   public int Move2Damage;
   
}
