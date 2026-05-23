using UnityEngine;

public class Player : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Deactivate()
    {
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        animator.Rebind();
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