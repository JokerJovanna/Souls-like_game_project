using UnityEngine;
using System;
using UnityEngine.UI;

public class StaminaComponent : MonoBehaviour
{
    public float maxStamina = 100f;
    public float staminaRegenRate = 20f;
    private float currentStamina;
    private Slider staminaBar;

    public event Action<float, float> OnStaminaChanged;
    
    void Start()
    {
        currentStamina = maxStamina;
        var staminaBarObj = GameObject.FindGameObjectWithTag("PlayerStaminaBar");
        staminaBar = staminaBarObj.GetComponentInChildren<Slider>();
    }

    void Update()
    {
        if (currentStamina < maxStamina)
        {
            currentStamina += staminaRegenRate * Time.deltaTime;
            staminaBar.value = currentStamina / maxStamina;
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
            staminaBar.value = currentStamina / maxStamina;
            OnStaminaChanged?.Invoke(currentStamina, maxStamina);
            return true;
        }
        return false;
    }

    public void AddStamina(float amount)
    {
        currentStamina += amount;
        staminaBar.value = currentStamina / maxStamina;
        if (currentStamina > maxStamina) currentStamina = maxStamina;
        OnStaminaChanged?.Invoke(currentStamina, maxStamina);
    }

    public float CurrentStamina => currentStamina;
    public float MaxStamina => maxStamina;
}
