using UnityEngine;

public class AttackComponent : MonoBehaviour
{
    [SerializeField] private Attack[] attacks;
    [SerializeField] private float cooldown = 2f;

    private float cooldownTimer;
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
        }
    }
}
