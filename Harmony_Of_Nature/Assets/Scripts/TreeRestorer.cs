using UnityEngine;

public class TreeRestorer : MonoBehaviour
{
    [Header("Restoration Settings")]
    public GameObject restoredTreePrefab; // Префаб відновленого дерева
    public string dryTreeTag = "DryTree"; // Тег сухих дерев
    private PlayerPickupConsumable player;

    [Header("Requirements")]
    public string artifactRequired = "Cube"; // Назва артефакта
    public string consumableRequired = "Seed"; // Назва витратного матеріалу

    public void SetPlayer(PlayerPickupConsumable playerScript)
    {
        player = playerScript;
    }

    public void TryRestore()
    {
        if (player == null)
        {
            Debug.LogWarning("Player не призначений в TreeRestorer.");
            return;
        }

        // Перевірка наявності артефакту
        if (!player.GetComponent<PlayerItemPickup>().hasArtifact)
        {
            Debug.Log("Вам потрібен артефакт, щоб відновити це дерево.");
            return;
        }

        // Перевірка наявності витратного матеріалу
        if (player.UseItem(consumableRequired))
        {
            Restore();
            Debug.Log($"Дерево успішно відновлено!");
        }
        else
        {
            Debug.Log("Недостатньо витратного матеріалу для відновлення дерева.");
        }
    }

    private void Restore()
    {
        // Перевірка, що дерево має правильний тег
        if (!CompareTag(dryTreeTag))
        {
            Debug.LogError($"Спроба відновити дерево, яке не є сухим (тег: {tag}).");
            return;
        }

        // Зберігання позиції та орієнтації дерева
        Vector3 position = transform.position;
        Quaternion rotation = transform.rotation;

        // Видалення сухого дерева
        Destroy(gameObject);

        // Створення відновленого дерева
        if (restoredTreePrefab != null)
        {
            Instantiate(restoredTreePrefab, position, rotation);
        }
        else
        {
            Debug.LogError("restoredTreePrefab не призначений!");
        }
    }
}
