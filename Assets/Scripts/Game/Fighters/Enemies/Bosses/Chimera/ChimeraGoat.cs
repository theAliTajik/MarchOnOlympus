
using System;
using UnityEngine;

public class ChimeraGoat : IChimeraHead, IChimeraHeadStun, IHaveIntention
{
    public event Action<Intention, string> OnIntentionDetermined;
    public event Action<BaseEnemy, int> OnDamaged;

    public void DetermineIntention()
    {
        throw new NotImplementedException();
    }

    public void ExecuteIntention(Action finishCallBack)
    {
        throw new NotImplementedException();
    }

    public void Stun()
    {
        Debug.Log("goat stunned");
    }
}

