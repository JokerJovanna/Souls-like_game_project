using UnityEngine;
using System;
using UnityEngine.UI;

public class PlayerHealthComponent : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float invincibilityDuration = 1f;
    [SerializeField] private float blockDamageReduction = 0.7f;
    [SerializeField] private float blockAngle = 120f;
    [SerializeField] private float blockStaminaCost = 15f;
    [SerializeField] private float greenEffectDuration = 0.2f;
    [SerializeField] private float perfectBlockStaminaRestore = 20f;

    private float currentHealth;
    private bool isInvincible = false;
    private float invincibilityEndTime = 0f;
    private bool isGreen = false;
    private float greenEffectEndTime = 0f;

    private DodgeComponent dodgeComponent;
    private BlockComponent blockComponent;
    private StaminaComponent stamina;
    private Animator animator;

    private AudioSource audioSource;
    [SerializeField] private AudioClip hurtSound;
    [SerializeField] private AudioClip blockSound;
    [SerializeField] private AudioClip dieSound;

    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    [SerializeField] private Slider healthBar;

    public event Action<float, float> OnHealthChanged;
    public event Action OnDie;

    public float CurrentHealth => currentHealth;
    public float MaxHealth => maxHealth;
    public bool IsInvincible => isInvincible;

    void Start()
    {
        currentHealth = maxHealth;
        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        dodgeComponent = GetComponent<DodgeComponent>();
        blockComponent = GetComponent<BlockComponent>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        stamina = GetComponent<StaminaComponent>();
        animator = GetComponent<Animator>();

        audioSource = GetComponent<AudioSource>();

        originalColor = spriteRenderer.color;
    }

    void Update()
    {
        if (isInvincible && Time.time >= invincibilityEndTime)
        {
            isInvincible = false;
            if (spriteRenderer != null && !isGreen) spriteRenderer.color = originalColor;
        }

        if (isGreen && Time.time >= greenEffectEndTime)
        {
            isGreen = false;
            if (spriteRenderer != null)
            {
                if (isInvincible) spriteRenderer.color = Color.red;
                else spriteRenderer.color = originalColor;
            }
        }
    }

    public void TakeDamage(AttackData attack)
    {
        if (ShouldIgnoreDamage()) return;

        ResetGreenEffect();

        float finalDamage = attack.Damage;
        bool blocked = false;
        bool perfect = false;

        ProcessBlock(attack, ref finalDamage, ref blocked, ref perfect);

        ApplyDamage(finalDamage, perfect);
        ApplyVisualEffects(finalDamage, blocked);
        ActivateInvincibility();

        if (currentHealth <= 0f) Die();
    }

    private bool ShouldIgnoreDamage()
    {
        if (isInvincible) return true;
        if (dodgeComponent != null && dodgeComponent.IsDodging) return true;
        return false;
    }

    private void ResetGreenEffect()
    {
        if (isGreen)
        {
            isGreen = false;
            if (spriteRenderer != null && !isInvincible)
                spriteRenderer.color = originalColor;
        }
    }

    private void ProcessBlock(AttackData attack, ref float finalDamage, ref bool blocked, ref bool perfect)
    {
        var canBlock = blockComponent != null && blockComponent.IsBlocking &&
                        attack.Attacker != null && IsTargetInFront(attack.Attacker.transform);

        if (!canBlock)
        {
            PlayHurtSound();
            return;
        }

        if (attack.CanBeBlocked && blockComponent.IsPerfectBlock())
        {
            perfect = true;
            finalDamage = 0f;
            if (stamina != null)
                stamina.AddStamina(perfectBlockStaminaRestore);
            return;
        }

        if (attack.CanBeBlocked && stamina != null && stamina.CurrentStamina >= blockStaminaCost)
        {
            stamina.TrySpendStamina(blockStaminaCost);
            finalDamage = attack.Damage * blockDamageReduction;
            blocked = true;
            PlayBlockSound();
        }
        else
        {
            finalDamage = attack.Damage;
            PlayHurtSound();
        }
    }

    private void PlayHurtSound()
    {
        if (audioSource != null && hurtSound != null)
            audioSource.PlayOneShot(hurtSound);
    }

    private void PlayBlockSound()
    {
        if (audioSource != null && blockSound != null)
            audioSource.PlayOneShot(blockSound);
    }

    private void ApplyDamage(float finalDamage, bool perfect)
    {
        if (!perfect && finalDamage > 0)
            currentHealth -= finalDamage;
        if (healthBar != null)
            healthBar.value = currentHealth / maxHealth;
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    private void ApplyVisualEffects(float finalDamage, bool blocked)
    {
        if (spriteRenderer != null && finalDamage > 0 && !blocked)
            spriteRenderer.color = Color.red;
    }

    private void ActivateInvincibility()
    {
        isInvincible = true;
        invincibilityEndTime = Time.time + invincibilityDuration;
    }

    private Vector2 GetForwardDirection()
    {
        var sr = GetComponent<SpriteRenderer>();
        return (sr != null && sr.flipX) ? Vector2.left : Vector2.right;
    }

    private bool IsTargetInFront(Transform target)
    {
        if (target == null) return false;
        var forward = GetForwardDirection();
        var directionToTarget = (target.position - transform.position).normalized;
        var angle = Vector2.Angle(forward, directionToTarget);
        return angle <= blockAngle / 2f;
    }

    public void Heal(float amount)
    {
        if (currentHealth <= 0f) return;

        var newHealth = currentHealth + amount;
        if (newHealth > maxHealth) newHealth = maxHealth;
        currentHealth = newHealth;
        if (healthBar != null)
            healthBar.value = newHealth / maxHealth;
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
        if (audioSource != null && dieSound != null) audioSource.PlayOneShot(dieSound);
        OnDie?.Invoke();

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null) sr.enabled = false;

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

        animator.SetTrigger("Die");
    }
}