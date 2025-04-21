using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    public int dam = 5;


    private bool isTouch = false;
    private IEnumerator curr;


    IEnumerator constantPlayDamage(HealthSystem player)
    {
        while (isTouch == true)
        {
            player.preCalcTakeDamage(dam);
            yield return new WaitForSeconds(0.5f);
        }
    }

    IEnumerator constantEnemDamage(BasicEnemyMovement enem)
    {
        while (true)
        {
            enem.damage(dam);
            yield return new WaitForSeconds(2f);
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            HealthSystem player = collision.GetComponent<HealthSystem>();
            if (player != null)
            {
                isTouch = true;
                StartCoroutine(constantPlayDamage(player));
            }
        }
        if (collision.CompareTag("Enemy"))
        {
            BasicEnemyMovement enemy = collision.GetComponent<BasicEnemyMovement>();
            if (enemy != null)
            {
                enemy.cor = constantEnemDamage(enemy);
                StartCoroutine(enemy.cor);
            }
        }
    }


    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            HealthSystem player = collision.GetComponent<HealthSystem>();
            if (player != null)
            {
                isTouch = false;
                StopCoroutine("constantPlayDamage");
            }
        }
        if (collision.CompareTag("Enemy"))
        {
            BasicEnemyMovement enemy = collision.GetComponent<BasicEnemyMovement>();
            if (enemy != null)
            {
                StopCoroutine(enemy.cor);
            }
        }
    }
}
