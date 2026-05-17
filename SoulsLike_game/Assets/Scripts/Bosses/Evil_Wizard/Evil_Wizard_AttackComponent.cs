using UnityEngine;
using System;

public class Evil_Wizard_AttackComponent : MonoBehaviour
{
    [SerializeField] private float spawnAttackCooldown;
    [SerializeField] private float meleeAttackCooldown;

    private EnemyAttack attack_1;
    private EnemyAttack attack_2;
    private EnemyAttack attack_spawn;
    private PlayerChaserComponent chaser;
    private float spawnAttackTimer;
    private float meleeAttackTimer;
    private GameObject target;
    private SpriteRenderer sprite;

    private void Awake()
    {
        attack_1 = GetComponent<MeleeAttack_1>();
        attack_2 = GetComponent<MeleeAttack_2>();
        attack_spawn = GetComponent<SpawnAttack>();
        chaser = GetComponent<PlayerChaserComponent>();
        sprite = GetComponent<SpriteRenderer>();

        target = GameObject.FindWithTag("Player");
        spawnAttackTimer = spawnAttackCooldown;
    }

    private void Update()
    {
        var nextAttack = GetNextAttack();
        if (nextAttack == null) return;
        if (target == null) return;

        var stopDistance = nextAttack.AttackDistance;
        chaser.StopDistance = stopDistance;

        sprite.flipX = target.transform.position.x - gameObject.transform.position.x < 0;
        if (Math.Abs(gameObject.transform.position.x - target.transform.position.x) <= stopDistance)
            nextAttack.Perform(gameObject, target);
    }

    private EnemyAttack GetNextAttack()
    {
        meleeAttackTimer -= Time.deltaTime;
        spawnAttackTimer -= Time.deltaTime;

        if (spawnAttackTimer <= 0)
        {
            spawnAttackTimer = spawnAttackCooldown;
            return attack_spawn;
        }

        if (meleeAttackTimer <= 0)
        {
            meleeAttackTimer = meleeAttackCooldown;
            return (UnityEngine.Random.Range(1, 3) == 1) ? attack_1 : attack_2;
        }
            
        return null;
    }  
}
