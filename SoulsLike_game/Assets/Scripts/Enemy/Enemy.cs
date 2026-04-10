using System;
using UnityEngine;

public class Enemy : MonoBehaviour
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
}
