
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SoundsDb", menuName = "Olympus/SoundsDb")]
public class SoundsDb : GenericData<SoundsDb>
{
    [System.Serializable]
    private struct Entry {
        public string key;
        public SoundEffectSO sound;
    }

    [SerializeField] private Entry[] sounds;
    private Dictionary<string, SoundEffectSO> soundMap;

    private void Awake() {
        soundMap = new Dictionary<string, SoundEffectSO>();
        foreach (var entry in sounds) {
            if (!soundMap.ContainsKey(entry.key) && entry.sound != null) {
                soundMap.Add(entry.key, entry.sound);
            }
        }
    }

    public SoundEffectSO Get(string key) {
        if (soundMap.TryGetValue(key, out var sound)) {
            return sound;
        }
        CustomDebug.LogWarning($"SoundDb: Key '{key}' not found!", Categories.Sound.SFX);
        return null;
    }}
