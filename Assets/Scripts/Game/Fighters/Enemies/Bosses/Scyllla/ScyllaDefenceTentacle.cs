
using Game;

public class ScyllaDefenceTentacle : ScyllaTentacle
{
    public override void OnHPDeath()
    {
        base.OnHPDeath();
        GameActionHelper.AddMechanicToOwner(m_mind, m_data.Move4DeadDexGain, MechanicType.DEXTERITY);
    }
}
