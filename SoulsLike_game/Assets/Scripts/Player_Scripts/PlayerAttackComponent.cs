using UnityEngine;

public class PlayerAttackComponent : MonoBehaviour
{
    [SerializeField] private KeyCode attackKey = KeyCode.Mouse0;
    [SerializeField] private Attack[] attacks;
    [SerializeField] private float cooldown = 0.5f;
    [SerializeField] private float staminaCost = 20f;
    [SerializeField] private float attackAngle = 120f;
    [SerializeField] private float attackActiveDuration = 0.3f;

    public AudioClip attackSound;

    private StaminaComponent stamina;
    private float lastAttackTime = -999f;   
    private int nextAttack;
    private bool isAttacking = false;
    private AudioSource audioSource;

    public bool IsAttacking => isAttacking;

    private void Start()
    {
        stamina = GetComponent<StaminaComponent>();
        audioSource = GetComponent<AudioSource>();
        if (stamina == null)
            Debug.LogError("PlayerAttackComponent требует компонент StaminaComponent!");

        if (attacks == null || attacks.Length == 0)
            Debug.LogError("Не назначены атаки в PlayerAttackComponent!");

        nextAttack = Random.Range(0, attacks.Length);
    }

    private void Update()
    {
        if (Input.GetKeyDown(attackKey) && !isAttacking && Time.time >= lastAttackTime + cooldown)
        {
            if (!stamina.TrySpendStamina(staminaCost))
            {
                Debug.Log("Недостаточно выносливости для атаки");
                return;
            }

            isAttacking = true;
            lastAttackTime = Time.time;

            var attack = attacks[nextAttack];
            GameObject target = FindTarget(attack.AttackDistance);

            if (target != null)
            {
                attack.Perform(gameObject, target);
                Debug.Log("Атака по врагу");
            }
            else
            {
                Debug.Log("Атака впустую (нет цели)");
            }
            if (audioSource != null && attackSound != null)
                audioSource.PlayOneShot(attackSound);

            nextAttack = (nextAttack + 1) % attacks.Length;
            Invoke(nameof(ResetAttack), attackActiveDuration);
        }
    }

    private void ResetAttack()
    {
        isAttacking = false;
        //Debug.Log("Атака завершена, управление разблокировано");
    }

    private Vector2 GetForwardDirection()
    {
        var sr = GetComponent<SpriteRenderer>();
        return (sr != null && sr.flipX) ? Vector2.left : Vector2.right;
    }

    private GameObject FindTarget(float attackDistance)
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, attackDistance);
        float closestDistance = attackDistance + 1f;
        GameObject closestTarget = null;
        Vector2 forward = GetForwardDirection();

        foreach (var hit in hitColliders)
        {
            var health = hit.GetComponent<EnemyHealthComponent>();
            if (health == null) continue;

            Vector2 dirToTarget = (hit.transform.position - transform.position).normalized;
            float angle = Vector2.Angle(forward, dirToTarget);

            if (angle <= attackAngle / 2f)
            {
                float dist = Vector2.Distance(transform.position, hit.transform.position);
                if (dist < closestDistance)
                {
                    closestDistance = dist;
                    closestTarget = hit.gameObject;
                }
            }
        }
        return closestTarget;
    }
}