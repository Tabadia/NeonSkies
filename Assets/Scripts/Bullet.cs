using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
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
        if (hitInfo.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Hit enemy");
            hitInfo.GetComponent<EnemyHealth>().TakeDamage(50);
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