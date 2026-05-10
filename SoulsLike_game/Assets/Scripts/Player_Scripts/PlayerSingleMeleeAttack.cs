using UnityEngine;

public class PlayerSingleMeleeAttack : Attack
{
    [SerializeField] private float damage = 25f;
    [SerializeField] private bool canBeBlocked = true;
    [SerializeField] private float attackDistance = 1.5f;

    public override float AttackDistance => attackDistance;
    public override float AttackRange => attackDistance;
    public override bool IsPerforming => false;

    private Vector2 attackDirectionAtPerform;

    private GameObject attacker;
    private GameObject target;

    public override void Perform(GameObject attacker, GameObject target)
    {
        if (target == null) return;
        this.attacker = attacker;
        this.target = target;

        var attackComp = attacker.GetComponent<PlayerAttackComponent>();
        if (attackComp != null) attackDirectionAtPerform = attackComp.GetForwardDirection();
        else attackDirectionAtPerform = Vector2.right;
    }

    private void OnHit()
    {
        if (attacker == null || target == null) return;

        var attackComp = attacker.GetComponent<PlayerAttackComponent>();
        Vector2 toTarget = (target.transform.position - attacker.transform.position).normalized;

        if (attackComp != null)
        {
            var currentForward = attackComp.GetForwardDirection();
            var angle = Vector2.Angle(currentForward, toTarget);
            if (angle > attackComp.AttackAngle / 2f) return;
        }
        else
        {
            if (Vector2.Dot(attackDirectionAtPerform, toTarget) < 0.5f) return;
        }

        var healthComponent = target.GetComponent<EnemyHealthComponent>();
        if (healthComponent == null) return;

        var attackData = new AttackData(damage, attacker, canBeBlocked);
        healthComponent.TakeDamage(attackData);
    }
}