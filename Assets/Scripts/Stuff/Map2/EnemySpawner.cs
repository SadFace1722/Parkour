using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance;

    [SerializeField] private List<Transform> spawnPoints;  // Danh sách các vị trí spawn
    [SerializeField] private GameObject enemyPrefab;       // Prefab của quái vật
    [SerializeField] private float spawnCooldown = 5f;     // Thời gian giữa các lần spawn
    [SerializeField] private int maxEnemies = 4;           // Giới hạn số lượng quái vật có thể spawn

    private int enemiesSpawned = 0;                        // Số lượng quái vật đã spawn
    private float lastSpawnTime = 0f;                      // Thời gian lần spawn gần nhất

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Update()
    {
        // Chỉ spawn khi số lượng quái vật hiện tại chưa đạt tới giới hạn và đã đủ thời gian cooldown
        if (enemiesSpawned < maxEnemies && Time.time >= lastSpawnTime + spawnCooldown)
        {
            SpawnEnemy();
            lastSpawnTime = Time.time;
        }
    }

    public void SpawnEnemy()
    {
        // Lựa chọn ngẫu nhiên một điểm spawn từ danh sách spawnPoints
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];

        // Instantiate quái vật tại vị trí spawn và tăng bộ đếm quái vật đã spawn
        Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
        enemiesSpawned++;

        Debug.Log("Quái vật đã được spawn tại " + spawnPoint.position);
    }
}
    