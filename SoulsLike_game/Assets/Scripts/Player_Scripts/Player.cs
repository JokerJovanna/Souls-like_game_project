using UnityEngine;

public class Player : MonoBehaviour
{
    private MovementComponent movement;
    private JumpComponent jump;
    private DodgeComponent dodge;
    private StaminaComponent stamina;
    private PlayerHealthComponent health;
    private BlockComponent block;

    private Animator animator;

    public float CurrentHealth => health != null ? health.CurrentHealth : 0;
    public float MaxHealth => health != null ? health.MaxHealth : 0;
    public float CurrentStamina => stamina != null ? stamina.CurrentStamina : 0;
    public float MaxStamina => stamina != null ? stamina.MaxStamina : 0;
    public bool IsDodging => dodge != null && dodge.IsDodging;
    public bool IsBlocking => block != null && block.IsBlocking;

    void Awake()
    {
        movement = GetComponent<MovementComponent>();
        jump = GetComponent<JumpComponent>();
        dodge = GetComponent<DodgeComponent>();
        stamina = GetComponent<StaminaComponent>();
        health = GetComponent<PlayerHealthComponent>();
        block = GetComponent<BlockComponent>();

        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (animator == null) return;
        var speedValue = movement != null ? movement.HorizontalSpeed : 0f;

        animator.SetFloat("Speed", speedValue);
        animator.SetBool("IsGrounded", jump != null && jump.IsGrounded);
        animator.SetBool("IsDodging", IsDodging);
    }

    public void DisableCollisionWithEnemies()
    {
        var playerCol = gameObject.GetComponent<Collider2D>();
        var enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var enemy in enemies)
        {
            var enemyCollider = GetEnemyCollider(enemy);
            if (enemyCollider != null && playerCol != null)
            {
                Physics2D.IgnoreCollision(enemyCollider, playerCol, true);
            }
        }
    }

    private Collider2D GetEnemyCollider(GameObject enemy)
    {
        Collider2D enemyCollider = null;
        var colliders = enemy.GetComponents<Collider2D>();
        foreach (var col in colliders)
        {
            if (!col.isTrigger)
            {
                enemyCollider = col;
                break;
            }
        }
        return enemyCollider;
    }
}