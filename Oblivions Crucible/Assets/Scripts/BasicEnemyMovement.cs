using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemyMovement : MonoBehaviour
{
    public float speed;
    public int hp;
    public GameObject player;


    public void damage(int dam)
    {

        if (hp - dam > 0)
        {
            hp = hp - dam;
        }
        else
        {
            hp = 0;
        }

        if (hp == 0)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {

    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
    }
}
