using System;
using UnityEngine;

public class Enemy : MonoBehaviour, IEnemy
{
    [SerializeField] private Patrol patrolComponent;

    void Awake()
    {
        patrolComponent = GetComponent<Patrol>();
    }
    
    void Start()
    {

    }

    void Update()
    {

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
