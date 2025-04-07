using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingBullet : MonoBehaviour
{
    public float speed = 10f;
    public float lifetime = 5f;
    public int damage = 25;
    private Transform target;
    private Vector2 moveDirection;

    void Start()
    {
        target = FindNearestEnemy();

        if (target != null)
        {
            // calculate direction toward the enemy
            Vector2 direction = (target.position - transform.position).normalized;

            // rotate toward enemy
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle - 90);

            moveDirection = direction;
        }
        else
        {
            // no enemy found, shoot up
            moveDirection = transform.up;
            //moveDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        // destroy bullet after a while
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        // move in a straight line toward enemy
        transform.position += (Vector3)moveDirection * speed * Time.deltaTime;
    }

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

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            // Check if it's a regular enemy
            BasicEnemyMovement enemy = other.GetComponent<BasicEnemyMovement>();
            if (enemy != null)
            {
                enemy.damage(damage); 
            }

            // Check if it's a dummy target
            DummyTarget dummy = other.GetComponent<DummyTarget>();
            if (dummy != null)
            {
                dummy.damage(damage);
            }

            Destroy(gameObject); 
        }
    }
}