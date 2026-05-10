using System.Collections;
using UnityEngine;

public class MovementComponent : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float footstepInterval = 0.4f;

    private float currentHorizontalSpeed;

    private DodgeComponent dodge;
    private PlayerAttackComponent attack;
    private JumpComponent jump;
    private BlockComponent block;

    private AudioSource audioSource;
    [SerializeField] private AudioClip footstepSound;

    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private Coroutine footstepCoroutine;
    private Animator animator;

    public float HorizontalSpeed => Mathf.Abs(currentHorizontalSpeed);
    public float LastDirection { get; private set; } = 1f;

    void Start()
    {
        attack = GetComponent<PlayerAttackComponent>();
        dodge = GetComponent<DodgeComponent>();
        jump = GetComponent<JumpComponent>();
        block = GetComponent<BlockComponent>();

        audioSource = GetComponent<AudioSource>();

        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        if (IsMovementLocked())
        {
            StopFootsteps();
            return;
        }

        var moveX = GetMoveInput();
        ApplyMovement(moveX);
        UpdateSpriteDirection(moveX);
        HandleFootsteps(moveX);

        animator.SetFloat("Speed", HorizontalSpeed);
    }

    private bool IsMovementLocked()
    {
        return (dodge != null && dodge.IsDodging) || (attack != null && attack.IsAttacking) ||
               (block != null && block.IsBlocking);
    }

    private float GetMoveInput()
    {
        var move = 0f;
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) move = -1f;
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) move = 1f;
        return move;
    }

    private void ApplyMovement(float moveX)
    {
        rb.linearVelocity = new Vector2(moveX * speed, rb.linearVelocity.y);
        currentHorizontalSpeed = rb.linearVelocity.x;
        LastDirection = moveX != 0 ? moveX : LastDirection;
    }

    private void UpdateSpriteDirection(float moveX)
    {
        if (moveX != 0) spriteRenderer.flipX = moveX < 0;
    }

    private void HandleFootsteps(float moveX)
    {
        var isOnGround = jump != null && jump.IsGrounded;
        var shouldPlayFootsteps = moveX != 0 && isOnGround;

        if (shouldPlayFootsteps && footstepCoroutine == null && audioSource != null && 
            footstepSound != null)
            footstepCoroutine = StartCoroutine(PlayFootsteps());
        else if (!shouldPlayFootsteps && footstepCoroutine != null) StopFootsteps();
    }

    private void StopFootsteps()
    {
        if (footstepCoroutine != null)
        {
            StopCoroutine(footstepCoroutine);
            footstepCoroutine = null;
        }
    }

    private IEnumerator PlayFootsteps()
    {
        if (jump != null && jump.IsGrounded) audioSource.PlayOneShot(footstepSound);

        while (true)
        {
            yield return new WaitForSeconds(footstepInterval);

            var stillOnGround = jump != null && jump.IsGrounded;
            var stillMoving = Mathf.Abs(rb.linearVelocity.x) > 0.1f;

            if (stillOnGround && stillMoving) audioSource.PlayOneShot(footstepSound);
            else break;
        }
        footstepCoroutine = null;
    }
}
