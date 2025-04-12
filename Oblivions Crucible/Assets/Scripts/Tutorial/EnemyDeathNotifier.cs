using UnityEngine;

public class EnemyDeathNotifier : MonoBehaviour
{
    public TutorialEnemySpawner spawner;

    void OnDestroy()
    {
        if (spawner != null)
        {
            spawner.OnEnemyKilled();
        }
    }
}
