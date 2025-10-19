using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    public GameObject bullet;
    public Transform  bulletPos;
    private GameObject player;
    private float timer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
      

        float distance = Vector2.Distance(transform.position, player.transform.position);
        Debug.Log(distance);
        if (distance < 4 )
        {
            timer += Time.deltaTime;

            if (timer > 2)
            {

                timer = 0;
                Shoot();
            }
        }

      
    }
    void Shoot()
    {
        Instantiate(bullet, bulletPos.position, Quaternion.identity);
    }   
}
