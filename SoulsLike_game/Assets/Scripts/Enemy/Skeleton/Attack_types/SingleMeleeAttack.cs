using System.Reflection;
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
        if (Vector2.Distance(target.transform.position, attacker.transform.position) > attackDistance) return;

        var healthComponent = target.GetComponent<PlayerHealthComponent>();
        var attack = new AttackData(damage, attacker, canBeBlocked);
        healthComponent.TakeDamage(attack);
    }
}
