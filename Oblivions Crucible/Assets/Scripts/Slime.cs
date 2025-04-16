using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerMovement player = collision.GetComponent<PlayerMovement>();
            if (player != null)
            {
                player.currentSpeed = player.currentSpeed / 2;
            }
        }
        if (collision.CompareTag("Enemy"))
        {
            BasicEnemyMovement enemy = collision.GetComponent<BasicEnemyMovement>();
            if (enemy != null)
            {
                enemy.speed = enemy.speed / 2 ;
            }
        }
    }


    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerMovement player = collision.GetComponent<PlayerMovement>();
            if (player != null)
            {
                player.currentSpeed = player.BASE_SPEED;
            }
        }
        if (collision.CompareTag("Enemy"))
        {
            BasicEnemyMovement enemy = collision.GetComponent<BasicEnemyMovement>();
            if (enemy != null)
            {
                enemy.speed = enemy.speed * 2;
            }
        }
    }
}
