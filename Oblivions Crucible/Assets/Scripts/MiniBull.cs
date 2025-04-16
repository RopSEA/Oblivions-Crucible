using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.EventSystems;

public class MiniBull : MonoBehaviour
{
    public float speed = 10f;
    public float lifetime = 5f;
    public int damage = 25;
    public Transform target;
    private GameObject player;
    public GameObject lazerB;
    public Vector2 moveDirection;


    public bool isGo = false;
    public bool isShoot = false;
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
                health.TakeDamage(damage);
            }

            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isGo == true)
        {
            GameObject temp;
            temp = Instantiate(lazerB, transform.position, transform.rotation);

            Destroy(temp, 5f);
            Destroy(gameObject);
            isGo = false;
        }

        if (isShoot == true)
        {
            Debug.Log("GUH");
            transform.position += (Vector3)moveDirection * speed * Time.deltaTime;
            //AudioManager.instance.PlaySfx("lazerSmall");
        }

    }
}
