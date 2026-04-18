using UnityEngine;

public class AttackComponent : MonoBehaviour
{
    [SerializeField] private PlayerChaserComponent chaser;
    [SerializeField] private Attack[] attacks;
    [SerializeField] private float cooldown = 2f;

    private float cooldownTimer = 0f;
    private GameObject target;
    private int nextAttack;

    public void SetTarget(GameObject target)
    {
        this.target = target;
    }

    public void ClearTarget()
    {
        this.target = null;
    }

    private void Start()
    {
        nextAttack = Random.Range(0, attacks.Length);
        chaser.StopDistance = attacks[nextAttack].AttackDistance;
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null) return;

        if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;
            return;
        }

        var attack = attacks[nextAttack];
        if (Vector2.Distance(transform.position, target.transform.position) <= attack.AttackDistance)
        {
            attack.Perform(gameObject, target);
            cooldownTimer = cooldown;
            nextAttack = Random.Range(0, attacks.Length);
            chaser.StopDistance = attacks[nextAttack].AttackDistance;
        }
    }
}
