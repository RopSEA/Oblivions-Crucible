using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemyMovement : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed;
    public GameObject player;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
    }
}
