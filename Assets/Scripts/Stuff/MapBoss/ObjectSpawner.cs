using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public static ObjectSpawner Instance;

    public int maxSpawnedObjects = 10; // Số lượng đối tượng tối đa
    public GameObject prefab; // Đối tượng mẫu để spawn
    public List<Transform> spawnPoints = new List<Transform>(); // Danh sách vị trí spawn
    private List<GameObject> spawnedObjects = new List<GameObject>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void SpawnObjects()
    {
        // Kiểm tra nếu số lượng đối tượng đã đạt giới hạn
        if (spawnedObjects.Count >= maxSpawnedObjects)
        {
            Debug.Log("Reached max spawned objects limit.");
            return;
        }

        // Kiểm tra danh sách vị trí spawn
        if (spawnPoints == null || spawnPoints.Count == 0)
        {
            Debug.LogError("No spawn points available!");
            return;
        }

        // Chọn ngẫu nhiên một vị trí từ danh sách
        Transform randomSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];

        // Tạo đối tượng tại vị trí ngẫu nhiên
        GameObject newObject = Instantiate(prefab, randomSpawnPoint.position, Quaternion.identity);
        spawnedObjects.Add(newObject);
    }

    public void ClearSpawnedObjects()
    {
        foreach (var obj in spawnedObjects)
        {
            if (obj != null)
            {
                Destroy(obj);
            }
        }
        spawnedObjects.Clear();
    }
}
