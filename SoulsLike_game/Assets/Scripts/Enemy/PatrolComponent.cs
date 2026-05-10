using UnityEngine;
using UnityEngine.Rendering;
using static UnityEngine.GraphicsBuffer;

public class PatrolComponent : MonoBehaviour
{
    [SerializeField] private float speed = 5.0f;
    [SerializeField] private Transform[] points;
    [SerializeField] private float minWaitTime = 1.5f;
    [SerializeField] private float maxWaitTime = 5.5f;

    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private Animator animator;
    private int nextPoint;
    private float waitTime;
    private bool isWaiting;

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        if (points == null || points.Length == 0)
        {
            enabled = false;
            Debug.LogWarning($"{name}: Patrol has no waypoints, disabling.");
            return;
        }
        nextPoint = Random.Range(0, points.Length);
    }

    void FixedUpdate()
    {
        if (isWaiting)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            animator.SetBool("isMoving", false);
            WaitOrPickNext();
            return;
        }

        MoveTowardsTarget();
        if (HasReachedTarget())
            isWaiting = true;
    }

    private void WaitOrPickNext()
    {
        if (waitTime > 0)
            waitTime -= Time.fixedDeltaTime;
        else
        {
            waitTime = minWaitTime + (maxWaitTime - minWaitTime) * Random.value;
            nextPoint = Random.Range(0, points.Length);
            isWaiting = false;
        }
    }

    private bool HasReachedTarget()
        => Mathf.Abs(transform.position.x - points[nextPoint].position.x) < 0.1f;

    private void MoveTowardsTarget()
    {
        animator.SetBool("isMoving", true);
        var nextPos = points[nextPoint].position;
        var direction = Mathf.Sign(points[nextPoint].position.x - transform.position.x);
        
        if (nextPos.x - transform.position.x != 0)
            sprite.flipX = (nextPos.x - transform.position.x) > 0;

        rb.linearVelocity = new Vector2(direction * speed, rb.linearVelocity.y);
    }
}
