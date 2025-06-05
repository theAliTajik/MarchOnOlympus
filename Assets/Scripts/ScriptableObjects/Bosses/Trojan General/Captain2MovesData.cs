using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;

[CreateAssetMenu(fileName = "Captain2MovesData", menuName = "Bosses/Captain2 Moves Data")]
public class Captain2MovesData : ScriptableObject
{
   public int HP;
   
   
   [Header("Move 1: Give x Thorns")]
   public int Move1Thorns;

   [Header("MISC")]
   public int NumOfTurnsToChannel;
}
