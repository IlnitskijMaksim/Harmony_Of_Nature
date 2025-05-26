using UnityEngine;

public class PlayerItemPickup : MonoBehaviour
{
    public float pickupRange = 2f;
    public LayerMask itemLayer;

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
        Debug.Log("Picked up: " + item.name);
        Destroy(item);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, pickupRange);
    }
}
