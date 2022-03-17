using UnityEngine;

namespace Item
{
    public class WeaponStatsController : ItemController
    {
        [Header("Weapon Stats")]
        [SerializeField] private float baseDamage;
        [SerializeField] private float criticalHitChance;
        [SerializeField] private float criticalHitMultiplier;
        [SerializeField] private float knockbackAmount;
        
        public virtual float BaseDamage => baseDamage;
        public virtual float CriticalHitChance => criticalHitChance;
        public virtual float CriticalHitMultiplier => criticalHitMultiplier;
        public virtual float KnockbackAmount => knockbackAmount;
    }
}