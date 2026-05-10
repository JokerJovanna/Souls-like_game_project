using UnityEngine;

public class JumpComponent : MonoBehaviour
{
    public bool isGrounded;
    public int maxJumps = 2;
    public float jumpForce = 10f;
    public float jumpCooldown = 0.2f;
    public float staminaCostJump = 20f;
    public float groundCheckRadius = 0.1f;

    private bool wasGrounded;
    private int remainingJumps;
    private float nextJumpTime;

    private StaminaComponent stamina;
    private PlayerAttackComponent attack;
    private BlockComponent block;

    public AudioClip jumpSound;
    private AudioSource audioSource;

    private Rigidbody2D rb;
    public Transform groundCheck;
    public LayerMask groundLayer;

    public event System.Action OnJump;

    public bool IsGrounded => isGrounded;

    void Start()
    {
        stamina = GetComponent<StaminaComponent>();
        block = GetComponent<BlockComponent>();
        attack = GetComponent<PlayerAttackComponent>();

        audioSource = GetComponent<AudioSource>();

        rb = GetComponent<Rigidbody2D>();

        remainingJumps = maxJumps;

        if (groundCheck == null)
        {
            var checkObj = new GameObject("GroundCheck");
            checkObj.transform.SetParent(transform);
            checkObj.transform.localPosition = new Vector3(0, -0.5f, 0);
            groundCheck = checkObj.transform;
        }
    }

    void Update()
    {
        if ((attack != null && attack.IsAttacking) || (block != null && block.IsBlocking)) return;

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        if (isGrounded && !wasGrounded) remainingJumps = maxJumps;
        wasGrounded = isGrounded;

        if (Input.GetKeyDown(KeyCode.Space) && remainingJumps > 0 && Time.time >= nextJumpTime)
        {
            if (stamina.TrySpendStamina(staminaCostJump))
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                remainingJumps--;
                nextJumpTime = Time.time + jumpCooldown;
                OnJump?.Invoke();

                if (audioSource != null && jumpSound != null) audioSource.PlayOneShot(jumpSound);
            }
        }
    }
}
