using UnityEngine;

public class PlayerItemPickup : MonoBehaviour
{
    public float pickupRange = 2f;
    public LayerMask itemLayer;
    public bool hasArtifact = false;


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            TryPickupItem();
        }
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

        // Проверяем, если это артефакт, выставляем hasArtifact = true
        if (itemName == "Cube") // Замените "Artifact" на название вашего артефакта
        {
            hasArtifact = true;
            Debug.Log("Artifact picked up! You can now grow trees.");
        }

        Debug.Log("Picked up: " + itemName);
        Destroy(item);
    }


    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, pickupRange);
    }
}
