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

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            TryPickupItem();
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
        string itemName = item.name;
        
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

    void UpdateInventoryUI()
    {
        if (inventoryText != null)
        {
            inventoryText.text = "\n";
            
            foreach (var item in inventory)
            {
                inventoryText.text += $"{item.Key}: {item.Value}\n";
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, pickupRange);
    }
}