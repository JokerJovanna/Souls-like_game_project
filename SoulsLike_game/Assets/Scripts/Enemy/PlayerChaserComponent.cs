using System;
using UnityEngine;

public class PlayerChaserComponent : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
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

        var nextPos = target.transform.position;
        nextPos.y = transform.position.y;
        if (Vector2.Distance(target.position, transform.position) > StopDistance)
        {
            animator.SetBool("isMoving", true);
            var newPosition = Vector2.MoveTowards(transform.position, nextPos, speed * Time.fixedDeltaTime);
            rb.MovePosition(newPosition);

            var direction = target.position.x - transform.position.x;
            if (direction != 0)
                sprite.flipX = direction > 0;
        }
        else
            animator.SetBool("isMoving", false);
    }
}
