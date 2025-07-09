using UnityEngine;

public class BaseNymph : BaseEnemy
{
    [SerializeField] protected LastNymphAliveMoveData m_lastNymphMoveData;
    protected LastNymphAliveAction m_lastNymphAction = new LastNymphAliveAction();

    protected bool m_doesCastTwice = false;
    protected bool m_isLastAlive = false;

    protected override void Awake()
    {
        base.Awake();
        NymphsCoordinator.RegisterNymph(this);
    }

    public void SetDoesCastTwice(bool doesCastTwice)
    {
        m_doesCastTwice = doesCastTwice;
    }

    public void SetLastNymphAlive(bool isLast)
    {
        m_isLastAlive = isLast;
        ShowIntention();
    }
}
