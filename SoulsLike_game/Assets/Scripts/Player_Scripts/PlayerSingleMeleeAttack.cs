using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerSingleMeleeAttack : Attack
{
    [SerializeField] private float damage = 25f;
    [SerializeField] private bool canBeBlocked = true;
    [SerializeField] private float attackDistance = 1.5f;

    public override float AttackDistance => attackDistance;
    public override float AttackRange => attackDistance;
    public override bool IsPerforming => false;

    private GameObject attacker;
    private PlayerAttackComponent attackComponent;

    public override void Perform(GameObject attacker)
    {
        this.attacker = attacker;
        attackComponent = GetComponent<PlayerAttackComponent>();
    }

    private void OnHit()
    {
        if (attacker == null) return;
        if (attackComponent == null) return;

        var targets = FindTargets(attackDistance);

        foreach (var target in targets)
        {
            if (target == null) continue;
            var distance = GetDistance(target);
            if (distance > AttackDistance) continue;

            var toTarget = (target.transform.position - attacker.transform.position).normalized;
            var angle = Vector2.Angle(GetForwardDirection(), toTarget);
            if (angle > attackComponent.AttackAngle / 2f) continue;

            var healthComponent = target.GetComponent<EnemyHealthComponent>();
            if (healthComponent == null) continue;

            var attackData = new AttackData(damage, attacker, canBeBlocked);
            healthComponent.TakeDamage(attackData);
        }
    }

    public Vector2 GetForwardDirection()
    {
        var right = transform.right;
        var sr = GetComponent<SpriteRenderer>();
        if (sr != null && sr.flipX) right = -right;
        return right.normalized;
    }

    private Collider2D FindTargetCollider(GameObject target)
    {
        Collider2D collider = null;
        foreach (var col in target.GetComponents<Collider2D>())
            if (!col.isTrigger) collider = col;
        return collider;
    }

    private float GetDistance(GameObject target)
    {
        var targetCol = FindTargetCollider(target);
        if (targetCol == null) return float.MaxValue;

        var closestPoint = targetCol.ClosestPoint(attacker.transform.position);
        return Vector2.Distance(closestPoint, attacker.transform.position);
    }

    private List<GameObject> FindTargets(float attackDistance)
    {
        var hitColliders = Physics2D.OverlapCircleAll(transform.position, attackDistance);
        var forward = GetForwardDirection();
        var halfAngle = attackComponent.AttackAngle * 0.5f;
        var targets = new List<GameObject>();

        foreach (var hit in hitColliders.Where(x => !x.isTrigger))
        {
            var health = hit.GetComponent<EnemyHealthComponent>();
            if (health == null) continue;

            var dirToTarget = (hit.transform.position - transform.position).normalized;
            var angle = Vector2.Angle(forward, dirToTarget);
            if (angle > halfAngle) continue;

            var closestPoint = hit.ClosestPoint(transform.position);
            var distance = Vector2.Distance(transform.position, closestPoint);
            if (distance < attackDistance)
                targets.Add(hit.gameObject);
        }
        return targets;
    }
}