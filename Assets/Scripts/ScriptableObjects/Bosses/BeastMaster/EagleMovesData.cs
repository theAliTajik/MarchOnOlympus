using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;

[CreateAssetMenu(fileName = "EagleMovesData", menuName = "Bosses/Eagle Moves Data")]
public class EagleMovesData : ScriptableObject
{
   public int HP;
   
   [Header("Move 1: Daze Player")]
   public MechanicType Move1MechanicType;

   [Header("Move 2: hit x")] 
   public int Move2Damage;
   
}
