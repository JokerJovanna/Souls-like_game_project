using System;
using UnityEngine;

public class PlayerChaserComponent : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    public float StopDistance = 1.5f;

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
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null) return;

        var nextPos = target.transform.position;
        nextPos.y = transform.position.y;
        if (Vector2.Distance(target.position, transform.position) > StopDistance)
        {
            animator.SetBool("isMoving", true);
            var newPosition = Vector2.MoveTowards(transform.position, nextPos, speed * Time.deltaTime);
            transform.position = newPosition;

            var direction = target.position.x - transform.position.x;
            if (direction != 0)
                sprite.flipX = direction > 0;
        }
        else
            animator.SetBool("isMoving", false);
    }
}
