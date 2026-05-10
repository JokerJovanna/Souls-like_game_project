using UnityEngine;

public class BlockComponent : MonoBehaviour
{
    public KeyCode blockKey = KeyCode.Mouse1;
    public KeyCode alternativeBlockKey = KeyCode.L;

    public float perfectBlockWindow = 0.2f;
    public bool resetVelocityOnBlockStart = true; 

    private bool isBlocking = false;
    private bool wasBlocking = false;
    private bool justBlockedPerfect = false;
    private float lastBlockPressTime = -999f;

    private DodgeComponent dodge;

    private AudioSource audioSource;
    public AudioClip perfectBlockSound;

    private Animator animator;
    private Rigidbody2D rb;

    public bool IsBlocking => isBlocking;

    void Start()
    {
        dodge = GetComponent<DodgeComponent>();

        audioSource = GetComponent<AudioSource>();

        animator = GetComponent<Animator>();
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

        var blockingNow = Input.GetKey(blockKey) || Input.GetKey(alternativeBlockKey);

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
        if (!isBlocking || justBlockedPerfect) return false;

        var perfect = (Time.time - lastBlockPressTime) <= perfectBlockWindow;
        if (perfect)
        {
            justBlockedPerfect = true;
            if (audioSource != null && perfectBlockSound != null)
                audioSource.PlayOneShot(perfectBlockSound);
        }
        return perfect;
    }
}