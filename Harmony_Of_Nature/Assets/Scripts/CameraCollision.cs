using UnityEngine;

public class CameraCollision : MonoBehaviour
{
    public Transform player;
    public float minDistance = 1.0f;
    public float maxDistance = 4.0f;
    public LayerMask collisionMask;
    public float smoothSpeed = 10f;

    private Vector3 cameraDirection;
    private float currentDistance;

    void Start()
    {
        cameraDirection = transform.localPosition.normalized;
        currentDistance = maxDistance;
    }

    void LateUpdate()
    {
        Vector3 desiredCameraPosition = player.position + cameraDirection * maxDistance;

        RaycastHit hit;
        if (Physics.SphereCast(player.position, 0.2f, cameraDirection, out hit, maxDistance, collisionMask))
        {
            currentDistance = Mathf.Clamp(hit.distance, minDistance, maxDistance);
        }
        else
        {
            currentDistance = Mathf.Lerp(currentDistance, maxDistance, Time.deltaTime * smoothSpeed);
        }
        
        transform.localPosition = cameraDirection * currentDistance;
    }
}