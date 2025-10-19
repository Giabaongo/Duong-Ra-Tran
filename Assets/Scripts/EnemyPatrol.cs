using UnityEngine;
using UnityEngine.InputSystem.XR.Haptics;

public enum EnemyState
{
    Patrol,
    Attack
}


public class EnemyPatrol : MonoBehaviour
{
    public GameObject pointA;
    public GameObject pointB;
    private Rigidbody2D rb;
    private Animator anim;
    private Transform currentPoint;
    public float speed;
       public float attackRange = 4f;
    public GameObject bullet;
    public Transform bulletPos;
    private GameObject player;
    private float timer;
    private EnemyState currentState;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        currentPoint = pointB.transform;
        player = GameObject.FindGameObjectWithTag("Player");
        currentState = EnemyState.Patrol;

    }

    void Update()
    {
        float distance = Vector2.Distance(transform.position, player.transform.position);

        // 🔥 Đổi trạng thái dựa vào khoảng cách
        if (distance < attackRange)
        {
            currentState = EnemyState.Attack;
        }
        else
        {
            currentState = EnemyState.Patrol;
        }

        if (currentState == EnemyState.Patrol)
        {
            Patrol();
        }
        else if (currentState == EnemyState.Attack)
        {
            Attack();
        }
    }

    private void Patrol()
    {
        // Di chuyển
        if (currentPoint == pointB.transform)
            rb.linearVelocity = new Vector2(speed, 0);
        else
            rb.linearVelocity = new Vector2(-speed, 0);

        anim.SetBool("IsRunning", true);

        if (Vector2.Distance(transform.position, currentPoint.position) < 0.5f && currentPoint == pointB.transform)
        {
            Flip();
            currentPoint = pointA.transform;
        }
        if (Vector2.Distance(transform.position, currentPoint.position) < 0.5f && currentPoint == pointA.transform)
        {
            Flip();
            currentPoint = pointB.transform;
        }
    }

    private void Attack()
    {
        // Đứng yên
        rb.linearVelocity = Vector2.zero;
        anim.SetBool("IsRunning", false);

        // Bắn
        timer += Time.deltaTime;
        if (timer > 4)
        {
            timer = 0;
            Shoot();
        }

        // 👉 Quay mặt về phía player (nếu cần)
        if (player.transform.position.x > transform.position.x && transform.localScale.x < 0 ||
            player.transform.position.x < transform.position.x && transform.localScale.x > 0)
        {
            Flip();
        }
    }

    private void Shoot()
    {
        Instantiate(bullet, bulletPos.position, Quaternion.identity);
    }

    private void Flip()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }
}

