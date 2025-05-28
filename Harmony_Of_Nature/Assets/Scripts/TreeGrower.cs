using System.Collections;
using UnityEngine;

public class TreeGrower : MonoBehaviour
{
    [Header("Tree Stages")]
    public GameObject[] treeStages;
    private GameObject currentTreeObject;
    public int currentStage = 0;

    [Header("Growth Settings")]
    public float timeBetweenGrowthStages = 10f; // Час між автостадіями
    public bool enableAutoGrowth = false; // Увімкнути автоматичний ріст
    private Coroutine growthRoutine;
    private bool isGrowing = false;

    private PlayerPickupConsumable player;
    private BoxCollider boxCollider;
    private bool isGrownManuallyOnce = false; // Дерево вирощене гравцем вручну

    void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
        if (boxCollider == null)
        {
            Debug.LogWarning("BoxCollider не призначено на TreeGrower");
        }
    }

    void Start()
    {
        if (treeStages == null || treeStages.Length == 0)
        {
            Debug.LogError("Масив treeStages не заповнено! Перевірте об'єкт у Unity.");
            return;
        }

        ChangeTreeStage(0); // Ініціалізація на першій стадії
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

        if (player.UseItem("Cube"))
        {
            Grow();
            isGrownManuallyOnce = true; // Позначка, що дерево вирощене вручну
            Debug.Log($"Дерево виросло до стадії {currentStage} вручну!");

            StartGrowth(); // Запускаємо автоматичний ріст після першого ручного росту
        }

        else
        {
            Debug.Log("Недостатньо насіння, щоб виростити дерево.");
        }
    }

    public void StartGrowth()
    {
        if (isGrowing)
        {
            Debug.Log("Автоматичне зростання вже запущено.");
            return;
        }

        if (!isGrownManuallyOnce)
        {
            Debug.LogWarning("Автоматичне зростання недозволене без ручного росту.");
            return;
        }

        isGrowing = true; // Встановлюємо стан росту
        growthRoutine = StartCoroutine(AutoGrowTree());
    }


    public void StopGrowth()
    {
        if (growthRoutine != null)
        {
            StopCoroutine(growthRoutine);
        }

        isGrowing = false;
    }

    private IEnumerator AutoGrowTree()
    {
        while (currentStage < treeStages.Length - 1)
        {
            yield return new WaitForSeconds(timeBetweenGrowthStages);
            Grow();
            Debug.Log($"Дерево автоматично виросло до стадії {currentStage}.");
        }

        isGrowing = false; // Ріст завершено
        Debug.Log("Дерево досягло максимальної стадії. Автозростання завершено.");
    }

    private void Grow()
    {
        if (currentStage + 1 < treeStages.Length)
        {
            ChangeTreeStage(++currentStage);
        }
        else
        {
            Debug.Log("Дерево досягло максимальної стадії.");
        }
    }

    private void ChangeTreeStage(int stage)
    {
        // Перевірка дійсності індексу
        if (stage < 0 || stage >= treeStages.Length)
        {
            Debug.LogError($"Спроба доступу до недійсної стадії: {stage}. Максимальний індекс: {treeStages.Length - 1}");
            return;
        }

        // Видаляємо поточний об'єкт, якщо є
        if (currentTreeObject != null)
        {
            Destroy(currentTreeObject);
        }

        // Створюємо новий об'єкт зі списку стадій
        currentTreeObject = Instantiate(treeStages[stage], transform.position, Quaternion.identity, transform);
    }

}