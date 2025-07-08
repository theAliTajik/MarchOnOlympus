
public class EnemyDamageBehaviour : StandardDamageBehaviour
{
    public override int TakeDamage(int damage, Fighter sender, bool doesReturnToSender, bool isArmorPiercing = false)
    {
        int finalDamage = base.TakeDamage(damage, sender, doesReturnToSender, isArmorPiercing);
        GameplayEvents.SendGamePhaseChanged(EGamePhase.ENEMY_DAMAGED);

        return finalDamage;
    }
}
