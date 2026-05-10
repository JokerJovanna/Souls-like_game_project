using TMPro;
using UnityEngine;

public class PotionComponent : MonoBehaviour
{
    public KeyCode useKey = KeyCode.H;   
    
    public int potionCount = 3;                 
    public int healAmount = 50;                         

    private PlayerHealthComponent health;

    private AudioSource audioSource;
    public AudioClip useSound;
    public AudioClip noPotionSound;

    private TMP_Text potionCountText;

    public int PotionCount => potionCount;

    void Start()
    {
        health = GetComponent<PlayerHealthComponent>();

        audioSource = GetComponent<AudioSource>();

        potionCountText = GetComponentInChildren<TMP_Text>();

        potionCountText.text = potionCount.ToString();
    }

    void Update()
    {
        if (Input.GetKeyDown(useKey)) TryUsePotion();
    }

    private void TryUsePotion()
    {
        if (potionCount <= 0)
        {
            if (audioSource != null && noPotionSound != null) audioSource.PlayOneShot(noPotionSound);
            return;
        }

        if (health == null) return;

        if (health.CurrentHealth >= health.MaxHealth) return;

        health.Heal(healAmount);
        potionCount--;
        potionCountText.text = potionCount.ToString();

        if (audioSource != null && useSound != null) audioSource.PlayOneShot(useSound);
    }

    public void AddPotion(int amount)
    {
        potionCount += amount;
    }
}