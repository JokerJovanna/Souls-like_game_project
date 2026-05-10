using UnityEngine;
public class PlayerAttackComponent : MonoBehaviour
{
    [SerializeField] private KeyCode attackKey = KeyCode.Mouse0;
    [SerializeField] private KeyCode alternativeAttackKey = KeyCode.K;

    [SerializeField] private Attack[] attacks;
    [SerializeField] private float cooldown = 0.5f;
    [SerializeField] private float staminaCost = 20f;
    [SerializeField] private float attackAngle = 90f;
    [SerializeField] private float attackActiveDuration = 0.3f;

    private float lastAttackTime = -999f;
    private int lastAttackFrame = -1;
    private int nextAttack;
    private bool isAttacking = false;

    private BlockComponent block;
    private StaminaComponent stamina;

    public AudioClip attackSound;
    private AudioSource audioSource;

    private Animator animator;

    public bool IsAttacking => isAttacking;
    public float AttackAngle => attackAngle;

    private void Start()
    {
        stamina = GetComponent<StaminaComponent>();
        block = GetComponent<BlockComponent>();

        audioSource = GetComponent<AudioSource>();

        animator = GetComponent<Animator>();

        nextAttack = Random.Range(0, attacks.Length);
    }

    private void Update()
    {
        if (block != null && block.IsBlocking) return;
        var attackPressed = Input.GetKeyDown(attackKey) || Input.GetKeyDown(alternativeAttackKey);

        if (attackPressed && !isAttacking && Time.time >= lastAttackTime + cooldown)
        {
            if (Time.frameCount == lastAttackFrame) return;
            lastAttackFrame = Time.frameCount;

            if (!stamina.TrySpendStamina(staminaCost)) return;

            isAttacking = true;
            lastAttackTime = Time.time;

            if (animator != null) animator.SetTrigger("AttackTrigger");

            var attack = attacks[nextAttack];
            var target = FindTarget(attack.AttackDistance);

            if (target != null) attack.Perform(gameObject, target);

            if (audioSource != null && attackSound != null) audioSource.PlayOneShot(attackSound);

            nextAttack = (nextAttack + 1) % attacks.Length;
            Invoke(nameof(ResetAttack), attackActiveDuration);
        }
    }

    private void ResetAttack()
    {
        isAttacking = false;
    }

    public Vector2 GetForwardDirection()
    {
        Vector2 right = transform.right;
        var sr = GetComponent<SpriteRenderer>();
        if (sr != null && sr.flipX) right = -right;
        return right.normalized;
    }

    private GameObject FindTarget(float attackDistance)
    {
        var hitColliders = Physics2D.OverlapCircleAll(transform.position, attackDistance);
        var closestDistance = attackDistance + 1f;
        GameObject closestTarget = null;
        var forward = GetForwardDirection();
        var halfAngle = attackAngle * 0.5f;

        foreach (var hit in hitColliders)
        {
            var health = hit.GetComponent<EnemyHealthComponent>();
            if (health == null) continue;

            Vector2 dirToTarget = (hit.transform.position - transform.position).normalized;
            var angle = Vector2.Angle(forward, dirToTarget);
            if (angle > halfAngle) continue;

            var distance = Vector2.Distance(transform.position, hit.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestTarget = hit.gameObject;
            }
        }
        return closestTarget;
    }
}