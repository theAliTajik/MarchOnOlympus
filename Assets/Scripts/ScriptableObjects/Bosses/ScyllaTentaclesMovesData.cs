
using UnityEngine;

[CreateAssetMenu(fileName = "ScyllaTentaclesMovesData", menuName = "Bosses/Scylla Tentacles Move Data")]
public class ScyllaTentaclesMovesData : ScriptableObject
{
    public int HP;
   
    [Header("Attack tentacle")]
    
    [Header("Move 1: hit 10 + (10.boss phase)")]
    public int Move1Damage;
    
    [Header("If dead: boss gains 5 str")] 
    public int Move2DeadStrGain;

    [Header("Defence tentacle")]
    
	[Header("Move 3: block 10 + (10.boss phase)")]
    public int Move3Block;
    
    [Header("If dead: boss gains 5 dex")]
    public int Move4DeadDexGain;

    [Header("Chaos Tentacel")]
    [Header("If Alive: give + 2 energy to player, On card played give +1 str or dex to boss")]
    public int Move5AliveEnergyGain;
    public int Move5StrOrDexGain;
}
