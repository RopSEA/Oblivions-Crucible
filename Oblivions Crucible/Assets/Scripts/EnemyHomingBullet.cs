using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.EventSystems;

public class EnemyHomingBullet : HomingBullet
{
    private Transform target;
    private Vector3 moveDirection;
    private GameObject enemy;
    public bool noHome;


    void Start()
    {
        if (noHome)
        {
            Destroy(gameObject, lifetime);
            return;
        }

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
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Player");
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
        if (other.CompareTag("Player"))
        {
            HealthSystem health = other.GetComponent<HealthSystem>();
            if (health != null)
            {
                health.TakeDamage(damage, enemy);
            }

            Destroy(gameObject);
        }
    }

    public void updateOwner(GameObject Enemy)
    {
        enemy = Enemy;
    }

    public void updateDir(int deg)
    {
        moveDirection = newDir(deg);
    }

    private Vector3 newDir(int deg)
    {
        Vector3 dirs = new Vector3(0, 1, 0);
        if (deg == 0)
        {
            dirs = new Vector3(0, 1, 0);
        }
        else if (deg == 45)
        {
            dirs = new Vector3(1, 1, 0);
        }
        else if (deg == 90)
        {
            dirs = new Vector3(1, 0, 0);
        }
        else if (deg == 135)
        {
            dirs = new Vector3(1, -1, 0);
        }
        else if (deg == 180)
        {
            dirs = new Vector3(0, -1, 0);
        }
        else if (deg == 225)
        {
            dirs = new Vector3(-1, -1, 0);
        }
        else if (deg == 270)
        {
            dirs = new Vector3(-1, 0, 0);
        }
        else if (deg == 315)
        {
            dirs = new Vector3(-1, 1, 0);
        }
        return dirs.normalized;
    }
}
