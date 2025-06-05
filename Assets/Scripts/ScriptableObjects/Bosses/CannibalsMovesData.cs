using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;

[CreateAssetMenu(fileName = "CannibalsMovesData", menuName = "Bosses/Cannibals Moves Data")]
public class CannibalsMovesData : ScriptableObject
{
   public int HP;
   public int Level2HP;
   public int Level3HP;
   
   [Header("Level 1")]
   [Space(10)]
   
   [Header("Move 1: Hit x")]
   public int Move1Damage;

   [Header("Move 2: Hit x to player and y to himself")] 
   public int Move2Damage;
   public int Move2DamageToSelf;
   
   [Header("Level 2")]
   [Space(10)]
   
   [Header("Move 1: Hit x")]
   public int Move3Damage;

   [Header("Move 2: Hit x to player and y to himself")] 
   public int Move4Damage;
   public int Move4DamageToSelf;

   [Header("Level 3")]
   [Space(10)]
   
   [Header("Move 1: Hit x to player and y himself")]
   public int Move5Damage;
   public int Move5DamageToSelf;
   
}
