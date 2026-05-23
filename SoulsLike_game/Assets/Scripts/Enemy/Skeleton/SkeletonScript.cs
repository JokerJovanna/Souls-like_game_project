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
        detector = GetComponent<PlayerDetectorComponent>();
        health = GetComponent<EnemyHealthComponent>();
        sound = GetComponent<EnemySoundComponent>();
    }

    void Start()
    {
        InitializeComponents();
        DisableCollisionWithPlayer();
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
    }

    private void DisableCollisionWithPlayer()
    {
        var player = GameObject.FindWithTag("Player");
        if (player == null) return;
        var player_col = player.GetComponent<Collider2D>();
        var myCollider = GetComponent<Collider2D>();
        Physics2D.IgnoreCollision(player_col, myCollider);
    }

    public void ResizeTriggerCollider(float coef)
    {
        BoxCollider2D triggerCollider = null;
        foreach (var col in GetComponents<BoxCollider2D>())
            if (col.isTrigger) triggerCollider = col;
        var size = triggerCollider.size;
        var newSize = new Vector2(size.x * coef, size.y * coef);
        triggerCollider.size = newSize;
    }
}
