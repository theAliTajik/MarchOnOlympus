
public class OdysseusController : PlayerController
{
    protected override void OnTookDamage(int damage, bool isCritical)
    {
        base.OnTookDamage(damage, isCritical);
        // m_animator.Play();
    }
}
