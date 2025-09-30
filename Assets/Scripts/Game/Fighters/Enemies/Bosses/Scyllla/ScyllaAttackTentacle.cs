
using Game;

public class ScyllaAttackTentacle : ScyllaTentacle
{
    public override void OnHPDeath()
    {
        base.OnHPDeath();
        GameActionHelper.AddMechanicToOwner(m_mind, m_data.Move2DeadStrGain, MechanicType.STRENGTH);
    }
}
