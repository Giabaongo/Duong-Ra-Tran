// 04/11/2025 AI-Tag
// This was created with the help of Assistant, a Unity Artificial Intelligence product.

using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float Movespeed = 2f;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Animator animator;

    public bool FacingLeft = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        if (rb == null)
        {
            Debug.LogError("Rigidbody2D không tìm thấy trên Player! Thêm Rigidbody2D component vào Player GameObject.");
        }
        else
        {
            rb.gravityScale = 0f; // Tắt gravity cho game top-down
            rb.constraints = RigidbodyConstraints2D.FreezeRotation; // Không cho xoay
            Debug.Log("PlayerController initialized successfully!");
        }

        if (animator == null)
        {
            Debug.LogWarning("Animator không tìm thấy trên Player!");
        }
    }

    void Update()
    {
        // Lấy input từ bàn phím (WASD hoặc Arrow keys)
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        moveInput = new Vector2(horizontal, vertical);

        // Áp dụng movement cho Rigidbody2D
        if (rb != null)
        {
            rb.linearVelocity = moveInput.normalized * Movespeed;
        }

        // Cập nhật animation
        UpdateAnimation();

        // Xoay nhân vật theo hướng di chuyển hoặc con trỏ chuột
        AdjustFacingDirection();
    }

    private void UpdateAnimation()
    {
        if (animator != null)
        {
            animator.SetBool("IsRunning", moveInput.magnitude > 0);

            if (Input.GetMouseButtonDown(0))
            {
                animator.SetBool("IsAttacking", true);
                RotateTowardsMouse(); // Xoay nhân vật theo hướng chuột khi tấn công
            }
            else
            {
                animator.SetBool("IsAttacking", false);
            }
        }
    }

    private void AdjustFacingDirection()
    {
        if (moveInput.magnitude > 0)
        {
            // Quay mặt theo hướng phím di chuyển
            if (moveInput.x > 0)
            {
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                FacingLeft = false;
            }
            else if (moveInput.x < 0)
            {
                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                FacingLeft = true;
            }
        }
        else
        {
            // Nếu không di chuyển, quay mặt theo hướng chuột
            RotateTowardsMouse();
        }
    }

    private void RotateTowardsMouse()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;

        Vector2 direction = (mousePosition - transform.position).normalized;

        if (direction.x > 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            FacingLeft = false;
        }
        else if (direction.x < 0)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            FacingLeft = true;
        }
    }
}