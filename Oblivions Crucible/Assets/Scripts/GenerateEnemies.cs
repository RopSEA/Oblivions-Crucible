using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateEnemies : MonoBehaviour
{
    public GameObject Enemy;
    public int xPos;
    public int yPos;
    public int enemyCount;

    void Start()
    {
        StartCoroutine(EnemyDrop());
    }

    IEnumerator EnemyDrop()
    {
        while (enemyCount < 20)
        {
            xPos = Random.Range(-20, 21);
            yPos = Random.Range(-14, 10);
            Instantiate(Enemy, new Vector3(xPos, yPos, -9.97f), Quaternion.identity);
            yield return new WaitForSeconds(0.5f);
            enemyCount += 1;
        }
    }
}

