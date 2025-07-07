using System;
using UnityEngine;

public interface IChimeraHead
{
    public void DetermineIntention();
    public void ExecuteIntention(Action finishCallBack);
    public void TakeDamage(int damage);
}

public interface IChimeraHeadStun
{
    public void Stun();
}
