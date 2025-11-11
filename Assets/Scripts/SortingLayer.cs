using UnityEngine;

public class SortingLayer : MonoBehaviour
{
    private SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void LateUpdate()
    {
        // Càng thấp trên trục Y => Order càng cao => hiển thị trên
        sr.sortingOrder = Mathf.RoundToInt(-transform.position.y * 100);
    }
}
