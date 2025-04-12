using UnityEngine;

public class TutorialEnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform[] spawnPoints;
    public int enemiesToSpawn = 5;

    public TutorialManager tutorialManager;

    private int enemiesRemaining;

    public void SpawnWave()
    {
        enemiesRemaining = enemiesToSpawn;

        for (int i = 0; i < enemiesToSpawn; i++)
        {
            Transform spawnPoint = spawnPoints[i % spawnPoints.Length];
            GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);

            EnemyDeathNotifier notifier = enemy.AddComponent<EnemyDeathNotifier>();
            notifier.spawner = this;
        }
    }


    public void OnEnemyKilled()
    {
        enemiesRemaining--;
        if (enemiesRemaining <= 0)
        {
            tutorialManager.OnTutorialWaveComplete();
        }
    }
}
