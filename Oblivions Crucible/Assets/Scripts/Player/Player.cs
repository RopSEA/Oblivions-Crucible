using UnityEngine;

public class Player : MonoBehaviour
{
    private HealthSystem healthSystem;
    public coinManager coin;

    void Start()
    {
        healthSystem = GetComponent<HealthSystem>();
        if (healthSystem == null)
        {
            Debug.LogError("No HealthSystem component found on Player!");
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Enemy"))
        {
            healthSystem.TakeDamage(25);
        }

        if (collision.collider.CompareTag("Coin"))
        {
            coin.CollectCoin(collision.gameObject);
            Destroy(collision.gameObject);
        }
    }
}
