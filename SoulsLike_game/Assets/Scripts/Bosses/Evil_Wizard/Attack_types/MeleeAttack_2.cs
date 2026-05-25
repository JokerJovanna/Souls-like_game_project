using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class MeleeAttack_2 : EnemyAttack
{
    [SerializeField] private float damage = 40f;
    [SerializeField] private float attackRange = 3.5f;
    [SerializeField] private float attackDistance = 2f;
    [SerializeField] private bool canBeBlocked = true;

    private Animator animator;
    private PlayerChaserComponent chaser;
    private SpriteRenderer sprite;
    private GameObject currentTarget;
    private GameObject currentAttacker;
    private bool isPerforming;

    public override float AttackDistance => attackDistance;
    public override float AttackRange => attackRange;
    public override bool IsPerforming => isPerforming;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        chaser = GetComponent<PlayerChaserComponent>();
        sprite = GetComponent<SpriteRenderer>();
    }

    public override void Perform(GameObject attacker, GameObject target)
    {
        if (target == null) return;
        currentAttacker = attacker;
        currentTarget = target;
        if (Vector2.Distance(currentTarget.transform.position, attacker.transform.position) > attackRange) return;
        animator.SetTrigger("isAttacking_2");
    }

    private void OnAttack_2End()
    {
        chaser.SetTarget(currentTarget);
        isPerforming = false;
    }

    private void OnAttack_2Start()
    {
        chaser.ClearTarget();
        isPerforming = true;
    }

    private void OnAttack_2Hit()
    {
        if (currentTarget == null) return;
        if (!IsTargetInFront()) return;
        var healthComponent = currentTarget.GetComponent<PlayerHealthComponent>();
        var attack = new AttackData(damage, currentAttacker, canBeBlocked);
        healthComponent.TakeDamage(attack);
    }

    private bool IsTargetInFront()
    {
        var attcakerPos = currentAttacker.transform.position;
        var targetPos = currentTarget.transform.position;
        if (Vector2.Distance(targetPos, attcakerPos) > attackRange) return false;

        var dir = targetPos.x - attcakerPos.x;
        if (dir >= 0 && sprite.flipX == false) return true;
        if (dir <= 0 && sprite.flipX == true) return true;

        return false;
    }
}
