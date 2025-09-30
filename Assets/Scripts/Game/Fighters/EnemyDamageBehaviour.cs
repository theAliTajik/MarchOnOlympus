
public class EnemyDamageBehaviour : StandardDamageBehaviour
{
    public EnemyDamageBehaviour()
    {
        
    }
    
    public EnemyDamageBehaviour(IHaveMechanics owner)
    {
        m_mechanicsOwner = owner;
    }
    
    public override Fighter.DamageContext TakeDamage(int damage, Fighter sender, bool doesReturnToSender,
        bool isArmorPiercing = false,
        Fighter.DamageContext damageContext = null)
    {
        Fighter.DamageContext context = base.TakeDamage(damage, sender, doesReturnToSender, isArmorPiercing, damageContext);
        GameplayEvents.SendGamePhaseChanged(EGamePhase.ENEMY_DAMAGED);

        return context;
    }
}
