using UnityEngine;

public class BlockComponent : MonoBehaviour
{
    public KeyCode blockKey = KeyCode.Mouse1;       
    public KeyCode alternativeBlockKey = KeyCode.L;

    private bool isBlocking = false;
    private Animator animator;
    private DodgeComponent dodge;

    void Start()
    {
        animator = GetComponent<Animator>();
        dodge = GetComponent<DodgeComponent>();
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
            return;
        }

        bool blockingNow = Input.GetKey(blockKey) || Input.GetKey(alternativeBlockKey);

        if (isBlocking != blockingNow)
        {
            isBlocking = blockingNow;
            if (animator != null)
                animator.SetBool("IsBlocking", isBlocking);
        }
    }

    public bool IsBlocking => isBlocking;
}