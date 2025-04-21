using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lazer : MonoBehaviour
{
    GameObject LazerB;
    private HealthSystem healthSystem;
    private GameObject player;
    private GameObject enemy;


    void FindPlayer()
    {
        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj;
            healthSystem = player.GetComponent<HealthSystem>();
        }
        else
        {
            Debug.LogWarning("No player found!");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //Debug.Log("HEY OVER HERE");
            if (enemy != null)
            {
                healthSystem.TakeDamage(15, enemy);
            }
            else
            {
                healthSystem.preCalcTakeDamage(15);
            }
            
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        FindPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
