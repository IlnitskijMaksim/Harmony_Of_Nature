using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class TreeInfester : MonoBehaviour
{
    [Header("Tree Infestation Settings")]
    public GameObject dryTreePrefab; // Префаб сухого дерева
    public string treeTag = "Tree"; // Тег дерев
    public float attackDistance = 1.5f; // Дистанція, на якій можна бити дерево
    public float attackCooldown = 2f; // Затримка між атаками

    private GameObject targetTree; // Поточне дерево для атаки
    private NavMeshAgent agent; // Компонент, щоб керувати рухом
    private bool isAttacking = false; // Чи йде атака зараз

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            Debug.LogError("Компонент NavMeshAgent відсутній на об'єкті!");
        }

        if (dryTreePrefab == null)
        {
            Debug.LogError("Префаб сухого дерева не призначений!");
        }
    }

    void Update()
    {
        if (targetTree == null)
        {
            // Знайти найближче виросле дерево
            LocateNearestTree();
        }
        else
        {
            // Рух до дерева
            MoveToTree();

            // Перевірити, чи можемо атакувати дерево
            float distanceToTree = Vector3.Distance(transform.position, targetTree.transform.position);
            if (distanceToTree <= attackDistance && !isAttacking)
            {
                StartCoroutine(AttackTree());
            }
        }
    }

    private void LocateNearestTree()
    {
        GameObject[] trees = GameObject.FindGameObjectsWithTag(treeTag);
        float minDistance = Mathf.Infinity;

        foreach (var tree in trees)
        {
            TreeGrower treeGrower = tree.GetComponent<TreeGrower>();
            if (treeGrower != null && treeGrower.currentStage == treeGrower.treeStages.Length - 1) // Перевірка, чи дерево в останній стадії
            {
                float distance = Vector3.Distance(transform.position, tree.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    targetTree = tree;
                }
            }
        }
    }

    private void MoveToTree()
    {
        if (targetTree != null && agent != null)
        {
            agent.SetDestination(targetTree.transform.position);
        }
    }

    private IEnumerator AttackTree()
    {
        isAttacking = true;

        // Атака дерева
        Debug.Log($"Атака дерева: {targetTree.name}");
        
        // Видалення дерева
        Transform treePosition = targetTree.transform;
        Destroy(targetTree);

        // Заміна на сухе дерево
        Instantiate(dryTreePrefab, treePosition.position, treePosition.rotation);

        // Затримка між атаками
        yield return new WaitForSeconds(attackCooldown);

        isAttacking = false;

        // Скидання цілі
        targetTree = null; 
    }
}