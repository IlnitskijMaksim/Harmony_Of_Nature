using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public Terrain terrain; // Террейн для спавна
    public GameObject[] consumablePrefabs; // Префабы для расходников (зелья, предметы)
    public GameObject artifactPrefab; // Префаб артефакта
    public int consumableCount = 10; // Количество расходников
    public int artifactCount = 1; // Количество артефактов (обычно 1)
    
    void Start()
    {
        SpawnConsumables();
        SpawnArtifact();
    }

    void SpawnConsumables()
    {
        TerrainData terrainData = terrain.terrainData;
        Vector3 terrainSize = terrainData.size;

        for (int i = 0; i < consumableCount; i++)
        {
            // Генерация случайной позиции на террейне
            float randomX = Random.Range(0, terrainSize.x);
            float randomZ = Random.Range(0, terrainSize.z);
            float y = terrain.SampleHeight(new Vector3(randomX, 0, randomZ)) + terrain.transform.position.y;

            // Выбор случайного расходника
            GameObject consumablePrefab = consumablePrefabs[Random.Range(0, consumablePrefabs.Length)];

            // Создаем объект
            Instantiate(consumablePrefab, new Vector3(randomX, y, randomZ), Quaternion.identity, terrain.transform);
        }
    }

    void SpawnArtifact()
    {
        TerrainData terrainData = terrain.terrainData;
        Vector3 terrainSize = terrainData.size;

        for (int i = 0; i < artifactCount; i++)
        {
            // Генерация случайной позиции для артефакта на террейне
            float randomX = Random.Range(0, terrainSize.x);
            float randomZ = Random.Range(0, terrainSize.z);
            float y = terrain.SampleHeight(new Vector3(randomX, 0, randomZ)) + terrain.transform.position.y;

            // Создаем артефакт
            Instantiate(artifactPrefab, new Vector3(randomX, y, randomZ), Quaternion.identity, terrain.transform);
        }
    }
}