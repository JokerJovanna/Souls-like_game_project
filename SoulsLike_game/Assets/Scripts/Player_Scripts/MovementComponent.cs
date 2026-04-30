using UnityEngine;
using System.Collections;

public class MovementComponent : MonoBehaviour
{
    public float speed = 5f;
    public AudioClip footstepSound;
    public float footstepInterval = 0.4f;

    private Rigidbody2D rb;
    private DodgeComponent dodge;
    private SpriteRenderer spriteRenderer;
    private PlayerAttackComponent attack;
    private JumpComponent jump;
    private AudioSource audioSource;

    private float currentHorizontalSpeed;
    private float lastMoveX = 0f;
    private Coroutine footstepCoroutine;

    public float HorizontalSpeed => Mathf.Abs(currentHorizontalSpeed);
    public float LastDirection { get; private set; } = 1f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        dodge = GetComponent<DodgeComponent>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        attack = GetComponent<PlayerAttackComponent>();
        audioSource = GetComponent<AudioSource>();
        jump = GetComponent<JumpComponent>();
    }

    void FixedUpdate()
    {
        if ((dodge != null && dodge.IsDodging) || (attack != null && attack.IsAttacking))
        {
            if (footstepCoroutine != null)
            {
                StopCoroutine(footstepCoroutine);
                footstepCoroutine = null;
            }
            return;
        }

        float moveX = 0f;
        if (Input.GetKey(KeyCode.A)) moveX = -1f;
        if (Input.GetKey(KeyCode.D)) moveX = 1f;
        rb.linearVelocity = new Vector2(moveX * speed, rb.linearVelocity.y);
        currentHorizontalSpeed = rb.linearVelocity.x;

        bool canMove = moveX != 0;
        bool isOnGround = jump != null && jump.IsGrounded;

        if (canMove && isOnGround)
        {
            LastDirection = moveX;
            spriteRenderer.flipX = moveX < 0;

            if (footstepCoroutine == null && audioSource != null && footstepSound != null)
                footstepCoroutine = StartCoroutine(PlayFootsteps());
        }
        else
        {
            if (footstepCoroutine != null)
            {
                StopCoroutine(footstepCoroutine);
                footstepCoroutine = null;
            }
        }

        lastMoveX = moveX;
    }


    private IEnumerator PlayFootsteps()
    {
        if (jump != null && jump.IsGrounded)
            audioSource.PlayOneShot(footstepSound);

        while (true)
        {
            yield return new WaitForSeconds(footstepInterval);

            bool stillOnGround = jump != null && jump.IsGrounded;
            bool stillMoving = Mathf.Abs(rb.linearVelocity.x) > 0.1f;

            if (stillOnGround && stillMoving)
                audioSource.PlayOneShot(footstepSound);
            else
                break;
        }
        footstepCoroutine = null;
    }
}
