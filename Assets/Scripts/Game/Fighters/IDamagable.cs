
using System;

public interface IDamageable
{
    public event Action<int> OnDamage;
    
    public int TakeDamage(int damage, Fighter sender, bool doesReturnToSender, bool isArmorPiercing = false);
}