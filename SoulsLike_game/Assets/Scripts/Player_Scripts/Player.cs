using UnityEngine;

public class Player : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Deactivate()
    {
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        animator.Rebind();
    }
}