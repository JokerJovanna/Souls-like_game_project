using UnityEngine;

public class BlockComponent : MonoBehaviour
{
    public KeyCode blockKey = KeyCode.Mouse1;
    public KeyCode alternativeBlockKey = KeyCode.L;

    public float perfectBlockWindow = 0.2f;
    public AudioClip perfectBlockSound;

    public bool resetVelocityOnBlockStart = true; 

    private bool isBlocking = false;
    private bool justBlockedPerfect = false;
    private float lastBlockPressTime = -999f;

    private Animator animator;
    private DodgeComponent dodge;
    private AudioSource audioSource;
    private Rigidbody2D rb;
    private bool wasBlocking = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        dodge = GetComponent<DodgeComponent>();
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (dodge != null && dodge.IsDodging)
        {
            if (isBlocking)
            {
                isBlocking = false;
                if (animator != null) animator.SetBool("IsBlocking", false);
            }
            wasBlocking = false;
            return;
        }

        bool blockingNow = Input.GetKey(blockKey) || Input.GetKey(alternativeBlockKey);

        if (blockingNow && !wasBlocking)
        {
            lastBlockPressTime = Time.time;
            justBlockedPerfect = false;
            if (resetVelocityOnBlockStart && rb != null)
            {
                rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            }
        }

        isBlocking = blockingNow;
        wasBlocking = blockingNow;

        if (animator != null)
            animator.SetBool("IsBlocking", isBlocking);
    }

    public bool IsPerfectBlock()
    {
        if (!isBlocking) return false;
        if (justBlockedPerfect) return false;

        bool perfect = (Time.time - lastBlockPressTime) <= perfectBlockWindow;
        if (perfect)
        {
            justBlockedPerfect = true;
            if (audioSource != null && perfectBlockSound != null)
                audioSource.PlayOneShot(perfectBlockSound);
        }
        return perfect;
    }

    public bool IsBlocking => isBlocking;
}