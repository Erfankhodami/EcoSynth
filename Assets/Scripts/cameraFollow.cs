using UnityEngine;

public class cameraFollow : MonoBehaviour
{
    public Transform playerObject;
    public float smoothSpeed = 5f;
    public Vector3 offset = new Vector3(0, 0, -10); // Ensures the camera stays behind

    public bool canFollow = true;

    private void LateUpdate() // Changed from FixedUpdate to LateUpdate
    {
        if (playerObject == null)
        {
            Debug.LogError("Camera Follow Error: Player object is missing!");
            return;
        }

        if (canFollow)
        {
            Vector3 targetPosition = playerObject.position + offset;
            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
        }
    }
}