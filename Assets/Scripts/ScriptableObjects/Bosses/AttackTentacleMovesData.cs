using UnityEngine;

[CreateAssetMenu(fileName = "AttackTentacleMovesData", menuName = "Bosses/AttackTentacle Move Data")]
public class AttackTentacleMovesData : ScriptableObject
{
    public int HP;
   
    [Header("Move 1: Hit 10 + (10. boss phase)")]
    public int Move1Damage;

	[Header("If dead, boss gains 5 str")]
	public int DeadStr;
}