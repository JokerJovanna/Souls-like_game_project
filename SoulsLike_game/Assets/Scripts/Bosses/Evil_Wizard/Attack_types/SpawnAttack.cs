using System.Collections;
using TMPro.Examples;
using UnityEngine;

public class SpawnAttack : EnemyAttack
{
    [SerializeField] private GameObject skeletonPrefab;
    [SerializeField] private Transform[] points;

    private float attackDistance = float.MaxValue;
    private float attackRange = float.MaxValue;
    private bool isPerforming;
    private GameObject target;
    private bool hasSpawned;

    private Animator animator;
    private PlayerChaserComponent chaser;

    public override float AttackDistance => attackDistance;
    public override float AttackRange => attackRange;
    public override bool IsPerforming => isPerforming;

    public System.Action<GameObject> OnSpawn;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        chaser = GetComponent<PlayerChaserComponent>();
    }

    public override void Perform(GameObject attacker, GameObject target)
    {
        animator.SetTrigger("isAttacking_3");
        this.target = target;
    }
    
    private void OnAttack_3Start()
    {
        chaser.ClearTarget();
        isPerforming = true;
        hasSpawned = false;
    }

    private void OnAttack_3End()
    {
        chaser.SetTarget(target);
        isPerforming = false;
    }

    private void OnAttack_3Perform()
    {
        if (hasSpawned) return;
        CreateSkeleton(points[0]);
        CreateSkeleton(points[1]);
        CreateSkeleton(points[2]);
        hasSpawned = true;
    }

    private void CreateSkeleton(Transform pos)
    {
        var skeleton = Instantiate(skeletonPrefab, pos);
        var script = skeleton.GetComponent<SkeletonScript>();
        script.ResizeTriggerCollider(30);
        script.OnPlayerDetected(target);
        OnSpawn?.Invoke(skeleton);
    }
}
