using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateSheild : MonoBehaviour
{
    public int damage;
    private GameObject player;

    public int calcDamage(GameObject enemy)
    {
        // Base Dam 25
        int attk = player.GetComponent<Classes>().attack;
        int def = enemy.GetComponent<BasicEnemyMovement>().defense;
        float r = Random.RandomRange(0.5f, 1.5f);
        int dam = (int)Mathf.Ceil((25 * r) * ((attk + 100) / (100 + def)));
        return dam;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            // Check if it's a regular enemy
            BasicEnemyMovement enemy = other.GetComponent<BasicEnemyMovement>();
            if (enemy != null)
            {
                damage = calcDamage(enemy.gameObject);
                enemy.damage(damage);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
