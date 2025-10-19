using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float Movespeed = 2f;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Animator animator;
   

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

    }

 
   

}
