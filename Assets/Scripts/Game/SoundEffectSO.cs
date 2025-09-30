
using UnityEngine;

[CreateAssetMenu(menuName = "Olympus/Sound Effect")]
public class SoundEffectSO : ScriptableObject
{
    public string effectName;
    public AudioClip[] clips;
    public float volume = 1f;

    public AudioClip GetRandomClip()
    {
        if (clips == null || clips.Length == 0) return null;
        return clips[Random.Range(0, clips.Length)];
    }
}