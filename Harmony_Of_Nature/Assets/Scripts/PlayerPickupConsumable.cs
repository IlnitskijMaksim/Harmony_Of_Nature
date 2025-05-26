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

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            TryPickupItem();
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            if (currentTreeInRange != null)
            {
                currentTreeInRange.TryGrow();
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
        TreeGrower tree = other.GetComponent<TreeGrower>();
        if (tree != null)
        {
            currentTreeInRange = tree;
            tree.SetPlayer(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        TreeGrower tree = other.GetComponent<TreeGrower>();
        if (tree != null && tree == currentTreeInRange)
        {
            currentTreeInRange = null;
            tree.SetPlayer(null);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, pickupRange);
    }
}
