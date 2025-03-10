using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ForwardBullet : MonoBehaviour
{
    public Vector3 dir = new Vector3(0, 1, 0);
    public int Speed;

    public void updateDir(int deg)
    {
        dir = newDir(deg);
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

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 5f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Speed * dir * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            BasicEnemyMovement enemy = other.GetComponent<BasicEnemyMovement>();
            if (enemy != null)
            {
                enemy.damage(50);
            }

            Destroy(gameObject);
        }
    }
}
