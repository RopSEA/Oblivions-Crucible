using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    private float currentHealth; 
    private bool isInvulnerable = false;

    public HealthBar healthBar;
    public delegate void OnDeathDelegate();
    public event OnDeathDelegate OnDeath;
    public RoundManager r;

    public SPUM_MatchingList sprite;
    public Material hit;

    private Coroutine regenCoroutine;

    void Start()
    {
        currentHealth = maxHealth;
        if (healthBar != null)
        {
            healthBar.SetMax(maxHealth);
        }

        List<MatchingElement> sprites = sprite.matchingTables;
        int hitEffectAmount = Shader.PropertyToID("_HitEffectAmount");
        foreach (MatchingElement i in sprites)
        {
            i.renderer.material = hit;
            i.renderer.material.SetFloat(hitEffectAmount, 1);
        }

        // Start constant regen: 0.7 HP per second
        StartConstantRegen(0.7f, 1f);
    }

    public void addHealth(int hp)
    {
        if (hp < 0)
        {
            return;
        }
        maxHealth += hp;
        currentHealth = maxHealth;

        if (healthBar != null)
        {
            healthBar.SetHealth(Mathf.CeilToInt(currentHealth));
        }
    }

    public void TakeDamage(int damage)
    {
        if (isInvulnerable) return;

        StartCoroutine(tempInvul());

        float def = gameObject.GetComponent<Classes>().defense;
        int dam = (int)Mathf.Ceil(damage * (10 / def));

        currentHealth -= dam;
        if (currentHealth < 0) currentHealth = 0;

        Debug.Log($"Player took {dam} damage! Health: {currentHealth}");

        if (healthBar != null)
        {
            healthBar.SetHealth(Mathf.CeilToInt(currentHealth));
            AudioManager.instance.PlaySfx("hit");
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    IEnumerator tempInvul()
    {
        List<MatchingElement> sprites = sprite.matchingTables;
        float dur = 0.5f;
        float elapsedTime = 0f;
        int hitEffectAmount = Shader.PropertyToID("_HitEffectAmount");
        isInvulnerable = true;

        while (elapsedTime < dur)
        {
             elapsedTime += Time.deltaTime;

             float lerpedAmt = Mathf.Lerp(1f, 0f, (elapsedTime / dur));
             foreach (MatchingElement i in sprites)
             {
               i.renderer.material.SetFloat(hitEffectAmount, lerpedAmt);
             }
             yield return null;
        }

        elapsedTime = 0f;

        while (elapsedTime < dur)
        {
            elapsedTime += Time.deltaTime;

            float lerpedAmt = Mathf.Lerp(0f, 1f, (elapsedTime / dur));
            foreach (MatchingElement i in sprites)
            {
                 i.renderer.material.SetFloat(hitEffectAmount, lerpedAmt);
            }
             yield return null;

        }
        isInvulnerable = false;
    }

    public void Heal(float amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth) currentHealth = maxHealth;

        if (healthBar != null)
        {
            healthBar.SetHealth(Mathf.CeilToInt(currentHealth));
        }
    }

    public void SlowlyHeal(float totalAmount, float duration)
    {
        StartCoroutine(SlowHealCoroutine(totalAmount, duration));
    }

    private IEnumerator SlowHealCoroutine(float totalAmount, float duration)
    {
        float amountHealed = 0f;
        float interval = 0.1f;
        int ticks = Mathf.CeilToInt(duration / interval);
        float amountPerTick = totalAmount / ticks;

        while (amountHealed < totalAmount && currentHealth < maxHealth)
        {
            float healThisTick = Mathf.Min(amountPerTick, totalAmount - amountHealed);
            Heal(healThisTick);
            amountHealed += healThisTick;
            yield return new WaitForSeconds(interval);
        }
    }

    public void StartConstantRegen(float amountPerTick, float interval)
    {
        if (regenCoroutine != null)
            StopCoroutine(regenCoroutine);

        regenCoroutine = StartCoroutine(ConstantRegenCoroutine(amountPerTick, interval));
    }

    public void StopRegen()
    {
        if (regenCoroutine != null)
        {
            StopCoroutine(regenCoroutine);
            regenCoroutine = null;
        }
    }

    private IEnumerator ConstantRegenCoroutine(float amountPerTick, float interval)
    {
        while (true)
        {
            if (currentHealth < maxHealth)
            {
                Heal(amountPerTick);
            }
            yield return new WaitForSeconds(interval);
        }
    }

    private void Die()
    {
        Debug.Log("Player Died!");
        if (r != null)
        {
            r.Lose();
        }
        

        if (DataPersistenceManager.instance != null && DataPersistenceManager.instance.GameData != null)
        {
            DataPersistenceManager.instance.GameData.deathCount++;
            DataPersistenceManager.instance.SaveGame();
            Debug.Log("Deaths: " + DataPersistenceManager.instance.GameData.deathCount);
        }
        else
        {
            Debug.LogError("DataPersistenceManager or gameData is null! Cannot increment death count.");
        }

        OnDeath?.Invoke();
    }

    public void SetInvulnerable(bool value)
    {
        isInvulnerable = value;
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }
}
