using System;
using UnityEngine;

public class PlayerChaserComponent : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private bool spriteFacesLeft = true;
    public float StopDistance = 1.5f;

    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private Transform target;
    private Animator animator;

    public void SetTarget(GameObject target)
    {
        if (target == null) return;
        this.target = target.transform;
    }

    public void ClearTarget()
    {
        target = null;
        animator.SetBool("isMoving", false);
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
    }

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (target == null) return;

        if (Math.Abs(target.position.x - transform.position.x) > StopDistance)
        {
            animator.SetBool("isMoving", true);

            var direction = Mathf.Sign(target.position.x - transform.position.x);
            rb.linearVelocity = new Vector2(direction * speed, rb.linearVelocity.y);

            if (direction != 0)
                sprite.flipX = !(direction > 0 ^ spriteFacesLeft);
        }
        else
        {
            animator.SetBool("isMoving", false);
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        }
    }
}
