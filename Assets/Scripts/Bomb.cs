using UnityEngine;

public class Bomb : MonoBehaviour
{
    public float speed = 10f;

    private Rigidbody2D rb;
    private PlayerHealth playerHealth;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = -transform.up * speed;
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (hitInfo.gameObject.CompareTag("Player"))
        {
            Debug.Log("Hit player");
            playerHealth.TakeDamage(20);
            Destroy(gameObject);
        }
    }

    void OnBecameInvisible()
    {
        Invoke("DestroyBullet", 1f);
    }

    void DestroyBullet()
    {
        Destroy(gameObject);
    }
}