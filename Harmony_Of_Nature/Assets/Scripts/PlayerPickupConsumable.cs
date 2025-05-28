using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class PlayerPickupConsumable : MonoBehaviour
{
    [Header("Pickup Settings")]
    public float pickupRange = 2f;
    public LayerMask itemLayer;

    [Header("UI Elements")]
    public TextMeshProUGUI inventoryText;

    private Dictionary<string, int> inventory = new Dictionary<string, int>();

    private TreeGrower currentTreeInRange = null;
    private TreeRestorer currentDryTreeInRange = null;


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            TryPickupItem();
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
        	if (currentTreeInRange != null && currentDryTreeInRange != null)
        	{
            	// Вибираємо об'єкт, найближчий до гравця
            	float distanceToGrower = Vector3.Distance(transform.position, currentTreeInRange.transform.position);
            	float distanceToRestorer = Vector3.Distance(transform.position, currentDryTreeInRange.transform.position);

            	if (distanceToGrower <= distanceToRestorer)
            	{
                	currentTreeInRange.TryGrow();
            	}
            	else
            	{
                	currentDryTreeInRange.TryRestore();
            	}
        	}
        	else if (currentTreeInRange != null)
        	{
            	currentTreeInRange.TryGrow();
        	}
        	else if (currentDryTreeInRange != null)
        	{
            	currentDryTreeInRange.TryRestore();
        	}
        }
        UpdateInventoryUI();
    }

    void TryPickupItem()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, pickupRange, itemLayer);

        if (hits.Length > 0)
        {
            GameObject item = hits[0].gameObject;
            PickupItem(item);
        }
    }

    void PickupItem(GameObject item)
    {
        string itemName = item.name.Replace(" (Clone)", "");

        if (inventory.ContainsKey(itemName))
        {
            inventory[itemName]++;
        }
        else
        {
            inventory[itemName] = 1;
        }

        Debug.Log($"Picked up: {itemName}. Total: {inventory[itemName]}");

        Destroy(item);
    }

    public bool UseItem(string itemName)
    {
        if (inventory.ContainsKey(itemName) && inventory[itemName] > 0)
        {
            inventory[itemName]--;
            if (inventory[itemName] <= 0)
                inventory.Remove(itemName);

            UpdateInventoryUI();

            Debug.Log($"Used 1 {itemName}. Remaining: {(inventory.ContainsKey(itemName) ? inventory[itemName] : 0)}");
            return true;
        }
        else
        {
            Debug.Log($"No {itemName} to use.");
            return false;
        }
    }

    void UpdateInventoryUI()
    {
        if (inventoryText != null)
        {
            inventoryText.text = "";

            foreach (var item in inventory)
            {
                inventoryText.text += $"{item.Key}: {item.Value}\n";
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
		TreeRestorer dryTree = other.GetComponent<TreeRestorer>();
        TreeGrower tree = other.GetComponent<TreeGrower>();
        if (tree != null)
        {
            currentTreeInRange = tree;
            tree.SetPlayer(this);
        }
        if (dryTree != null)
        {
            currentDryTreeInRange = dryTree;
            dryTree.SetPlayer(this);
            Debug.Log("TreeRestorer в зоні досяжності.");
        }

    }

    private void OnTriggerExit(Collider other)
    {
		TreeRestorer dryTree = other.GetComponent<TreeRestorer>();
        TreeGrower tree = other.GetComponent<TreeGrower>();
        if (tree != null && tree == currentTreeInRange)
        {
            currentTreeInRange = null;
            tree.SetPlayer(null);
        }
        if (dryTree != null && dryTree == currentDryTreeInRange)
        {
            Debug.Log("TreeRestorer вийшов із зони досяжності.");
            currentDryTreeInRange = null;
            dryTree.SetPlayer(null);
        }

    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, pickupRange);
    }
}
