using TMPro;
using UnityEngine;

public class PotionComponent : MonoBehaviour
{
    [SerializeField] private KeyCode useKey = KeyCode.H;

    [SerializeField] private int potionCount = 3;
    [SerializeField] private int healAmount = 50;                         

    private PlayerHealthComponent health;

    private AudioSource audioSource;
    [SerializeField] private AudioClip useSound;
    [SerializeField] private AudioClip noPotionSound;

    [SerializeField] private TMP_Text potionCountText;

    public int PotionCount => potionCount;

    void Start()
    {
        health = GetComponent<PlayerHealthComponent>();
        audioSource = GetComponent<AudioSource>();
        if (potionCountText != null)
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
        if (potionCountText != null)
            potionCountText.text = potionCount.ToString();

        if (audioSource != null && useSound != null) audioSource.PlayOneShot(useSound);
    }

    public void AddPotion(int amount)
    {
        potionCount += amount;
    }
}