using UnityEngine;

public class PatrolComponent : MonoBehaviour
{
    [SerializeField] private float speed = 5.0f;
    [SerializeField] private Transform[] points;

    private SpriteRenderer sprite;
    private int nextPoint;
    private float waitTime;

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (points == null || points.Length == 0)
        {
            enabled = false;
            Debug.LogWarning($"{name}: Patrol has no waypoints, disabling.");
            return;
        }

        waitTime = 0.5f + 4 * Random.value;
        nextPoint = Random.Range(0, points.Length);
    }

    // Update is called once per frame
    void Update()
    {
        MoveTowardsTarget();
        if (HasReachedTarget())
            WaitOrPickNext();
    }

    private void WaitOrPickNext()
    {
        if (waitTime <= 0)
        {
            waitTime = 0.5f + 4 * Random.value;
            nextPoint = Random.Range(0, points.Length);
        }
        else
            waitTime -= Time.deltaTime;
    }

    private bool HasReachedTarget()
        => Mathf.Abs(transform.position.x - points[nextPoint].position.x) < 0.1f;

    private void MoveTowardsTarget()
    {
        var nextPos = transform.position;
        nextPos.x = points[nextPoint].position.x;

        if (nextPos.x - transform.position.x != 0)
            sprite.flipX = (nextPos.x - transform.position.x) < 0;

        transform.position = Vector2.MoveTowards(transform.position, nextPos, speed * Time.deltaTime);
    }
}
