using UnityEngine;
using System;

public class StaminaComponent : MonoBehaviour
{
    public float maxStamina = 100f;
    public float staminaRegenRate = 20f;
    private float currentStamina;

    public event Action<float, float> OnStaminaChanged;
    
    void Start()
    {
        currentStamina = maxStamina;
    }

    void Update()
    {
        if (currentStamina < maxStamina)
        {
            currentStamina += staminaRegenRate * Time.deltaTime;
            if (currentStamina > maxStamina) currentStamina = maxStamina;
            OnStaminaChanged?.Invoke(currentStamina, maxStamina);
        }

        //Debug.Log(currentStamina);
    }

    public bool TrySpendStamina(float amount)
    {
        if (currentStamina >= amount)
        {
            currentStamina -= amount;
            OnStaminaChanged?.Invoke(currentStamina, maxStamina);
            return true;
        }
        return false;
    }

    public void AddStamina(float amount)
    {
        currentStamina += amount;
        if (currentStamina > maxStamina) currentStamina = maxStamina;
        OnStaminaChanged?.Invoke(currentStamina, maxStamina);
    }

    public float CurrentStamina => currentStamina;
    public float MaxStamina => maxStamina;
}
