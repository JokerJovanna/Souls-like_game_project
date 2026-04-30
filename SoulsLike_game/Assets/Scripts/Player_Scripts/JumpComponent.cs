using UnityEngine;

public class JumpComponent : MonoBehaviour
{
    public float jumpForce = 10f;
    public int maxJumps = 2;
    public float jumpCooldown = 0.2f;
    public float staminaCostJump = 20f;

    public Transform groundCheck;
    public LayerMask groundLayer;
    public float groundCheckRadius = 0.1f;

    public AudioClip jumpSound;

    private Rigidbody2D rb;
    private StaminaComponent stamina;
    private AudioSource audioSource;
    private int remainingJumps;
    private float nextJumpTime;
    public bool isGrounded;
    private bool wasGrounded;

    public event System.Action OnJump;
    private PlayerAttackComponent attack;

    public bool IsGrounded => isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        stamina = GetComponent<StaminaComponent>();
        attack = GetComponent<PlayerAttackComponent>();
        audioSource = GetComponent<AudioSource>();
        remainingJumps = maxJumps;

        if (groundCheck == null)
        {
            GameObject checkObj = new GameObject("GroundCheck");
            checkObj.transform.SetParent(transform);
            checkObj.transform.localPosition = new Vector3(0, -0.5f, 0);
            groundCheck = checkObj.transform;
        }
    }

    void Update()
    {
        if (attack != null && attack.IsAttacking)
        {
            //Debug.Log("Jump blocked by attack");
            return;
        }
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 
            groundCheckRadius, groundLayer);
        if (isGrounded && !wasGrounded) remainingJumps = maxJumps;
        wasGrounded = isGrounded;

        if (Input.GetKeyDown(KeyCode.Space) && remainingJumps > 0 
            && Time.time >= nextJumpTime)
        {
            if (stamina.TrySpendStamina(staminaCostJump))
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                remainingJumps--;
                nextJumpTime = Time.time + jumpCooldown;
                OnJump?.Invoke();

                if (audioSource != null && jumpSound != null)
                    audioSource.PlayOneShot(jumpSound);
            }
        }
    }
}
