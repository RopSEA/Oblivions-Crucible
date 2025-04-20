using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class StaminaBar : MonoBehaviour
{
    public UnityEngine.UI.Slider staminaBar;

    private int maxStamina = 100;
    private int currentStamina;

    private WaitForSeconds regenTick = new WaitForSeconds(0.1f);
    private Coroutine regen;


    void Start()
    {
        currentStamina = maxStamina;
        staminaBar.maxValue = maxStamina;
        staminaBar.value = maxStamina;
    }

    public void SetMax(int stam)
    {
        staminaBar.maxValue += stam;
        staminaBar.value = staminaBar.maxValue;
    }

    public int UseStamina(int amount)
    {
        if (currentStamina - amount >= 0)
        {
            currentStamina -= amount;
            staminaBar.value = currentStamina;

            if (regen != null)
            {
                StopCoroutine(regen);
            }

            regen = StartCoroutine(Regen());
            return 1;
        }
        else
        {
            Debug.Log("not enough stamina");
            return -1;
        }
    }

    private IEnumerator Regen()
    {
        yield return new WaitForSeconds(1.2f);

        while (currentStamina < maxStamina)
        {
            currentStamina += maxStamina / 100;
            staminaBar.value = currentStamina;
            yield return regenTick;
        }
        regen = null;
    }
}
