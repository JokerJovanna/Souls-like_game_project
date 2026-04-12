using System;
using UnityEngine;

public class PlayerChaserComponent : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float stopDistance = 1.5f;
    [SerializeField] private SpriteRenderer sprite;

    private Transform target;

    public void SetTarget(GameObject target)
    {
        if (target == null) return;
        this.target = target.transform;
    }

    public void ClearTarget()
    {
        target = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null) return;

        if (Vector2.Distance(target.position, transform.position) > stopDistance)
        {
            var newPosition = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
            transform.position = newPosition;

            var direction = target.position.x - transform.position.x;
            if (direction != 0)
                sprite.flipX = direction < 0;
        }
    }
}
