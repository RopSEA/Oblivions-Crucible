using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class betterSpawn : MonoBehaviour
{
    public GameObject ene;
    public float waitTime;

    IEnumerator spawnEnemy()
    {
        yield return new WaitForSeconds(waitTime);
        ene.SetActive(true);
        Destroy(this.gameObject);
    }
    public GameObject spawnEnmy(GameObject enemy)
    {
        ene = Instantiate(enemy, transform.position, transform.rotation);
        ene.gameObject.SetActive(false);
        StartCoroutine(spawnEnemy());
        return ene;
    }
}
