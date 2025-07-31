using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [Tooltip("The different types of enemies to spawn, in order of difficulty based on game level.")]
    [SerializeField] private EnemyData[] enemyTypes;
    [Tooltip("The points where enemies can spawn. If empty, children of this object will be used.")]
    [SerializeField] private Transform[] spawnPoints;
    [Tooltip("The time in seconds between each spawn.")]
    [SerializeField] private float spawnInterval = 2f;

    private float m_timer = 0f;

    private void Awake()
    {
        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            // Get all child transforms, excluding the parent itself.
            spawnPoints = new Transform[transform.childCount];
            for (int i = 0; i < transform.childCount; i++)
            {
                spawnPoints[i] = transform.GetChild(i);
            }
        }
    }

    void Update()
    {
        m_timer += Time.deltaTime;

        if (m_timer > spawnInterval)
        {
            m_timer = 0;
            Spawn();
        }
    }

    private void Spawn()
    {
        if (enemyTypes == null || enemyTypes.Length == 0)
        {
            Debug.LogWarning("No enemy types assigned to the spawner.");
            return;
        }

        // Determine which enemy to spawn based on the game level from GameManager
        int enemyIndex = Mathf.Min(GameManager.Instance.Level, enemyTypes.Length - 1);
        EnemyData enemyToSpawn = enemyTypes[enemyIndex];

        if (enemyToSpawn == null || enemyToSpawn.enemyPrefab == null)
        {
            Debug.LogError($"EnemyData or its prefab is not set for level {GameManager.Instance.Level}.");
            return;
        }

        // Get an enemy instance from the pool using the prefab from EnemyData
        GameObject enemyObj = GameManager.Instance.poolManager.Get(enemyToSpawn.enemyPrefab);
        if (enemyObj == null) return;

        // Assign the EnemyData to the Enemy component
        Enemy enemyScript = enemyObj.GetComponent<Enemy>();
        if (enemyScript != null)
        {
            enemyScript.enemyData = enemyToSpawn;
        }
        else
        {
            Debug.LogError($"The enemy prefab {enemyToSpawn.enemyPrefab.name} is missing the Enemy script.");
            // Release the object back if the script is missing to avoid issues
            GameManager.Instance.poolManager.Release(enemyObj);
            return;
        }

        // Position the enemy at a random spawn point
        if (spawnPoints.Length > 0)
        {
            int spawnPointIndex = Random.Range(0, spawnPoints.Length);
            enemyObj.transform.position = spawnPoints[spawnPointIndex].position;
        }
        else
        {
            Debug.LogWarning("No spawn points are set for the spawner.");
            enemyObj.transform.position = transform.position; // Default to spawner's position
        }
    }
}