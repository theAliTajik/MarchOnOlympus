using System;

public interface IChimeraHead
{
    public event Action<BaseEnemy, int> OnDamaged;

    public void DetermineIntention();
    public void ExecuteIntention(Action finishCallBack);
}

public interface IChimeraHeadStun
{
    public void Stun();
}