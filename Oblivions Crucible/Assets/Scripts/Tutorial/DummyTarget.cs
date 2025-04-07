using UnityEngine;

public class DummyTarget : MonoBehaviour
{
    public int health = 50;
    private Animator animator;

    private bool isHit = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void damage(int amount)
    {
        if (isHit) return;

        isHit = true;

        health -= amount;

        if (animator != null)
        {
            animator.SetTrigger("Hit"); // This assumes you have a "Hit" trigger in your Animator
        }

        if (health <= 0)
        {
            Die();
        }
        else
        {
            // Allow hit animation again after short delay
            Invoke(nameof(ResetHit), 0.2f);
        }
    }

    void ResetHit()
    {
        isHit = false;
    }

    void Die()
    {
        Destroy(gameObject);
        FindObjectOfType<TutorialManager>()?.OnDummyDestroyed();
    }
}
