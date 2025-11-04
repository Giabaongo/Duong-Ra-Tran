using UnityEngine;

public class PlayerBulletScript : MonoBehaviour
{
    public float force = 5f;          // tốc độ bay
    public float damage = 10f;        // sát thương gây ra
    private Rigidbody2D rb;
    private float timer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Hướng bay theo hướng chuột
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePos - transform.position;
        rb.linearVelocity = direction.normalized * force;

        // Xoay đầu đạn theo hướng bay (tùy sprite)
        float rot = Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rot);
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer > 5f) Destroy(gameObject); // tự hủy sau 5 giây
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Khi chạm Enemy
        if (other.CompareTag("Enemy"))
        {
            EnemyAI enemy = other.GetComponent<EnemyAI>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
            Destroy(gameObject); // hủy viên đạn
        }

      
    }
}
