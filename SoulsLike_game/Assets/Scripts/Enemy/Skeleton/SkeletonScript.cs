using System;
using UnityEngine;

public class SkeletonScript : MonoBehaviour
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
