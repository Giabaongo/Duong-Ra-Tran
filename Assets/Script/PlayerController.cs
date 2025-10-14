using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float Movespeed = 2f;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Animator animator;
    private Camera mainCamera;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        UpdateAnimationState();
    }

    
    void FixedUpdate()
    {
        rb.linearVelocity = moveInput.normalized * Movespeed;

        RotatePlayer();
    }
    void UpdateAnimationState()
    {
        if (moveInput.magnitude>0 )
        {
            animator.SetBool("IsRunning", true);
    
            
        }
        else
        {
            animator.SetBool("IsRunning", false);
        }
    }
    void RotatePlayer()
    {
        Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 lookDirection = mousePosition - rb.position;
        float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg ;
        rb.rotation = angle;
    }

}
