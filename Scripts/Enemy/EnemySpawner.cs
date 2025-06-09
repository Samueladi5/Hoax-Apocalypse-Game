using UnityEngine;
using System.Collections;

public class BattleCitySpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float spawnInterval = 10f;
    [SerializeField] private int maxEnemies = 30;
    
    [Header("Spawn Points")]
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField]
    private GameObject swarmerPrefab;
    [SerializeField]
    private GameObject bigSwarmerPrefab;

    public int spawnCount;

    private int currentEnemyCount = 0;
    private bool isSpawning = false;
    private Coroutine spawnRoutine;

    private void Start()
    {
        if (ValidateSetup())
        {
            StartSpawning();
        }
    }

    private bool ValidateSetup()
    {
        if (enemyPrefab == null)
        {
            Debug.LogError($"[{gameObject.name}] Enemy prefab is not assigned!");
            return false;
        }

        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogError($"[{gameObject.name}] No spawn points assigned!");
            return false;
        }

        // Validasi nilai-nilai numerik
        spawnInterval = Mathf.Max(0.1f, spawnInterval);
        maxEnemies = Mathf.Max(1, maxEnemies);

        return true;
    }

    private void StartSpawning()
    {
        if (!isSpawning && spawnRoutine == null)
        {
            isSpawning = true;
            spawnRoutine = StartCoroutine(SpawnRoutine());
        }
    }

    private void StopSpawning()
    {
        isSpawning = false;
        if (spawnRoutine != null)
        {
            StopCoroutine(spawnRoutine);
            spawnRoutine = null;
        }
    }

    private IEnumerator SpawnRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(spawnInterval);

        while (isSpawning)
        while (true)
        while (true/*spawnCount > 20*/)
        {
            if (currentEnemyCount < maxEnemies)
            {
                SpawnEnemy();
            }
            yield return wait;
        }
    }

    private void SpawnEnemy()
    {
        if (spawnPoints.Length == 0 || enemyPrefab == null) return;

        int spawnIndex = Random.Range(0, spawnPoints.Length);
        Transform spawnPoint = spawnPoints[spawnIndex];

        if (spawnPoint != null)
        {
            GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
            
            // Tambahkan EnemyController ke enemy yang di-spawn
            EnemyController controller = enemy.AddComponent<EnemyController>();
            controller.Initialize(this);
            
            currentEnemyCount++;
            Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
            Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);

            //spawnCount++;
        }
    }

    // Dipanggil oleh EnemyController ketika enemy dihancurkan
    public void OnEnemyDestroyed()
    {
        currentEnemyCount = Mathf.Max(0, currentEnemyCount - 1);
    }

    private void OnDisable()
    {
        StopSpawning();
    }
}

// Script terpisah untuk enemy
public class EnemyController : MonoBehaviour
{
    private BattleCitySpawner spawner;

    public void Initialize(BattleCitySpawner spawnerRef)
    {
        spawner = spawnerRef;
    }

    private void OnDestroy()
    {
        if (spawner != null)
        {
            spawner.OnEnemyDestroyed();
        }
    }
}