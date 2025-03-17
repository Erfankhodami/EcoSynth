
using UnityEngine;

public class cameraFollow : MonoBehaviour
{
    public Transform playerObject;
    public float smoothSpeed = 5f;
    public Vector3 offset;

    public void FixedUpdate()
    {
        if(playerObject == null) Debug.Log("where is the player??");

        Vector3 targetPostion = playerObject.position + offset;
        transform.position = Vector3.Lerp(transform.position, targetPostion, smoothSpeed * Time.deltaTime);

    }
}
