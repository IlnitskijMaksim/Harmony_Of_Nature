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
        	Debug.LogWarning("Player не призначений в TreeGrower.");
        	return;
    	}

    // Перевірка, чи дерево досягло максимальної стадії
    	if (currentStage + 1 >= treeStages.Length)
    	{
        	Debug.Log("Дерево вже на останній стадії.");
        	return;
    	}

    	if (!player.GetComponent<PlayerItemPickup>().hasArtifact)
    	{
        	Debug.Log("Вам потрібен артефакт, щоб виростити це дерево.");
        	return;
    	}

    // Використовуємо насіння лише якщо можемо виростити дерево
    	if (player.UseItem("Cube"))
    	{
        	Grow();
        	Debug.Log($"Tree виросло до стадії {currentStage + 1}!");
    	}
    	else
    	{
        	Debug.Log("Недостатньо насіння, щоб виростити дерево.");
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