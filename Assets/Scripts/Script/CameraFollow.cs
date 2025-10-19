using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;   // Player
    public float smoothSpeed = 5f;  // độ mượt khi di chuyển camera
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null) return;

        Vector3 targetPosition = new Vector3(target.position.x, target.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
    }
}

