using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;

[CreateAssetMenu(fileName = "Captain1MovesData", menuName = "Bosses/Captain1 Moves Data")]
public class Captain1MovesData : ScriptableObject
{
   public int HP;
   
   
   [Header("Move 1: Give 5 Str")]
   public int Move1Str;

   [Header("MISC")]
   public int NumOfTurnsToChannel;
}
