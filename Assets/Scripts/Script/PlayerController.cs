// 04/11/2025 AI-Tag
// This was created with the help of Assistant, a Unity Artificial Intelligence product.

using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float Movespeed = 2f;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Animator animator;
    private float baseScaleX;

    public bool FacingLeft { get; private set; }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        if (rb == null)
        {
            Debug.LogError("PlayerController: Rigidbody2D not found on Player.");
        }
        else
        {
            rb.gravityScale = 0f;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            Debug.Log("PlayerController initialized successfully!");
        }

        if (animator == null)
        {
            Debug.LogWarning("PlayerController: Animator not found on Player.");
        }

        baseScaleX = Mathf.Abs(transform.localScale.x);
    }

    private void Update()
    {
        Vector2 rawInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        rawInput = Vector2.ClampMagnitude(rawInput, 1f);

        moveInput = ConvertInputToWorld(rawInput);

        if (moveInput.sqrMagnitude > 0f)
        {
            Vector2 currentVelocity = rb != null ? rb.linearVelocity : Vector2.zero;
            Debug.Log($"Move Input (world): {moveInput} | Velocity: {currentVelocity}");
        }

        if (rb != null)
        {
            Vector2 appliedVelocity = moveInput.normalized * Movespeed;
            rb.linearVelocity = appliedVelocity;
        }

        if (animator != null)
        {
            bool isRunning = moveInput.sqrMagnitude > 0f;
            animator.SetBool("IsRunning", isRunning);
            animator.SetBool("IsAttacking", Input.GetMouseButtonDown(0));
        }

        UpdateFacing(rawInput);
    }

    private Vector2 ConvertInputToWorld(Vector2 input)
    {
        Camera cam = Camera.main;
        if (cam == null)
        {
            return input;
        }

        Vector3 camRight = cam.transform.right;
        Vector3 camUp = cam.transform.up;

        Vector3 projected = camRight * input.x + camUp * input.y;
        return new Vector2(projected.x, projected.y);
    }

    private void UpdateFacing(Vector2 rawInput)
    {
        if (Mathf.Abs(rawInput.x) > 0.01f)
        {
            SetFacingLeft(rawInput.x < 0f);
        }
        else
        {
            RotateTowardsMouse();
        }
    }

    private void RotateTowardsMouse()
    {
        Camera cam = Camera.main;
        if (cam == null)
        {
            return;
        }

        Vector3 mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;

        Vector2 direction = (mousePosition - transform.position).normalized;

        if (direction.x > 0f)
        {
            SetFacingLeft(false);
        }
        else if (direction.x < 0f)
        {
            SetFacingLeft(true);
        }
    }

    private void SetFacingLeft(bool left)
    {
        FacingLeft = left;
        float newScaleX = FacingLeft ? -baseScaleX : baseScaleX;
        Vector3 localScale = transform.localScale;
        localScale.x = newScaleX;
        transform.localScale = localScale;
    }

    public void FaceTowardsWorld(Vector3 worldPosition)
    {
        Vector2 direction = worldPosition - transform.position;
        if (Mathf.Abs(direction.x) > 0.01f)
        {
            SetFacingLeft(direction.x < 0f);
        }
    }
}
