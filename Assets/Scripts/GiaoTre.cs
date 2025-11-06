using UnityEngine;

public class GiaoTre : MonoBehaviour
{
    [SerializeField] private GameObject SlashAnimPrefab;
    [SerializeField] private Transform slashAnimSpawnPoint;
    [SerializeField] private Transform weaponCollider;

    private PlayerControls playerControls;
    private PlayerController playerController;
    private Animator myAnimator;

    private GameObject slashAnim;
    private bool isAttacking;

    private Vector3 weaponColliderBaseLocalPos;
    private Vector3 slashSpawnBaseLocalPos;

    private void Awake()
    {
        myAnimator = GetComponent<Animator>();
        if (myAnimator == null)
        {
            Debug.LogError("Animator component không tìm thấy trên " + gameObject.name);
        }

        playerControls = new PlayerControls();
        playerController = GetComponent<PlayerController>();
        if (playerController == null)
        {
            Debug.LogError("PlayerController component không tìm thấy trên " + gameObject.name);
        }
    }

    private void Start()
    {
        if (playerControls != null)
        {
            playerControls.Combat.Attack.started += _ => Attack();
        }

        if (weaponCollider == null)
        {
            Debug.LogError("weaponCollider chưa được assign! Vào Inspector và assign Transform của weapon collider.");
        }
        else
        {
            weaponCollider.gameObject.SetActive(false);
            weaponColliderBaseLocalPos = weaponCollider.localPosition;

            DamageSource damageSource = weaponCollider.GetComponent<DamageSource>();
            if (damageSource == null)
            {
                Debug.LogWarning("weaponCollider không có DamageSource script! Thêm script DamageSource vào weapon collider để gây sát thương.");
            }

            Collider2D collider = weaponCollider.GetComponent<Collider2D>();
            if (collider == null)
            {
                Debug.LogWarning("weaponCollider không có Collider2D! Thêm BoxCollider2D hoặc CircleCollider2D và bật Is Trigger.");
            }
            else if (!collider.isTrigger)
            {
                Debug.LogWarning("weaponCollider's Collider2D chưa bật Is Trigger! Bật Is Trigger trong Inspector.");
            }
        }

        if (slashAnimSpawnPoint != null)
        {
            slashSpawnBaseLocalPos = slashAnimSpawnPoint.localPosition;
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isAttacking)
        {
            Attack();
        }
    }

    private void LateUpdate()
    {
        UpdateFacingAttachments();
    }

    private void OnEnable()
    {
        playerControls?.Enable();
    }

    private void OnDisable()
    {
        if (playerControls != null)
        {
            playerControls.Combat.Attack.started -= _ => Attack();
            playerControls.Disable();
        }

        if (slashAnim != null)
        {
            Destroy(slashAnim);
        }
    }

    private void Attack()
    {
        if (isAttacking) return;

        isAttacking = true;
        Debug.Log("=== ATTACK TRIGGERED ===");

        if (myAnimator != null)
        {
            myAnimator.SetTrigger("Attack");
        }

        if (weaponCollider != null)
        {
            weaponCollider.gameObject.SetActive(true);

            if (weaponCollider.GetComponent<DamageSource>() != null && weaponCollider.GetComponent<Collider2D>() != null)
            {
                Debug.Log("Weapon ready to deal damage!");
            }
        }

        Invoke(nameof(ForceDeactivateWeapon), 0.3f);
    }

    private void ForceDeactivateWeapon()
    {
        if (weaponCollider != null && weaponCollider.gameObject.activeSelf)
        {
            weaponCollider.gameObject.SetActive(false);
        }
        isAttacking = false;
    }

    public void DoneAttackingAnimEvent()
    {
        CancelInvoke(nameof(ForceDeactivateWeapon));

        if (weaponCollider != null)
        {
            weaponCollider.gameObject.SetActive(false);
        }

        isAttacking = false;
    }

    public void SwingUpFlipAnimEvent()
    {
        if (slashAnim != null)
        {
            slashAnim.transform.rotation = Quaternion.Euler(-180, 0, 0);

            if (playerController != null && playerController.FacingLeft)
            {
                slashAnim.GetComponent<SpriteRenderer>().flipX = true;
            }
        }
    }

    public void SwingDownFlipAnimEvent()
    {
        if (slashAnim != null)
        {
            slashAnim.transform.rotation = Quaternion.Euler(0, 0, 0);

            if (playerController != null && playerController.FacingLeft)
            {
                slashAnim.GetComponent<SpriteRenderer>().flipX = true;
            }
        }
    }

    private void UpdateFacingAttachments()
    {
        if (playerController == null)
        {
            return;
        }

        bool facingLeft = playerController.FacingLeft;

        if (weaponCollider != null)
        {
            Vector3 localPos = weaponColliderBaseLocalPos;
            localPos.x = facingLeft ? -Mathf.Abs(localPos.x) : Mathf.Abs(localPos.x);
            weaponCollider.localPosition = localPos;

            Vector3 localScale = weaponCollider.localScale;
            localScale.x = facingLeft ? -Mathf.Abs(localScale.x) : Mathf.Abs(localScale.x);
            weaponCollider.localScale = localScale;
        }

        if (slashAnimSpawnPoint != null)
        {
            Vector3 localPos = slashSpawnBaseLocalPos;
            localPos.x = facingLeft ? -Mathf.Abs(localPos.x) : Mathf.Abs(localPos.x);
            slashAnimSpawnPoint.localPosition = localPos;
        }
    }
}

