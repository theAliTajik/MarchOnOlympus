using UnityEngine;

[CreateAssetMenu(fileName = "ChaosTentacleMovesData", menuName = "Bosses/ChaosTentacle Move Data")]
public class ChaosTentacleMovesData : ScriptableObject
{
	public int HP;
   
    [Header("Move 1: Gives +2 energy to PLAYER but whenever player uses a card, gives +1 str or dex to the boss")]
    public int Move1Energy;
	public int Move1StrOrDex;
}