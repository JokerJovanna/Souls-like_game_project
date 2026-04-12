using System;
using UnityEngine;

public class Player : MonoBehaviour, IPlayer
{
    private readonly float speed = 5.0f;
    private readonly float jumpForce = 10.0f;
    private int remainingJumps;

    public Transform groundCheck;
    public LayerMask groundLayer;
    public float groundCheckRadius = 0.1f;

    private Rigidbody2D rb;
    private bool isGrounded;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (isGrounded)
        {
            remainingJumps = 2;
        }

        if (Input.GetButtonDown("Jump") && (isGrounded || remainingJumps < 2) && remainingJumps > 0)
        {
            Debug.Log(remainingJumps);
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            remainingJumps -= 1;
        }

    }

    void FixedUpdate()
    {
        float moveX = Input.GetAxis("Horizontal");

        Vector2 movement = new Vector2(moveX * speed * Time.fixedDeltaTime, rb.linearVelocity.y);
        rb.linearVelocity = new Vector2(moveX * speed, rb.linearVelocity.y);
    }


    public float CurrentStamina => throw new System.NotImplementedException();
    public float MaxStamina => throw new System.NotImplementedException();
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

    public void Dodge()
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
