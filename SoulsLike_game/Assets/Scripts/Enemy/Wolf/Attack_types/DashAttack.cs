using System.Collections;
using UnityEngine;

public class DashAttack : EnemyAttack
{
    [SerializeField] private float dashStartDistance = 4f;
    [SerializeField] private float dashDistance = 8f;
    [SerializeField] private float attackDistance = 0.4f;
    [SerializeField] private float dashSpeed = 20f;
    [SerializeField] private float damage = 10f;
    [SerializeField] private bool canBeBlocked = false;

    public override float AttackDistance => dashStartDistance;
    public override float AttackRange => dashDistance;
    public override bool IsPerforming => isPerforming;

    private bool isPerforming;
    private Animator animator;
    private PlayerChaserComponent chaser;
    private GameObject target;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        chaser = GetComponent<PlayerChaserComponent>();
    }

    public override void Perform(GameObject attacker, GameObject target)
    {
        if (target == null) return;
        this.target = target;
        animator.SetTrigger("isAttacking");
        StartCoroutine(DashRoutine(attacker, target));
    }

    private IEnumerator DashRoutine(GameObject attacker, GameObject target)
    {
        var startPos = attacker.transform.position;
        var endPos = attacker.transform.position;
        endPos.x = target.transform.position.x;
        var direction = (endPos - startPos).normalized;
        var traveledDistance = 0f;
        var damageDealt = false;

        while (traveledDistance < dashDistance)
        {
            MakeStep(attacker, ref traveledDistance, direction);
            if (target == null)
            {
                yield return null;
                continue;
            }

            if (!damageDealt)
                damageDealt = TryAttack(attacker, target, damageDealt);

            yield return null;
        }
    }

    private void MakeStep(GameObject attacker, ref float traveledDistance, Vector2 direction)
    {
        var step = GetStep(traveledDistance);
        attacker.transform.Translate(direction * step, Space.World);
        traveledDistance += step;
    }

    private float GetStep(float traveledDistance)
    {
        var step = dashSpeed * Time.deltaTime;
        if (traveledDistance + step > dashDistance)
            step = dashDistance - traveledDistance;
        return step;
    }

    private bool TryAttack(GameObject attacker, GameObject target, bool damageDealt)
    {
        var currentDist = Vector2.Distance(attacker.transform.position, target.transform.position);
        if (currentDist <= attackDistance && !damageDealt)
        {
            var healthComponent = target.GetComponent<PlayerHealthComponent>();
            var attack = new AttackData(damage, attacker, canBeBlocked);
            healthComponent.TakeDamage(attack);
            damageDealt = true;
        }
        return damageDealt;
    }

    public void OnAttackStart()
    {
        isPerforming = true;
        chaser.ClearTarget();
    }

    public void OnAttackEnd()
    {
        isPerforming = false;
        chaser.SetTarget(target);
    }
}
