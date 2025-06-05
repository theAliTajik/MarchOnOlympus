using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;

[CreateAssetMenu(fileName = "Captain3MovesData", menuName = "Bosses/Captain3 Moves Data")]
public class Captain3MovesData : ScriptableObject
{
   public int HP;
   
   
   [Header("Move 1: Give x Block")]
   public int Move1Block;

   [Header("MISC")]
   public int NumOfTurnsToChannel;
}
