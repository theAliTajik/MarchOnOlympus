using Game;
using UnityEngine;

[CreateAssetMenu(fileName = "HydraData", menuName = "Bosses/Hydra Move Data")]
public class HydraMovesData : ScriptableObject
{
	[Header("MiSC")]
    public int OnHeadDestroyedDamage;

    public int NumOfMechanic;
    public MechanicType MechanicToAdd;

    public int NumOfRevives;
    public int BurnAmount;
}