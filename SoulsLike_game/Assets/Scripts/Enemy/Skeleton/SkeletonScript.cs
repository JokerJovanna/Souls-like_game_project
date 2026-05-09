using System;
using UnityEngine;

public class SkeletonScript : MonoBehaviour
{
    private EnemyHealthComponent health;
    private AttackComponent attack;
    private PatrolComponent patrol;
    private PlayerChaserComponent chaser;
    private PlayerDetectorComponent detector;

    void Awake()
    {
        health = GetComponent<EnemyHealthComponent>();
        attack = GetComponent<AttackComponent>();
        patrol = GetComponent<PatrolComponent>();
        chaser = GetComponent<PlayerChaserComponent>();
        detector = GetComponent<PlayerDetectorComponent>();
        if (health == null) Debug.LogError("HealthComponent missing");
        if (attack == null) Debug.LogError("AttackComponent missing");
        if (patrol == null) Debug.LogError("PatrolComponent missing");
        if (chaser == null) Debug.LogError("PlayerChaserComponent missing");
        if (detector == null) Debug.LogError("PlayerDetectorComponent missing");
    }

    void Start()
    {
        InitializeComponents();
    }

    public void OnPlayerDetected(GameObject player)
    {
        patrol.enabled = false;
        chaser.SetTarget(player);
        attack.SetTarget(player);
    }

    public void OnPlayerLost(GameObject player)
    {
        patrol.enabled = true;
        chaser.ClearTarget();
        attack.ClearTarget();
    }

    private void OnDestroy()
    {
        if (detector != null)
        {
            detector.OnPlayerDetected -= OnPlayerDetected;
            detector.OnPlayerLost -= OnPlayerLost;
        }
    }

    private void InitializeComponents()
    {
        health.enabled = true;
        attack.enabled = true;
        patrol.enabled = true;
        chaser.enabled = true;
        detector.enabled = true;

        detector.OnPlayerDetected += OnPlayerDetected;
        detector.OnPlayerLost += OnPlayerLost;
    }
}
