using UnityEngine;

public class WolfScript : MonoBehaviour
{
    [SerializeField] private EnemyHealthComponent health;
    [SerializeField] private AttackComponent attack;
    [SerializeField] private PatrolComponent patrol;
    [SerializeField] private PlayerChaserComponent chaser;
    [SerializeField] private PlayerDetectorComponent detector;

    void Awake()
    {
        health = GetComponent<EnemyHealthComponent>();
        attack = GetComponent<AttackComponent>();
        patrol = GetComponent<PatrolComponent>();
        chaser = GetComponent<PlayerChaserComponent>();
        detector = GetComponent<PlayerDetectorComponent>();
        if (health == null) Debug.LogError("HealthComponent missing");
        if (attack == null) Debug.LogError("AttackComponent missing");
        if (patrol == null) Debug.LogError("PatrolComponent missing");
        if (chaser == null) Debug.LogError("PlayerChaserComponent missing");
        if (detector == null) Debug.LogError("PlayerDetectorComponent missing");
    }

    void Start()
    {
        InitializeComponents();
        DisableCollisionWithPlayer();
    }

    void Update()
    {

    }

    public void OnPlayerDetected(GameObject player)
    {
        patrol.enabled = false;
        chaser.SetTarget(player);
        attack.SetTarget(player);
    }

    public void OnPlayerLost(GameObject player)
    {
        patrol.enabled = true;
        chaser.ClearTarget();
        attack.ClearTarget();
    }

    private void OnDestroy()
    {
        if (detector != null)
        {
            detector.OnPlayerDetected -= OnPlayerDetected;
            detector.OnPlayerLost -= OnPlayerLost;
        }
    }

    private void InitializeComponents()
    {
        health.enabled = true;
        attack.enabled = true;
        patrol.enabled = true;
        chaser.enabled = true;
        detector.enabled = true;

        detector.OnPlayerDetected += OnPlayerDetected;
        detector.OnPlayerLost += OnPlayerLost;
    }

    private Collider2D GetEnemyCollider()
    {
        var enemyNormalCollider = GetComponent<Collider2D>();
        if (enemyNormalCollider != null && enemyNormalCollider.isTrigger)
        {
            // Если случайно получили триггер, ищем другой коллайдер
            Collider2D[] colliders = GetComponents<Collider2D>();
            foreach (var col in colliders)
            {
                if (!col.isTrigger)
                {
                    enemyNormalCollider = col;
                    break;
                }
            }
        }
        return enemyNormalCollider;
    }

    private void DisableCollisionWithPlayer()
    {
        var enemyCol = GetEnemyCollider();
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            Collider2D playerCollider = player.GetComponent<Collider2D>();
            if (playerCollider != null && enemyCol != null)
            {
                Physics2D.IgnoreCollision(playerCollider, enemyCol, true);
            }
        }
    }
}
