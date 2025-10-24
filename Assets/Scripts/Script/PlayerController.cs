using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //public bool FacingLeft { get { return facingLeft; } set { facingLeft = value; } }

    public float Movespeed = 2f;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Animator animator;


    public bool FacingLeft = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
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
            // Đảm bảo Rigidbody2D được cấu hình đúng
            rb.gravityScale = 0f; // Tắt gravity cho game top-down
            rb.constraints = RigidbodyConstraints2D.FreezeRotation; // Không cho xoay
            Debug.Log("PlayerController initialized successfully!");
        }

        if (animator == null)
        {
            Debug.LogWarning("Animator không tìm thấy trên Player!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Lấy input từ bàn phím (WASD hoặc Arrow keys)
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        moveInput = new Vector2(horizontal, vertical);

        // Debug để kiểm tra input
        if (moveInput.magnitude > 0)
        {
            Debug.Log($"Move Input: {moveInput} | Velocity: {rb.linearVelocity}");
        }

        // Áp dụng movement cho Rigidbody2D
        if (rb != null)
        {
            rb.linearVelocity = moveInput.normalized * Movespeed;
        }

        // Cập nhật animation
        if (animator != null)
        {
            if (moveInput.magnitude > 0)
            {
                animator.SetBool("IsRunning", true);
            }
            else
            {
                animator.SetBool("IsRunning", false);
            }

            if (Input.GetMouseButtonDown(0))
            {
                animator.SetBool("IsAttacking", true);
            }
            else
            {
                animator.SetBool("IsAttacking", false);
            }
        }

        // Xoay nhân vật theo hướng con trỏ chuột
        RotateTowardsMouse();
    }

    void RotateTowardsMouse()
    {
        // Lấy vị trí con trỏ chuột trong thế giới game
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;

        // Tính hướng từ nhân vật đến con trỏ chuột
        Vector2 direction = (mousePosition - transform.position).normalized;

        // Flip sprite theo hướng trái/phải
        if (direction.x > 0)
        {
            // Quay phải
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            FacingLeft = false;
        }
        else if (direction.x < 0)
        {
            // Quay trái
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            FacingLeft = true;
        }
    }




}
