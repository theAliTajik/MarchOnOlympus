using UnityEngine;

[CreateAssetMenu(fileName = "Assassin_B_Data", menuName = "Bosses/Assassin B Move Data")]
public class Assassin_B_MovesData : ScriptableObject
{
    public int HP;
   
    [Header("Move 1: 2 Vulnerable")]
    public int Move1Vulnerable;

	[Header("Move 2: Hit 5, Restore 10")]
    public int Move2Damage;
	public int Move2Restore;
}