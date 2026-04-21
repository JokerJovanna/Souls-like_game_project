using UnityEngine;

public class PlayerSingleMeleeAttack : Attack
{
    [SerializeField] private float damage = 25f;
    [SerializeField] private bool canBeBlocked = true;
    [SerializeField] private float attackDistance = 1.5f;

    public override float AttackDistance => attackDistance;

    public override void Perform(GameObject attacker, GameObject target)
    {
        if (target == null) return;

        // У врага должен быть компонент HealthComponent (или любой другой с TakeDamage(AttackData))
        var healthComponent = target.GetComponent<EnemyHealthComponent>();
        if (healthComponent == null) return;

        var attackData = new AttackData(damage, attacker, canBeBlocked);
        healthComponent.TakeDamage(attackData);
    }
}