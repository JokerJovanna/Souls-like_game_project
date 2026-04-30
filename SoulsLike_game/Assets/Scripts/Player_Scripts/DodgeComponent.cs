using UnityEngine;

public class DodgeComponent : MonoBehaviour
{
    public float dodgeDistance = 3f;
    public float dodgeDuration = 0.2f;
    public float dodgeCooldown = 1f;
    public float staminaCostDodge = 30f;

    public AudioClip dodgeSound;

    private Rigidbody2D rb;
    private StaminaComponent stamina;
    private MovementComponent movement;
    private AudioSource audioSource;
    private bool isDodging = false;
    private float dodgeEndTime = 0f;
    private float nextDodgeTime = 0f;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        stamina = GetComponent<StaminaComponent>();
        movement = GetComponent<MovementComponent>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isDodging && 
            Time.time >= nextDodgeTime)
        {
            if (stamina.TrySpendStamina(staminaCostDodge))
            {
                Dodge();
            }
        }

        if (isDodging && Time.time >= dodgeEndTime)
        {
            isDodging = false;
            rb.gravityScale = 1f;
        }
    }

    void Dodge()
    {
        float direction = 0f;
        if (Input.GetKey(KeyCode.A)) direction = -1f;
        if (Input.GetKey(KeyCode.D)) direction = 1f;

        if (direction == 0f && movement != null)
            direction = movement.LastDirection;

        if (direction == 0f) direction = 1f;

        Vector2 dodgeDirection = new Vector2(direction, 0).normalized;

        isDodging = true;
        dodgeEndTime = Time.time + dodgeDuration;
        nextDodgeTime = Time.time + dodgeCooldown;

        rb.gravityScale = 0f;
        float dodgeSpeed = dodgeDistance / dodgeDuration;
        rb.linearVelocity = dodgeDirection * dodgeSpeed;

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null) sr.flipX = direction < 0;

        if (audioSource != null && dodgeSound != null)
            audioSource.PlayOneShot(dodgeSound);
    }

    public bool IsDodging => isDodging;
}