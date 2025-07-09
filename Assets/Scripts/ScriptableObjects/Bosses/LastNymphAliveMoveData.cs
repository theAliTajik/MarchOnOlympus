
using UnityEngine;

[CreateAssetMenu(fileName = "LastNymphAliveMoveData", menuName = "Bosses/Last Nymph Alive Move Data")]
public class LastNymphAliveMoveData : ScriptableObject
{
    [Header("Move")] 
    public string Description;
    
    public int Vulnerable;
    public int Bleed;
    public int Damage;
    public int Restore;
}
