
using Game;

public class CyclopsClubDamageBehaviour : EnemyDamageBehaviour
{
    public CyclopsClubDamageBehaviour(IHaveMechanics owner)
    {
        m_mechanicsOwner = owner;
    }
    
    public override Fighter.DamageContext TakeDamage(int damage, Fighter sender, bool doesReturnToSender,
        bool isArmorPiercing = false,
        Fighter.DamageContext damageContext = null)
    {
        int playerBLockStack = GameInfoHelper.GetMechanicStack(GameInfoHelper.GetPlayer(), MechanicType.BLOCK);
        if (playerBLockStack > 0)
        {
            damage += playerBLockStack;
        }
        
        return base.TakeDamage(damage, sender, doesReturnToSender, isArmorPiercing);
    }
}
