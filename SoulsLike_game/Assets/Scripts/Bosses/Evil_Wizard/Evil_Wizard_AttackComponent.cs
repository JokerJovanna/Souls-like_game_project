using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Evil_Wizard_AttackComponent : MonoBehaviour
{
    [SerializeField] private float minMeleeCd = 0.5f;
    [SerializeField] private float maxMeleeCd = 3f;
    [SerializeField] private float spawnAttackCooldown = 20f;

    private MeleeAttack_1 attack_1;
    private MeleeAttack_2 attack_2;
    private SpawnAttack attack_spawn;
    private PlayerChaserComponent chaser;
    private float lastMeleeTime;
    private float lastSpawnTime;
    private GameObject target;
    private SpriteRenderer sprite;
    private EnemyAttack currentAttack;
    private List<GameObject> activeMinions = new();
    private float meleeAttackCooldown;

    private void Awake()
    {
        attack_1 = GetComponent<MeleeAttack_1>();
        attack_2 = GetComponent<MeleeAttack_2>();
        attack_spawn = GetComponent<SpawnAttack>();
        chaser = GetComponent<PlayerChaserComponent>();
        sprite = GetComponent<SpriteRenderer>();

        target = GameObject.FindWithTag("Player");

        meleeAttackCooldown = UnityEngine.Random.Range(minMeleeCd, 100 * maxMeleeCd) / 100f;
        lastMeleeTime = -meleeAttackCooldown;
        lastSpawnTime = -spawnAttackCooldown;

        attack_spawn.OnSpawn += OnMinionSpawned;
    }

    private void Update()
    {
        if (target == null) return;
        if (currentAttack == null) GetNextAttack();
        if (currentAttack == null) return;

        chaser.StopDistance = currentAttack.AttackDistance;

        sprite.flipX = target.transform.position.x - transform.position.x < 0;
        if (Mathf.Abs(transform.position.x - target.transform.position.x) <= currentAttack.AttackDistance)
        {
            currentAttack.Perform(gameObject, target);
            if (currentAttack is MeleeAttack_1 || currentAttack is MeleeAttack_2)
            {
                meleeAttackCooldown = UnityEngine.Random.Range(0, 301) / 100f;
                lastMeleeTime = Time.time;
            }
            else lastSpawnTime = Time.time;
                currentAttack = null;
        }
    }

    private void GetNextAttack()
    {
        if (activeMinions.Count == 0 && Time.time - lastSpawnTime >= spawnAttackCooldown)
            currentAttack = attack_spawn;


        else if (Time.time - lastMeleeTime >= meleeAttackCooldown)
            currentAttack = (UnityEngine.Random.Range(0, 2) == 0) ? attack_1 : attack_2;

        else currentAttack = null;
    }

    private void OnMinionSpawned(GameObject minion)
    {
        activeMinions.Add(minion);
        var hp = minion.GetComponent<EnemyHealthComponent>();
        if (hp != null)
            hp.OnDied += () => activeMinions.Remove(minion);
    }
}
