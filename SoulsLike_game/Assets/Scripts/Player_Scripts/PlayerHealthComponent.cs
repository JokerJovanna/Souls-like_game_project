using UnityEngine;
using System;

public class PlayerHealthComponent : MonoBehaviour
{
    public AudioClip blockSound;
    private AudioSource audioSource;

    public float maxHealth = 100f;
    private float currentHealth;

    public float invincibilityDuration = 1f;
    private bool isInvincible = false;
    private float invincibilityEndTime = 0f;

    public float blockDamageReduction = 0.7f;

    private DodgeComponent dodgeComponent;
    private BlockComponent blockComponent;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    public event Action<float, float> OnHealthChanged;
    public event Action OnDie;

    void Start()
    {
        currentHealth = maxHealth;
        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        dodgeComponent = GetComponent<DodgeComponent>();
        blockComponent = GetComponent<BlockComponent>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        originalColor = spriteRenderer.color;
    }

    void Update()
    {
        if (isInvincible && Time.time >= invincibilityEndTime)
        {
            isInvincible = false;
            if (spriteRenderer != null) spriteRenderer.color = originalColor;
        }
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
        if (blockComponent.IsBlocking && blockComponent != null)
        {
            finalDamage = attack.Damage * blockDamageReduction;
            Debug.Log($"Урон заблокирован: {attack.Damage} -> {finalDamage}");
            if (audioSource != null && blockSound != null)
                audioSource.PlayOneShot(blockSound);
        }

        currentHealth -= finalDamage;
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
        Debug.Log(currentHealth);

        if (spriteRenderer != null) spriteRenderer.color = Color.red;

        isInvincible = true;
        invincibilityEndTime = Time.time + invincibilityDuration;

        if (currentHealth <= 0f)
        {
            currentHealth = 0f;
            Die();
        }
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