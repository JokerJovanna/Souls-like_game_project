using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    private readonly float speed = 5.0f;
    private Rigidbody2D rb;


    private readonly float jumpForce = 10.0f;
    private readonly int maxJumps = 2;
    public float jumpCooldown = 0.2f;
    private int remainingJumps;
    private float nextJumpTime;

    public Transform groundCheck;
    public LayerMask groundLayer;
    public float groundCheckRadius = 0.1f;
    private bool isGrounded;
    private bool wasGrounded;

    public float dodgeDistance = 3f;
    public float dodgeDuration = 0.2f;
    public float dodgeCooldown = 1f;
    private bool isDodging = false;
    private float dodgeEndTime = 0f;
    private float nextDodgeTime = 0f;
    private Vector2 dodgeDirection;

    public float maxStamina = 100f;
    private float currentStamina;
    public float staminaRegenRate = 20f;
    public float staminaCostDodge = 30f;
    public float staminaCostJump = 10f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        remainingJumps = maxJumps;
        currentStamina = maxStamina;
    }

    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 
            groundCheckRadius, groundLayer);
        
        if (isGrounded && !wasGrounded)
        {
            remainingJumps = maxJumps;
        }
        wasGrounded = isGrounded;

        if (Input.GetKeyDown(KeyCode.LeftShift) && !isDodging &&
            Time.time >= nextDodgeTime)
        {
            Dodge();
        }

        if (isDodging && Time.time >= dodgeEndTime)
        {
            isDodging = false;
            rb.gravityScale = 1f;
        }


        if (Input.GetKeyDown(KeyCode.Space) && 
            remainingJumps > 0 && Time.time >= nextJumpTime)
        {
            if (currentStamina >= staminaCostJump)
            {
                currentStamina -= staminaCostJump;
                OnStaminaChanged?.Invoke(currentStamina, maxStamina);
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                remainingJumps--;
                nextJumpTime = Time.time + jumpCooldown;
            }
        }

        if (currentStamina < maxStamina)
        {
            currentStamina += staminaRegenRate * Time.deltaTime;
            if (currentStamina > maxStamina) currentStamina = maxStamina;
            OnStaminaChanged?.Invoke(currentStamina, maxStamina);
        }
        Debug.Log(currentStamina);
    }

    void FixedUpdate()
    {
        if (isDodging) return;

        float moveX = 0f;
        if (Input.GetKey(KeyCode.A)) moveX = -1f;
        if (Input.GetKey(KeyCode.D)) moveX = 1f;
        rb.linearVelocity = new Vector2(moveX * speed, rb.linearVelocity.y);
    }

    public void Dodge()
    {
        if (currentStamina < staminaCostDodge) return;

        currentStamina -= staminaCostDodge;
        OnStaminaChanged?.Invoke(currentStamina, maxStamina);

        float moveX = 0f;
        if (Input.GetKey(KeyCode.A)) moveX = -1f;
        if (Input.GetKey(KeyCode.D)) moveX = 1f;
        if (moveX == 0) moveX = 1f;

        dodgeDirection = new Vector2(moveX, 0).normalized;

        isDodging = true;
        dodgeEndTime = Time.time + dodgeDuration;
        nextDodgeTime = Time.time + dodgeCooldown;

        rb.gravityScale = 0f;
        float dodgeSpeed = dodgeDistance / dodgeDuration;
        rb.linearVelocity = dodgeDirection * dodgeSpeed;
    }

    public float Damage => throw new System.NotImplementedException();
    public float CurrentHealth => throw new System.NotImplementedException();
    public float MaxHealth => throw new System.NotImplementedException();
    public float LastCheckpointCoordinates => throw new System.NotImplementedException();
    public int HealPotionCount => throw new System.NotImplementedException();

    public event System.Action<float, float> OnHealthChanged; // currentHealth, maxHealth
    public event System.Action<float, float> OnStaminaChanged; // currentStamina, maxStamina
    public event System.Action OnHealPotionCountChanged;
    public event System.Action OnDie;
    
    public void Attack()
    {
        throw new System.NotImplementedException();
    }

    public void Block()
    {
        throw new System.NotImplementedException();
    }

    public void Heal()
    {
        throw new System.NotImplementedException();
    }



    public void TakeDamage(float amount, GameObject source)
    {
        throw new System.NotImplementedException();
    }

    public void Die()
    {
        throw new System.NotImplementedException();
    }



    public void InteractionWithCampfire()
    {
        throw new System.NotImplementedException();
    }

}
