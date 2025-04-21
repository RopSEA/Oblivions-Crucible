using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.EventSystems;

public class Sheild : MonoBehaviour
{
    private GameObject player;
    public int Power;


    Transform FindNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        Transform nearestEnemy = null;
        float minDistance = Mathf.Infinity;
        Vector3 currentPosition = transform.position;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(currentPosition, enemy.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestEnemy = enemy.transform;
            }
        }

        return nearestEnemy;
    }

    Vector2 findDir()
    {

        Transform temp2 = FindNearestEnemy();

        Vector2 dir;

        if (temp2 != null)
        {
            // calculate direction toward the enemy
            Vector2 direction = (temp2.position - transform.position).normalized;

            // rotate toward enemy
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle - 90);
            dir = direction;
        }
        else
        {
            // no enemy found, shoot up
            dir = transform.up;
            //moveDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        return dir;
    }

    private void Awake()
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
        // Base Dam 25
        int attk = player.GetComponent<Classes>().attack;
        int def = enemy.GetComponent<BasicEnemyMovement>().defense;
        float r = Random.RandomRange(0.5f, 1.5f);
        int dam = (int)Mathf.Ceil((25 * r) * ((attk + 100) / (100 + def)));
        return dam;
    }
}
