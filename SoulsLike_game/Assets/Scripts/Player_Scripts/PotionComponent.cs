using TMPro;
using UnityEngine;

public class PotionComponent : MonoBehaviour
{
    public KeyCode useKey = KeyCode.H;          
    public int potionCount = 3;                 
    public int healAmount = 50;                 

    public AudioClip useSound;                 
    public AudioClip noPotionSound;         

    private PlayerHealthComponent health;
    private AudioSource audioSource;
    private TMP_Text potionCountText;

    public int PotionCount => potionCount;

    void Start()
    {
        health = GetComponent<PlayerHealthComponent>();
        audioSource = GetComponent<AudioSource>();
        potionCountText = GetComponentInChildren<TMP_Text>();
        potionCountText.text = potionCount.ToString();

        if (health == null)
            Debug.LogError("PotionComponent требует компонент PlayerHealthComponent!");
    }

    void Update()
    {
        if (Input.GetKeyDown(useKey))
        {
            TryUsePotion();
        }
    }

    private void TryUsePotion()
    {
        if (potionCount <= 0)
        {
            Debug.Log("Нет зелий!");
            if (audioSource != null && noPotionSound != null)
                audioSource.PlayOneShot(noPotionSound);
            return;
        }

        if (health == null) return;

        if (health.CurrentHealth >= health.MaxHealth)
        {
            Debug.Log("Здоровье уже максимально!");
            return;
        }

        health.Heal(healAmount);
        potionCount--;
        potionCountText.text = potionCount.ToString();

        Debug.Log($"Использовано зелье. Осталось: {potionCount}. HP: {health.CurrentHealth}");

        if (audioSource != null && useSound != null)
            audioSource.PlayOneShot(useSound);
    }

    public void AddPotion(int amount)
    {
        potionCount += amount;
        Debug.Log($"Добавлено {amount} зелий. Теперь: {potionCount}");
    }
}