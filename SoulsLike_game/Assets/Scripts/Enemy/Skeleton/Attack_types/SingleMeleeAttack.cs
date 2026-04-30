using UnityEngine;

public class SingleMeleeAttack : Attack
{
    [SerializeField] private float damage = 10f;
    [SerializeField] private bool canBeBlocked = true;
    [SerializeField] private float attackDistance = 1.5f;
    [SerializeField] private float attackRange = 2f;

    public override float AttackDistance => attackDistance;
    public override float AttackRange => attackRange;
    public override bool IsPerforming => isPerforming;

    private Animator animator;
    private PlayerChaserComponent chaser;
    private SpriteRenderer sprite;
    private EnemySoundComponent sound;
    private GameObject currentTarget;
    private GameObject currentAttacker;
    private bool isPerforming;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        chaser = GetComponent<PlayerChaserComponent>();
        sprite = GetComponent<SpriteRenderer>();
        sound = GetComponent<EnemySoundComponent>();
    }

    public override void Perform(GameObject attacker, GameObject target)
    {
        if (target == null) return;
        if (Vector2.Distance(target.transform.position, attacker.transform.position) > attackRange) return;
        animator.SetTrigger("isAttacking");
        currentAttacker = attacker;
        currentTarget = target;
    }

    private void OnAttackEnd()
    {
        chaser.enabled = true;
        isPerforming = false;
    }

    private void OnAttackStart()
    {
        chaser.enabled = false;
        isPerforming = true;
    }

    private void OnHit()
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
        if (Vector2.Distance(targetPos, attcakerPos) > attackDistance) return false;

        var dir = targetPos.x - attcakerPos.x;
        if (dir >= 0 && sprite.flipX == true) return true;
        if (dir <= 0 && sprite.flipX == false) return true;

        return false;
    }
}
