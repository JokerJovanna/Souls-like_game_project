using System;
using UnityEngine;

public class SkeletonScript : MonoBehaviour
{
    private AttackComponent attack;
    private PatrolComponent patrol;
    private PlayerChaserComponent chaser;
    private PlayerDetectorComponent detector;
    private EnemyHealthComponent health;
    private EnemySoundComponent sound;

    void Awake()
    {
        attack = GetComponent<AttackComponent>();
        patrol = GetComponent<PatrolComponent>();
        chaser = GetComponent<PlayerChaserComponent>();
        detector = GetComponentInChildren<PlayerDetectorComponent>();
        health = GetComponent<EnemyHealthComponent>();
        sound = GetComponent<EnemySoundComponent>();
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

    public void OnPlayerLost()
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
        health.OnDied -= DisableAllComponents;
    }

    private void InitializeComponents()
    {
        health.enabled = true;
        attack.enabled = true;
        patrol.enabled = true;
        chaser.enabled = true;
        detector.enabled = true;
        sound.enabled = true;

        detector.OnPlayerDetected += OnPlayerDetected;
        detector.OnPlayerLost += OnPlayerLost;
        health.OnDied += DisableAllComponents;
    }

    private void DisableAllComponents()
    {
        health.enabled = false;
        attack.enabled = false;
        patrol.enabled = false;
        chaser.enabled = false;
        detector.enabled = false;
        sound.enabled = false;
        foreach (var col in GetComponents<BoxCollider2D>())
            if (col.isTrigger) col.enabled = false;
    }

    public void ResizeTriggerCollider(float coef)
    {
        BoxCollider2D triggerCollider = null;
        foreach (var col in GetComponentsInChildren<BoxCollider2D>())
            if (col.isTrigger) triggerCollider = col;
        var size = triggerCollider.size;
        var newSize = new Vector2(size.x * coef, size.y * coef);
        triggerCollider.size = newSize;
    }
}
