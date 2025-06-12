
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "CombatWavesDb", menuName = "Olympus/Combat Waves Database")]
public class CombatWavesDb : GenericData<CombatWavesDb>
{
    [System.Serializable]
    public class CombatWaveSet
    {
        public string ClientID;
        public List<CombatWave> Waves = new List<CombatWave>();
    }

    [System.Serializable]
    public class CombatWave
    {
        public string Name;
        public List<EnemiesDb.BossInfo> Enemies = new List<EnemiesDb.BossInfo>();
    }
    
    public List<CombatWaveSet> AllCombatWaveSets = new List<CombatWaveSet>();


    public CombatWaveSet FindById(string clientId)
    {
        return AllCombatWaveSets.Find(waveSet => waveSet.ClientID == clientId);
    }   
    
}

