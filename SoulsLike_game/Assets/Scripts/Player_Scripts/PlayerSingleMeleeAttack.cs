using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PlayerSingleMeleeAttack : Attack
{
    [SerializeField] private float damage = 25f;
    [SerializeField] private bool canBeBlocked = true;
    [SerializeField] private float attackDistance = 1.5f;

    public override float AttackDistance => attackDistance;

    public override float AttackRange => attackDistance;

    public override bool IsPerforming => false;

    private GameObject attacker;
    private GameObject target;

    public override void Perform(GameObject attacker, GameObject target)
    {
        if (target == null) return;
        this.attacker = attacker;
        this.target = target;
    }

    private void OnHit()
    {
        if (attacker == null) return;
        if (target == null) return;
        // У врага должен быть компонент HealthComponent (или любой другой с TakeDamage(AttackData))
        var healthComponent = target.GetComponent<EnemyHealthComponent>();
        if (healthComponent == null) return;

        var attackData = new AttackData(damage, attacker, canBeBlocked);
        healthComponent.TakeDamage(attackData);
    }
}