using UnityEngine;

public class DummyTarget : MonoBehaviour
{
    public int health = 50;
    public GameObject coinPrefab;

    private Animator animator;
    private bool isHit = false;
    private TutorialManager tutorialManager;

    [Header("Tutorial Settings")]
    public static int requiredKillsToAdvance = 4;

    [Tooltip("Tracks how many dummies have been destroyed.")]
    public static int totalDummiesKilled = 0;

    void Start()
    {
        animator = GetComponent<Animator>();
        tutorialManager = FindObjectOfType<TutorialManager>();
    }

    public void damage(int amount)
    {
        if (tutorialManager != null && !tutorialManager.HasMoved())
            return;

        if (isHit) return;

        isHit = true;
        health -= amount;

        if (animator != null)
        {
            animator.SetTrigger("Hit");
        }

        if (health <= 0)
        {
            Die();
        }
        else
        {
            Invoke(nameof(ResetHit), 0.2f);
        }
    }

    void ResetHit()
    {
        isHit = false;
    }

    void Die()
    {
        if (coinPrefab != null)
        {
            Instantiate(coinPrefab, transform.position, Quaternion.identity);
        }

        totalDummiesKilled++;

        if (totalDummiesKilled == requiredKillsToAdvance && tutorialManager != null)
        {
            tutorialManager.OnDummyDestroyed();
        }

        Destroy(gameObject);
    }
}
