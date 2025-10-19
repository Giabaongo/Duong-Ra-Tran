using UnityEngine;

public class GiaoTre : MonoBehaviour
{
    //[SerializeField] private GameObject SlashAnimPrefab;
    //[SerializeField] private Transform slashAnimSpawnPoint;

    private PlayerControls playerControls;
    private Animator myAnimator;

    //private GameObject slashAnim;

    private void Awake()
    {
        myAnimator = GetComponent<Animator>();
        playerControls = new PlayerControls();
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void Start()
    {
        playerControls.Combat.Attack.started += _ => Attack();
    }

    private void Attack()
    {
        myAnimator.SetTrigger("Attack");

        //slashAnim = Instantiate(SlashAnimPrefab, slashAnimSpawnPoint.position, Quaternion.identity);
        //slashAnim.transform.parent = this.transform.parent;
    }

    public void SwingUpFlipAnimEvent()
    {
        //slashAnim.gameObject.transform.rotation = Quaternion.Euler(-180, 0, 0);

        //if (PlayerController.FacingLeft)
        //{
        //    slashAnim.GetComponent<SpriteRenderer>().flipX = true;
        //}
    }

    public void SwingDownFlipAnimEvent()
    {
        //slashAnim.gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);

        //if (PlayerController.FacingLeft)
        //{
        //    slashAnim.GetComponent<SpriteRenderer>().flipX = true;
        //}
    }
}
