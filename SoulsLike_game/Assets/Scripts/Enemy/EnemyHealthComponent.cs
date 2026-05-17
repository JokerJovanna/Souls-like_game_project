using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthComponent : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;

    public System.Action OnDied;

    private Slider healthSlider;
    private SpriteRenderer sprite;
    private EnemySoundComponent sound;
    private float currentHealth;

    private void Awake()
    {
        currentHealth = maxHealth;
        healthSlider = GetComponentInChildren<Slider>();
        sprite = GetComponent<SpriteRenderer>();
        sound = GetComponent<EnemySoundComponent>();
    }

    public void TakeDamage(AttackData attack)
    {
        if (currentHealth <= 0) return;
        currentHealth -= attack.Damage;

        if (sound != null)
            sound.PlayHurt();

        StartCoroutine(FlashRed());

        if (healthSlider != null)
            healthSlider.value = currentHealth / maxHealth;

        Debug.Log($"{name} получил {attack.Damage} урона. Осталось {currentHealth} HP.");
        if (currentHealth <= 0)
            Die();
    }

    private System.Collections.IEnumerator FlashRed()
    {
        if (sprite == null) yield break;
        var originalColor = sprite.color;
        sprite.color = Color.red;
        yield return new WaitForSeconds(0.1f); 
        sprite.color = originalColor;
    }

    private void Die()
    {
        Debug.Log($"{name} умер.");
        OnDied?.Invoke();
        Destroy(gameObject);
    }
}
