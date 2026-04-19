using UnityEngine;
using System;

public class PlayerHealthComponent : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;

    public float invincibilityDuration = 1f;
    private bool isInvincible = false;
    private float invincibilityEndTime = 0f;

    public float blockDamageReduction = 0.7f;

    private DodgeComponent dodgeComponent;
    private BlockComponent blockComponent;

    public event Action<float, float> OnHealthChanged;
    public event Action OnDie;

    void Start()
    {
        currentHealth = maxHealth;
        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        dodgeComponent = GetComponent<DodgeComponent>();
        blockComponent = GetComponent<BlockComponent>();
    }

    void Update()
    {
        if (isInvincible && Time.time >= invincibilityEndTime) isInvincible = false;
    }

    public void TakeDamage(AttackData attack)
    {
        if (isInvincible) return;

        if (dodgeComponent != null && dodgeComponent.IsDodging)
        {
            Debug.Log("уклонение");
            return;
        }

        float finalDamage = attack.Damage;
        if (blockComponent.IsBlocking)
        {
            finalDamage = attack.Damage * blockDamageReduction;
            Debug.Log($"Урон заблокирован: {attack.Damage} -> {finalDamage}");
        }

        currentHealth -= finalDamage;
        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        if (currentHealth <= 0f)
        {
            currentHealth = 0f;
            Die();
            return;
        }

        isInvincible = true;
        invincibilityEndTime = Time.time + invincibilityDuration;
    }

    
    private void Die()
    {
        Debug.Log("Игрок погиб");
        OnDie?.Invoke();
    }

    public float CurrentHealth => currentHealth;
    public float MaxHealth => maxHealth;
    public bool IsInvincible => isInvincible;
}