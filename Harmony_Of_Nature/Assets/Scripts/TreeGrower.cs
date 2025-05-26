using UnityEngine;

public class TreeGrower : MonoBehaviour
{
    public Vector3 growthAmount = new Vector3(1f, 1f, 1f);

    private PlayerPickupConsumable player;
    private BoxCollider boxCollider;

    void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
        if (boxCollider == null)
        {
            Debug.LogWarning("BoxCollider �� ��������� �� TreeGrower");
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
            Debug.LogWarning("Player �� ����������� � TreeGrower");
            return;
        }

        if (player.UseItem("Cube"))
        {
            Grow();
            Debug.Log("Tree has grown!");
        }
        else
        {
            Debug.Log("Not enough Cubes to grow the tree.");
        }
    }

    void Grow()
    {
        // �������� ��������� �����
        transform.localScale += growthAmount;
    }
}
