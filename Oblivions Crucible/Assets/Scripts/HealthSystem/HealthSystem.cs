using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;
    private bool isInvulnerable = false;

    public HealthBar healthBar; 
    public delegate void OnDeathDelegate(); // Event to trigger on death
    public event OnDeathDelegate OnDeath; // Subscribe other scripts to this

    void Start()
    {
        currentHealth = maxHealth;
        if (healthBar != null)
        {
            healthBar.SetMax(maxHealth);
        }
    }

    public void TakeDamage(int damage)
    {
        if (isInvulnerable) return; // Ignore damage if invulnerable

        currentHealth -= damage;
        if (currentHealth < 0) currentHealth = 0;

        Debug.Log($"Player took {damage} damage! Health: {currentHealth}");

        if (healthBar != null)
        {
            healthBar.SetHealth(currentHealth); // Update UI
        }

        if (currentHealth == 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth) currentHealth = maxHealth;

        if (healthBar != null)
        {
            healthBar.SetHealth(currentHealth);
        }
    }

    private void Die()
    {
        Debug.Log("Player Died!");

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

    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }
}
