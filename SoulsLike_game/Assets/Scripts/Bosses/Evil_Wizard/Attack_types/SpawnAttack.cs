using System.Collections;
using TMPro.Examples;
using UnityEngine;

public class SpawnAttack : EnemyAttack
{
    [SerializeField] private GameObject skeletonPrefab;
    [SerializeField] private GameObject wolfPrefab;
    [SerializeField] private Transform[] points;

    private float attackDistance = float.MaxValue;
    private float attackRange = float.MaxValue;
    private bool isPerforming;
    private GameObject target;

    private Animator animator;
    private PlayerChaserComponent chaser;

    public override float AttackDistance => attackDistance;
    public override float AttackRange => attackRange;
    public override bool IsPerforming => isPerforming;

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
    }

    private void OnAttack_3End()
    {
        chaser.SetTarget(target);
        isPerforming = false;
    }

    private void OnAttack_3Perform()
    {
        CreateSkeleton(points[0]);
        CreateWolf(points[1]);
        CreateSkeleton(points[2]);
    }

    private void CreateSkeleton(Transform pos)
    {
        var skeleton = Instantiate(skeletonPrefab, pos);
        var script = skeleton.GetComponent<SkeletonScript>();
        script.ResizeTriggerCollider(30);
        script.OnPlayerDetected(target);
    }

    private void CreateWolf(Transform pos)
    {
        var wolf = Instantiate(wolfPrefab, pos);
        var script = wolf.GetComponent<WolfScript>();
        script.ResizeTriggerCollider(30);
        script.OnPlayerDetected(target);
    }
}
