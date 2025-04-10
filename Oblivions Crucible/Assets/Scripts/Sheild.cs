using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sheild : MonoBehaviour
{
    private GameObject player;
    public int Power;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        int dam;
        if (other.gameObject.CompareTag("Enemy"))
        {
            BasicEnemyMovement enemy = other.gameObject.GetComponent<BasicEnemyMovement>();
            if (enemy != null)
            {
                dam = calcDamage(enemy.gameObject);
                enemy.damage(dam);
                enemy.gameObject.GetComponent<Rigidbody2D>().AddForce(player.GetComponent<PlayerMovement>().dirs * Power, ForceMode2D.Impulse);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("EnemyProj"))
        {
            Destroy(other.gameObject);
        }
    }

    public int calcDamage(GameObject enemy)
    {
        // Base Dam 50
        float r = Random.RandomRange(0.5f, 1.5f);
        int dam = 20 + (int)Mathf.Ceil(player.GetComponent<Classes>().attack * r);
        return dam;
    }
}
