using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserManager : MonoBehaviour
{
    public GameObject horizontalLaserPrefab;
    public GameObject verticalLaserPrefab;
    public float spawnInterval = 5f;

    void Start()
    {
        InvokeRepeating(nameof(SpawnAllLasers), 2f, spawnInterval);
    }

    void SpawnAllLasers()
    {
    Debug.Log("Spawning laser wave");

    // Spawn full horizontal laser system
    Instantiate(horizontalLaserPrefab, Vector3.zero, Quaternion.identity);

    // Spawn full vertical laser system
    Instantiate(verticalLaserPrefab, Vector3.zero, Quaternion.identity);
    }
}
