using Unity.VisualScripting;
using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;
    private float currentHealth;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(AttackData attack)
    {
        if (currentHealth < 0) return;
        currentHealth -= attack.Damage;
        Debug.Log($"{name} получил {attack.Damage} урона. Осталось {currentHealth} HP.");
        if (currentHealth < 0)
            Die();
    }

    private void Die()
    {
        Debug.Log($"{name} умер.");
        Destroy(gameObject);
    }
}
