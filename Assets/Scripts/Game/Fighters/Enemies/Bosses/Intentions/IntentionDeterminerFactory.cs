
using UnityEngine;

public enum IntentionDeterminerType
{
    RANDOM,
    CYCLIC,
}

public static class IntentionDeterminerFactory 
{
    public static IDetermineIntention CreateDeterminer(IntentionDeterminerType type, BaseEnemy.MoveData[] moves)
    {
        switch (type)
        {
            case IntentionDeterminerType.RANDOM:
                return new RandomIntentionDeterminer(moves);
            
            case IntentionDeterminerType.CYCLIC:
                return new CyclicIntentionDeterminer(moves);

            
            default:
                Debug.Log("ERROR: Invalid Intention Determiner type");
                return null;
        }
    }    
}
