using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private GameObject playerObject;
    [SerializeField] private Vector3 offsetValue;
    [SerializeField] private float smoothValue;

    void Update()
    {
        if(playerObject != null)
        {
            transform.position = Vector3.Lerp(transform.position, playerObject.transform.position + offsetValue, smoothValue);
        }
    }
}
