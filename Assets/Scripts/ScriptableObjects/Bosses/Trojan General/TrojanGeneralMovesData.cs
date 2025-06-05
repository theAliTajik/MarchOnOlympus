using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;

[CreateAssetMenu(fileName = "TrojanGeneralMovesData", menuName = "Bosses/Trojan General Moves Data")]
public class TrojanGeneralMovesData : ScriptableObject
{
   public int HP;
   
   
   [Header("Move 1: Hit x*y Bleed z")]
   public int Move1Damage;
   public int Move1DamageTimes;
   public int Move1Bleed;

   [Header("Move 2: Hit x , fortify")] 
   public int Move2Damage;
   public int Move2Fortify;

}
