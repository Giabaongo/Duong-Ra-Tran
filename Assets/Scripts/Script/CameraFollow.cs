using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;   // Player
    public float smoothSpeed = 5f;  // độ mượt khi di chuyển camera
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (target == null)
        {
            // Tự động tìm player nếu chưa assign
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                target = player.transform;
                Debug.Log("Camera found player automatically!");
            }
            else
            {
                Debug.LogError("Camera: No target assigned and Player not found!");
            }
        }
    }

    // LateUpdate được gọi sau Update, tốt hơn cho camera follow
    void LateUpdate()
    {
        if (target == null) return;

        Vector3 targetPosition = new Vector3(target.position.x, target.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
    }
}