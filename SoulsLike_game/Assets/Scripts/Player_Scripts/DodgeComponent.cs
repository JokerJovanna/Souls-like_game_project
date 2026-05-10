using UnityEngine;

public class DodgeComponent : MonoBehaviour
{
    public float dodgeDistance = 3f;
    public float dodgeDuration = 0.2f;
    public float dodgeCooldown = 1f;
    public float staminaCostDodge = 30f;

    private bool isDodging = false;
    private float dodgeEndTime = 0f;
    private float nextDodgeTime = 0f;
    private float currentGravityScale;

    private StaminaComponent stamina;
    private MovementComponent movement;
    private BlockComponent block;

    private AudioSource audioSource;
    public AudioClip dodgeSound;

    private Rigidbody2D rb;

    public bool IsDodging => isDodging;

    void Start()
    {
        stamina = GetComponent<StaminaComponent>();
        movement = GetComponent<MovementComponent>();
        block = GetComponent<BlockComponent>();

        audioSource = GetComponent<AudioSource>();

        rb = GetComponent<Rigidbody2D>();

        currentGravityScale = rb.gravityScale;
    }

    void Update()
    {
        if (block != null && block.IsBlocking) return;

        if (Input.GetKeyDown(KeyCode.LeftShift) && !isDodging && Time.time >= nextDodgeTime &&
            stamina.TrySpendStamina(staminaCostDodge))
        {
            Dodge();
        }

        if (isDodging && Time.time >= dodgeEndTime)
        {
            isDodging = false;
            rb.gravityScale = currentGravityScale;
        }
    }

    void Dodge()
    {
        var direction = GetDirection();
        var dodgeDirection = new Vector2(direction, 0).normalized;

        isDodging = true;
        dodgeEndTime = Time.time + dodgeDuration;
        nextDodgeTime = Time.time + dodgeCooldown;

        rb.gravityScale = 0f;
        var dodgeSpeed = dodgeDistance / dodgeDuration;
        rb.linearVelocity = dodgeDirection * dodgeSpeed;

        var sr = GetComponent<SpriteRenderer>();
        if (sr != null) sr.flipX = direction < 0;

        if (audioSource != null && dodgeSound != null) audioSource.PlayOneShot(dodgeSound);
    }

    private float GetDirection()
    {
        var direction = 0f;
        if (Input.GetKey(KeyCode.A)) direction = -1f;
        if (Input.GetKey(KeyCode.D)) direction = 1f;
        if (direction == 0f && movement != null) direction = movement.LastDirection;
        if (direction == 0f) direction = 1f;

        return direction;
    }
}