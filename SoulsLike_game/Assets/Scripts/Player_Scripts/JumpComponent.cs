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

    private Rigidbody2D rb;
    private StaminaComponent stamina;
    private int remainingJumps;
    private float nextJumpTime;
    private bool isGrounded;
    private bool wasGrounded;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        stamina = GetComponent<StaminaComponent>();
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
            }
        }
    }
}
