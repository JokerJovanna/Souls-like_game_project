using UnityEngine;

public class AttackComponent : MonoBehaviour
{
    [SerializeField] private EnemyAttack[] attacks;
    [SerializeField] private float cooldown = 2f;

    private PlayerChaserComponent chaser;
    private SpriteRenderer sprite;
    private float cooldownTimer = 0f;
    private GameObject target;
    private int nextAttack;
    private EnemyAttack currentAttack;

    public void SetTarget(GameObject target)
    {
        this.target = target;
    }

    public void ClearTarget()
    {
        this.target = null;
    }

    private void Awake()
    {
        chaser = GetComponent<PlayerChaserComponent>();
        sprite = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        nextAttack = Random.Range(0, attacks.Length);
        currentAttack = attacks[nextAttack];
        chaser.StopDistance = attacks[nextAttack].AttackDistance;
    }

    void Update()
    {
        if (target == null) return;
        if (currentAttack.IsPerforming) return;

        if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;
            return;
        }

        currentAttack = attacks[nextAttack];
        sprite.flipX = target.transform.position.x - gameObject.transform.position.x > 0;

        if (Vector2.Distance(transform.position, target.transform.position) <= currentAttack.AttackDistance)
            PerformAttack(currentAttack);
    }

    private void PerformAttack(EnemyAttack attack)
    {
        attack.Perform(gameObject, target);
        cooldownTimer = cooldown;
        nextAttack = Random.Range(0, attacks.Length);
        chaser.StopDistance = attacks[nextAttack].AttackDistance;
    }
}
