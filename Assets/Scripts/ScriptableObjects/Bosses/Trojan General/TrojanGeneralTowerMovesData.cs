using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;

[CreateAssetMenu(fileName = "TrojanGeneralTowerMovesData", menuName = "Bosses/Trojan General Tower Moves Data")]
public class TrojanGeneralTowerMovesData : ScriptableObject
{
   public int HP;
   
   
   [Header("Move 1: Pump")]

   [Header("Move 2: Hit x , fortify")] 
   public int Move2Damage;
   
}
