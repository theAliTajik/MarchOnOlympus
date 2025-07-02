
public interface IDamagable
{
    public int TakeDamage(int damage, Fighter sender, bool doesReturnToSender, bool isArmorPiercing = false);
    
    
}