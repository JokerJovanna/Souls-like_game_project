using UnityEngine;

public interface IAttacker
{
    float Damage { get; }

    void Attack();
}

public interface IDamageable
{
    float CurrentHealth { get; }
    float MaxHealth { get; }

    void TakeDamage(float amount, GameObject source);
    void Die();
}

public interface IPlayer : IAttacker, IDamageable
{
    float CurrentStamina { get; }
    float MaxStamina { get; }
    int HealPotionCount { get; }

    void Heal();
    void Dodge();
    void Block();

    event System.Action<float, float> OnHealthChanged; // currentHealth, maxHealth
    event System.Action<float, float> OnStaminaChanged; // currentStamina, maxStamina
    event System.Action OnHealPotionCountChanged;
    event System.Action OnDie;
}

public interface IEnemy : IAttacker, IDamageable
{

}

public interface IBoss : IEnemy
{
    event System.Action<float, float> OnHealthChanged; // currentHealth, maxHealth
}
