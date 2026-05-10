using UnityEngine;

public class JumpComponent : MonoBehaviour
{
    [SerializeField] private bool isGrounded;
    [SerializeField] private int maxJumps = 2;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float jumpCooldown = 0.2f;
    [SerializeField] private float staminaCostJump = 20f;
    [SerializeField] private float groundCheckRadius = 0.1f;

    private bool wasGrounded;
    private int remainingJumps;
    private float nextJumpTime;

    private StaminaComponent stamina;
    private PlayerAttackComponent attack;
    private BlockComponent block;

    [SerializeField] private AudioClip jumpSound;
    private AudioSource audioSource;

    private Rigidbody2D rb;
    private Animator animator;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    public event System.Action OnJump;

    public bool IsGrounded => isGrounded;

    void Start()
    {
        stamina = GetComponent<StaminaComponent>();
        block = GetComponent<BlockComponent>();
        attack = GetComponent<PlayerAttackComponent>();

        audioSource = GetComponent<AudioSource>();

        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

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
                animator.SetTrigger("JumpTrigger");
            }
        }

        animator.SetBool("IsGrounded", isGrounded);
    }
}
