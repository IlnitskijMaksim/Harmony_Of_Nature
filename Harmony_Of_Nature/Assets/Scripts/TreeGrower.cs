using UnityEngine;

public class TreeGrower : MonoBehaviour
{
    public GameObject[] treeStages;
    private int currentStage = 0;

    private PlayerPickupConsumable player;
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

        if (!player.GetComponent<PlayerItemPickup>().hasArtifact)
        {
            Debug.Log("You need to have the artifact to grow this tree.");
            return;
        }

        if (player.UseItem("Cube"))
        {
            Grow();
            Debug.Log($"Tree has grown to stage {currentStage + 1}!");
        }
        else
        {
            Debug.Log("Not enough Seeds to grow the tree.");
        }
    }

    void Grow()
    {
        if (currentStage + 1 < treeStages.Length)
        {
            // Зберігаємо позицію
            Vector3 position = transform.position;

            // Деактивуємо поточну модель
            if (transform.childCount > 0)
            {
                Destroy(transform.GetChild(0).gameObject);
            }

            // Створюємо нову модель як дочірній об'єкт
            currentStage++;
            GameObject newTree = Instantiate(treeStages[currentStage], position, Quaternion.identity);
            newTree.transform.parent = transform;
            newTree.transform.localPosition = Vector3.zero;
            newTree.transform.localPosition = Vector3.zero;
        }
        else
        {
            Debug.Log("Дерево вже на останній стадії.");
        }
    }
}