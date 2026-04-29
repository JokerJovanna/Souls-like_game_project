using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthComponent : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;

    private Slider healthSlider;
    private SpriteRenderer sprite;
    private float currentHealth;

    private void Awake()
    {
        currentHealth = maxHealth;
        healthSlider = GetComponentInChildren<Slider>();
        sprite = GetComponent<SpriteRenderer>();
    }

    public void TakeDamage(AttackData attack)
    {
        if (currentHealth <= 0) return;
        currentHealth -= attack.Damage;
        StartCoroutine(FlashRed());
        healthSlider.value = currentHealth / maxHealth;
        Debug.Log($"{name} получил {attack.Damage} урона. Осталось {currentHealth} HP.");
        if (currentHealth <= 0)
            Die();
    }

    private System.Collections.IEnumerator FlashRed()
    {
        if (sprite == null) yield break;
        Color originalColor = sprite.color;
        sprite.color = Color.red;
        yield return new WaitForSeconds(0.1f); 
        sprite.color = originalColor;
    }

    private void Die()
    {
        Debug.Log($"{name} умер.");
        Destroy(gameObject);
    }
}
