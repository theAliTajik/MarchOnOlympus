using UnityEngine;

[CreateAssetMenu(fileName = "ChimeraMovesData", menuName = "Bosses/Chimera Move Data")]
public class ChimeraMovesData : ScriptableObject
{
    public int HP;
   
    [Header("Move 1: Hit 10")]
    public int Move1Damage;

    [Header("MISCS:")] 
    public FighterHP.TriggerPercentage PoisonPercentageTrigger;
    public FighterHP.TriggerPercentage TauntPercentageTrigger;
}