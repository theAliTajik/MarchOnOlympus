
using System;

public interface IDamageable
{
    public event Action<int> OnDamage;
    
    public Fighter.DamageContext TakeDamage(int damage, Fighter sender, bool doesReturnToSender = true,
        bool isArmorPiercing = false,
        Fighter.DamageContext damageContext = null);
}