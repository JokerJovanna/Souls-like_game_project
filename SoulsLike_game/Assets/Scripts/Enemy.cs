using System;
using UnityEngine;

public class Enemy : MonoBehaviour, IEnemy
{
    [SerializeField] private float speed = 5.0f;
    [SerializeField] private Transform[] points;
    private readonly System.Random random = new();
    private int nextPoint;
    private float waitTime;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        waitTime = 0.5f + 4 * (float)random.NextDouble();
        nextPoint = random.Next(points.Length);
    }

    // Update is called once per frame
    void Update()
    {
        var nextPos = transform.position;
        nextPos.x = points[nextPoint].position.x;
        transform.position = Vector2.MoveTowards(transform.position, nextPos, speed * Time.deltaTime);
        if (Vector2.Distance(transform.position, nextPos) < 0.1f)
            if (waitTime <= 0)
            {
                waitTime = 0.5f + 4 * (float)random.NextDouble();
                nextPoint = random.Next(points.Length);
            }
            else
                waitTime -= Time.deltaTime;
    }

    public float Damage => throw new System.NotImplementedException();

    public float CurrentHealth => throw new System.NotImplementedException();

    public float MaxHealth => throw new System.NotImplementedException();

    public void Attack()
    {
        throw new System.NotImplementedException();
    }

    public void Die()
    {
        throw new System.NotImplementedException();
    }

    public void TakeDamage(float amount, GameObject source)
    {
        throw new System.NotImplementedException();
    }
}
