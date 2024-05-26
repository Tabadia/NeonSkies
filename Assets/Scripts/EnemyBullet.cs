using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public GameObject hitFX;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (hitInfo.gameObject.CompareTag("Player"))
        {
            Debug.Log("Hit player");
            hitInfo.GetComponent<PlayerHealth>().TakeDamage(20);
            DestroyBullet();
        }
    }

    void OnBecameInvisible()
    {
        Invoke("DestroyBullet", 1f);
    }

    void DestroyBullet()
    {
        Instantiate(hitFX, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}