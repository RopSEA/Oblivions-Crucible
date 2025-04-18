using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserManager : MonoBehaviour
{
    public static LaserManager instance;

    public GameObject horizontalLaserPrefab;
    public GameObject verticalLaserPrefab;
    public float spawnInterval = 5f;
    public int pylonCount = 0;

    private Vector3 h = new Vector3(-8.537104f, -2.552133f, -0.1259285f);
    private Vector3 v = new Vector3(-0.24f, -0.03f, -0.1259285f);

    public void addPylon()
    {
        if (pylonCount == 0)
        {
            InvokeRepeating(nameof(SpawnAllLasers), 2f, spawnInterval);
        }
        pylonCount++;
    }
    public void breakPylon()
    {
        pylonCount = pylonCount - 1;

        if (pylonCount == 0)
        {
            CancelInvoke(nameof(SpawnAllLasers));
        }
    }


    void Awake()
    {

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        //InvokeRepeating(nameof(SpawnAllLasers), 2f, spawnInterval);
    }

    void SpawnAllLasers()
    {
        Debug.Log("Spawning laser wave");

        Instantiate(horizontalLaserPrefab, h, Quaternion.identity);
        Instantiate(verticalLaserPrefab, v, Quaternion.identity);
    }
}
