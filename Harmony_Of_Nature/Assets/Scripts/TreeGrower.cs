using UnityEngine;

public class TreeGrower : MonoBehaviour
{
    public Vector3 growthAmount = new Vector3(1f, 1f, 1f);

    private PlayerPickupConsumable player; // Используем класс, где обработан инвентарь игрока
    private BoxCollider boxCollider;

    void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
        if (boxCollider == null)
        {
            Debug.LogWarning("BoxCollider не назначен на TreeGrower");
        }
    }

    public void SetPlayer(PlayerPickupConsumable playerScript)
    {
        player = playerScript;
    }

    public void TryGrow()
    {
        if (player == null)
        {
            Debug.LogWarning("Player не назначен в TreeGrower");
            return;
        }

        // Проверка: поднят ли артефакт
        if (!player.GetComponent<PlayerItemPickup>().hasArtifact)
        {
            Debug.Log("You need to have the artifact to grow this tree.");
            return;
        }

        // Если артефакт есть, проверяем расходник
        if (player.UseItem("Cube")) // Замените "Seed" на название вашего расходника
        {
            Grow();
            Debug.Log("Tree has grown!");
        }
        else
        {
            Debug.Log("Not enough Seeds to grow the tree.");
        }
    }

    void Grow()
    {
        // Увеличиваем размеры дерева
        transform.localScale += growthAmount;
    }
}