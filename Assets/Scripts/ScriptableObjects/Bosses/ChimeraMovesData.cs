using UnityEngine;

[CreateAssetMenu(fileName = "ChimeraMovesData", menuName = "Bosses/Chimera Move Data")]
public class ChimeraMovesData : ScriptableObject
{
    public int HP = 300;
   
    [Header("Move 1: Hit 10")]
    public int Move1Damage;

    [Header("MISCS:")] 
    public FighterHP.TriggerPercentage Phase1PercentageTrigger;
    public FighterHP.TriggerPercentage Phase2PercentageTrigger;
    public int PhaseOneUntilThisPrecentOfHP;
    public int PhaseTwoUntilThisPrecentOfHP;
    public int EachCardUsedByPlayerDamageToPlayer;
}