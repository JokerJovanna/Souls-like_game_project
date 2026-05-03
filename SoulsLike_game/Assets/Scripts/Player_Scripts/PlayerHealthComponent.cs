using UnityEngine;
using System;
using UnityEngine.UI;

public class PlayerHealthComponent : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;

    public float invincibilityDuration = 1f;
    private bool isInvincible = false;
    private float invincibilityEndTime = 0f;

    public float blockDamageReduction = 0.7f;
    public float blockAngle = 120f;
    public float blockStaminaCost = 15f;

    public float perfectBlockDamageMultiplier = 0f;

    public AudioClip hurtSound;   
    public AudioClip blockSound;  
    public AudioClip dieSound;

    public float greenEffectDuration = 0.2f;

    private DodgeComponent dodgeComponent;
    private BlockComponent blockComponent;
    private StaminaComponent stamina;
    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource;
    private Color originalColor;
    private Slider healthBar; 

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
        stamina = GetComponent<StaminaComponent>();
        audioSource = GetComponent<AudioSource>();
        originalColor = spriteRenderer.color;
        var healthBarObj = GameObject.FindGameObjectWithTag("PlayerHealthBar");
        healthBar = healthBarObj.GetComponentInChildren<Slider>();
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
        if (dodgeComponent != null && dodgeComponent.IsDodging) return;

        if (isGreen)
        {
            isGreen = false;
            if (spriteRenderer != null && !isInvincible)
                spriteRenderer.color = originalColor;
        }

        float finalDamage = attack.Damage;
        bool blocked = false;
        bool perfect = false;

        if (blockComponent != null && blockComponent.IsBlocking && attack.Attacker != null && IsTargetInFront(attack.Attacker.transform))
        {
            if (attack.CanBeBlocked && blockComponent.IsPerfectBlock())
            {
                perfect = true;
                finalDamage = 0f;
                Debug.Log("��������� ����! ����� ���.");
            }
            else
            {
                if (attack.CanBeBlocked && stamina != null && stamina.CurrentStamina >= blockStaminaCost)
                {
                    stamina.TrySpendStamina(blockStaminaCost);
                    finalDamage = attack.Damage * blockDamageReduction;
                    blocked = true;
                    Debug.Log($"���� ������������: {attack.Damage} -> {finalDamage}");
                    if (audioSource != null && blockSound != null)
                        audioSource.PlayOneShot(blockSound);
                }
                else
                {
                    finalDamage = attack.Damage;
                    Debug.Log("������������ ������������ ��� �����");
                    if (audioSource != null && hurtSound != null)
                        audioSource.PlayOneShot(hurtSound);
                }
            }
        }
        else
        {
            if (audioSource != null && hurtSound != null)
                audioSource.PlayOneShot(hurtSound);
        }

        if (!perfect && finalDamage > 0)
            currentHealth -= finalDamage;
        healthBar.value = currentHealth / maxHealth;

        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        if (spriteRenderer != null && finalDamage > 0 && !blocked)
            spriteRenderer.color = Color.red;

        isInvincible = true;
        invincibilityEndTime = Time.time + invincibilityDuration;

        if (currentHealth <= 0f)
            Die();
    }

    private Vector2 GetForwardDirection()
    {
        var sr = GetComponent<SpriteRenderer>();
        return (sr != null && sr.flipX) ? Vector2.left : Vector2.right;
    }

    private bool IsTargetInFront(Transform target)
    {
        if (target == null) return false;
        Vector2 forward = GetForwardDirection();
        Vector2 directionToTarget = (target.position - transform.position).normalized;
        float angle = Vector2.Angle(forward, directionToTarget);
        return angle <= blockAngle / 2f;
    }

    public void Heal(float amount)
    {
        if (currentHealth <= 0f) return;

        float newHealth = currentHealth + amount;
        if (newHealth > maxHealth) newHealth = maxHealth;
        currentHealth = newHealth;
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
        Debug.Log("����� �����");
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