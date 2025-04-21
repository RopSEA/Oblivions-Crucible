using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saber : MonoBehaviour
{
    int damage = 25;
    int power = 3;
    private GameObject enemy;

    private void Start()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Check if it's a regular enemy
            HealthSystem player = other.GetComponent<HealthSystem>();
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
            enemy = transform.parent.parent.parent.gameObject;
            if (player != null)
            {
                damage = calcDamage(player.gameObject);
                rb.AddForce(player.GetComponent<PlayerMovement>().dirs * power, ForceMode2D.Impulse);
                player.TakeDamage(damage, enemy);
            }
        }
    }

    public int calcDamage(GameObject player)
    {
        float r = Random.RandomRange(0.85f, 1.5f);
        return (int)Mathf.Ceil(15 * r);
    }
}
