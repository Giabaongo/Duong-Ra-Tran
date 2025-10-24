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
    private bool isAttacking = false;

    private void Awake()
    {
        myAnimator = GetComponent<Animator>();

        if (myAnimator == null)
        {
            Debug.LogError("Animator component không tìm thấy trên " + gameObject.name);
        }

        playerControls = new PlayerControls();

        // Lấy PlayerController từ GameObject hiện tại
        playerController = GetComponent<PlayerController>();

        if (playerController == null)
        {
            Debug.LogError("PlayerController component không tìm thấy trên " + gameObject.name);
        }
    }

    private void Update()
    {
        // Hỗ trợ cả Input Manager cũ (mouse click)
        if (Input.GetMouseButtonDown(0) && !isAttacking)
        {
            Attack();
        }
    }

    private void OnEnable()
    {
        if (playerControls != null)
        {
            playerControls.Enable();
        }
    }

    private void OnDisable()
    {
        if (playerControls != null)
        {
            playerControls.Combat.Attack.started -= _ => Attack();
            playerControls.Disable();
        }

        // Cleanup slash animation nếu còn tồn tại
        if (slashAnim != null)
        {
            Destroy(slashAnim);
        }
    }

    private void Start()
    {
        if (playerControls != null)
        {
            playerControls.Combat.Attack.started += _ => Attack();
        }

        // Kiểm tra weapon collider
        if (weaponCollider == null)
        {
            Debug.LogError("weaponCollider chưa được assign! Vào Inspector và assign Transform của weapon collider.");
        }
        else
        {
            // Tắt weapon collider ban đầu
            weaponCollider.gameObject.SetActive(false);

            // Kiểm tra xem weapon collider có DamageSource script không
            DamageSource damageSource = weaponCollider.GetComponent<DamageSource>();
            if (damageSource == null)
            {
                Debug.LogWarning("weaponCollider không có DamageSource script! Thêm script DamageSource vào weapon collider để gây sát thương.");
            }
            else
            {
                Debug.Log("DamageSource script đã được tìm thấy trên weapon collider!");
            }

            // Kiểm tra xem có Collider2D không
            Collider2D collider = weaponCollider.GetComponent<Collider2D>();
            if (collider == null)
            {
                Debug.LogWarning("weaponCollider không có Collider2D! Thêm BoxCollider2D hoặc CircleCollider2D và bật Is Trigger.");
            }
            else
            {
                if (!collider.isTrigger)
                {
                    Debug.LogWarning("weaponCollider's Collider2D không có Is Trigger bật! Bật Is Trigger trong Inspector.");
                }
                else
                {
                    Debug.Log("Weapon Collider setup hoàn tất! Is Trigger = " + collider.isTrigger);
                }
            }
        }
    }

    private void Attack()
    {
        if (isAttacking) return; // Tránh spam attack
        
        isAttacking = true;
        Debug.Log("=== ATTACK TRIGGERED ===");

        if (myAnimator != null)
        {
            myAnimator.SetTrigger("Attack");
            Debug.Log("Attack animation triggered!");
        }

        // Kích hoạt weapon collider để gây sát thương
        if (weaponCollider != null)
        {
            weaponCollider.gameObject.SetActive(true);
            Debug.Log("Weapon Collider activated at position: " + weaponCollider.position);
            
            // Kiểm tra xem có DamageSource và Collider2D không
            if (weaponCollider.GetComponent<DamageSource>() != null && weaponCollider.GetComponent<Collider2D>() != null)
            {
                Debug.Log("Weapon ready to deal damage!");
            }
        }
        else
        {
            Debug.LogError("weaponCollider is NULL! Cannot activate weapon!");
        }

        // Tự động tắt weapon collider sau 0.3 giây nếu animation không gọi DoneAttackingAnimEvent
        Invoke(nameof(ForceDeactivateWeapon), 0.3f);

        // Uncomment nếu bạn muốn dùng slash animation prefab
        //if (SlashAnimPrefab != null && slashAnimSpawnPoint != null)
        //{
        //    slashAnim = Instantiate(SlashAnimPrefab, slashAnimSpawnPoint.position, Quaternion.identity);
        //    slashAnim.transform.parent = this.transform.parent;
        //}
    }

    private void ForceDeactivateWeapon()
    {
        if (weaponCollider != null && weaponCollider.gameObject.activeSelf)
        {
            weaponCollider.gameObject.SetActive(false);
            Debug.Log("Weapon Collider force deactivated (backup timer)!");
        }
        isAttacking = false;
    }

    public void DoneAttackingAnimEvent()
    {
        // Hủy invoke nếu animation đã gọi event này
        CancelInvoke(nameof(ForceDeactivateWeapon));
        
        // Tắt weapon collider sau khi attack xong
        if (weaponCollider != null)
        {
            weaponCollider.gameObject.SetActive(false);
            Debug.Log("Weapon Collider deactivated by animation event!");
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
}
