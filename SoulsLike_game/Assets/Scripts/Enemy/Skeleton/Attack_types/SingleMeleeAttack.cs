using UnityEngine;

public class SingleMeleeAttack : Attack
{
    [SerializeField] private float damage = 10f;
    [SerializeField] private bool canBeBlocked = true;
    [SerializeField] private float attackDistance = 1.5f;

    public override float AttackDistance => attackDistance;

    public override void Perform(GameObject attacker, GameObject target)
    {
        if (target == null) return;

        var healthComponent = target.GetComponent<HealthComponent>();
        var attack = new AttackData(damage, attacker, canBeBlocked);
        healthComponent.TakeDamage(attack);
    }
}
