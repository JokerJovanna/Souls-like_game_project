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

    public AudioClip hurtSound;   
    public AudioClip blockSound;  
    public AudioClip dieSound;

    public float greenEffectDuration = 0.2f;

    private DodgeComponent dodgeComponent;
    private BlockComponent blockComponent;
    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource;
    private Color originalColor;

    private bool isGreen = false;
    private float greenEffectEndTime = 0f;

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
            if (spriteRenderer != null && !isGreen)
                spriteRenderer.color = originalColor;
        }

        if (isGreen && Time.time >= greenEffectEndTime)
        {
            isGreen = false;
            if (spriteRenderer != null)
            {
                if (isInvincible)
                    spriteRenderer.color = Color.red;
                else
                    spriteRenderer.color = originalColor;
            }
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

        if (isGreen)
        {
            isGreen = false;
            if (spriteRenderer != null && !isInvincible)
                spriteRenderer.color = originalColor;
        }

        float finalDamage = attack.Damage;

        if (blockComponent.IsBlocking && blockComponent != null)
        {
            finalDamage = attack.Damage * blockDamageReduction;
            Debug.Log($"Урон заблокирован: {attack.Damage} -> {finalDamage}");
            if (audioSource != null && blockSound != null)
                audioSource.PlayOneShot(blockSound);
        }
        else
        {
            if (audioSource != null && hurtSound != null)
                audioSource.PlayOneShot(hurtSound);
        }

        currentHealth -= finalDamage;
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
        //Debug.Log(currentHealth);

        if (spriteRenderer != null && finalDamage > 0)
            spriteRenderer.color = Color.red;

        isInvincible = true;
        invincibilityEndTime = Time.time + invincibilityDuration;

        if (currentHealth <= 0f)
        {
            currentHealth = 0f;
            Die();
        }
    }

    public void Heal(float amount)
    {
        if (currentHealth <= 0f) return;

        float newHealth = currentHealth + amount;
        if (newHealth > maxHealth) newHealth = maxHealth;
        currentHealth = newHealth;
        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.green;
            isGreen = true;
            greenEffectEndTime = Time.time + greenEffectDuration;
        }
    }

    private void Die()
    {
        Debug.Log("Игрок погиб");
        if (audioSource != null && dieSound != null)
            audioSource.PlayOneShot(dieSound);
        OnDie?.Invoke();

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null) sr.enabled = false;

        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;

        MovementComponent movement = GetComponent<MovementComponent>();
        if (movement != null) movement.enabled = false;

        JumpComponent jump = GetComponent<JumpComponent>();
        if (jump != null) jump.enabled = false;

        AttackComponent attack = GetComponent<AttackComponent>(); 
        if (attack != null) attack.enabled = false;

        DodgeComponent dodge = GetComponent<DodgeComponent>();
        if (dodge != null) dodge.enabled = false;

        BlockComponent block = GetComponent<BlockComponent>();
        if (block != null) block.enabled = false;

        PotionComponent potion = GetComponent<PotionComponent>();
        if (potion != null) potion.enabled = false;
        gameObject.SetActive(false);
    }

    public float CurrentHealth => currentHealth;
    public float MaxHealth => maxHealth;
    public bool IsInvincible => isInvincible;
}