using UnityEngine;

public class TreeSpawner : MonoBehaviour
{
    public Terrain terrain; // Укажите террейн в инспекторе
    public GameObject[] treePrefabs; // Список префабов деревьев с уже привязанными к ним скриптами
    public int treeCount = 100; // Количество деревьев для спавна

    void Start()
    {
        SpawnTrees();
    }

    void SpawnTrees()
    {
        // Получаем данные террейна
        TerrainData terrainData = terrain.terrainData;

        // Размер террейна
        Vector3 terrainSize = terrainData.size;

        for (int i = 0; i < treeCount; i++)
        {
            // Генерируем случайную позицию на террейне
            float randomX = Random.Range(0, terrainSize.x);
            float randomZ = Random.Range(0, terrainSize.z);

            // Получаем высоту террейна в этой позиции
            float y = terrain.SampleHeight(new Vector3(randomX, 0, randomZ)) + terrain.transform.position.y;

            // Выбираем случайный префаб дерева
            GameObject treePrefab = treePrefabs[Random.Range(0, treePrefabs.Length)];

            // Создаем дерево
            GameObject tree = Instantiate(treePrefab, new Vector3(randomX, y, randomZ), Quaternion.identity);

            // Если нужно настроить скрипты на деревьях, например, передать им параметры
            TreeGrower treeGrower = tree.GetComponent<TreeGrower>();

            // Устанавливаем дерево как дочерний объект для текущего объекта (по желанию)
            tree.transform.parent = terrain.transform;
        }
    }
}