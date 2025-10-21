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

    }

    // Update is called once per frame
    void Update()
    {
        moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        rb.linearVelocity = moveInput.normalized * Movespeed;

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

        // Xoay nhân v?t theo h??ng con tr? chu?t
        RotateTowardsMouse();
    }

    void RotateTowardsMouse()
    {
        // L?y v? trí con tr? chu?t trong th? gi?i game
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;

        // Tính h??ng t? nhân v?t ??n con tr? chu?t
        Vector2 direction = (mousePosition - transform.position).normalized;

        // Flip sprite theo h??ng trái/ph?i
        if (direction.x > 0)
        {
            // Quay ph?i
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (direction.x < 0)
        {
            // Quay trái
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }




}
