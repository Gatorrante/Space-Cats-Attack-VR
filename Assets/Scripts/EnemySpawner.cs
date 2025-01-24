using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public string enemyTag = "Enemy";
    public GameObject enemyPrefab;
    public float spawnInterval = 10f;

    private Transform[] spawnPoints;
    private float timer;

    void Start()
    {
        GameObject[] spawnerObjects = GameObject.FindGameObjectsWithTag("Spawner");
        spawnPoints = new Transform[spawnerObjects.Length];
        for (int i = 0; i < spawnerObjects.Length; i++)
        {
            spawnPoints[i] = spawnerObjects[i].transform;
        }
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnEnemy();
            timer = 0f;
        }
    }

    void SpawnEnemy()
    {
        if (spawnPoints.Length == 0) return;

        int spawnIndex = Random.Range(0, spawnPoints.Length);
        Transform spawnPoint = spawnPoints[spawnIndex];
        GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
        enemy.tag = enemyTag; 
    }
}