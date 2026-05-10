using UnityEngine;
using System;
using UnityEngine.UI;

public class StaminaComponent : MonoBehaviour
{
    [SerializeField] private float maxStamina = 100f;
    [SerializeField] private float staminaRegenRate = 20f;
    public event Action<float, float> OnStaminaChanged;

    private float currentStamina;

    [SerializeField] private Slider staminaBar;

    public float CurrentStamina => currentStamina;
    public float MaxStamina => maxStamina;

    void Start()
    {
        currentStamina = maxStamina;
    }

    void Update()
    {
        if (currentStamina < maxStamina)
        {
            currentStamina += staminaRegenRate * Time.deltaTime;
            if (staminaBar != null) staminaBar.value = currentStamina / maxStamina;
            if (currentStamina > maxStamina) currentStamina = maxStamina;
            OnStaminaChanged?.Invoke(currentStamina, maxStamina);
        }
    }

    public bool TrySpendStamina(float amount)
    {
        if (currentStamina >= amount)
        {
            currentStamina -= amount;
            if (staminaBar != null) staminaBar.value = currentStamina / maxStamina;
            OnStaminaChanged?.Invoke(currentStamina, maxStamina);
            return true;
        }
        return false;
    }

    public void AddStamina(float amount)
    {
        currentStamina += amount;
        if (staminaBar != null) staminaBar.value = currentStamina / maxStamina;
        if (currentStamina > maxStamina) currentStamina = maxStamina;
        OnStaminaChanged?.Invoke(currentStamina, maxStamina);
    }
}
