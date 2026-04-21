using UnityEngine;

public class DodgeComponent : MonoBehaviour
{
    public float dodgeDistance = 3f;
    public float dodgeDuration = 0.2f;
    public float dodgeCooldown = 1f;
    public float staminaCostDodge = 30f;

    private Rigidbody2D rb;
    private StaminaComponent stamina;
    private bool isDodging = false;
    private float dodgeEndTime = 0f;
    private float nextDodgeTime = 0f;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        stamina = GetComponent<StaminaComponent>();
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
        float moveX = 0f;
        if (Input.GetKey(KeyCode.A)) moveX = -5f;
        if (Input.GetKey(KeyCode.D)) moveX = 5f;
        if (moveX == 0) moveX = 5f;

        Vector2 dodgeDirection = new Vector2(moveX, 0).normalized;

        isDodging = true;
        dodgeEndTime = Time.time + dodgeDuration;
        nextDodgeTime = Time.time + dodgeCooldown;

        rb.gravityScale = 0f;
        float dodgeSpeed = dodgeDistance / dodgeDuration;
        rb.linearVelocity = dodgeDirection * dodgeSpeed;
    }

    public bool IsDodging => isDodging;
}