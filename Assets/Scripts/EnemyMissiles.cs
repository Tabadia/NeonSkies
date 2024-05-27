using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMissiles : MonoBehaviour
{
    public float speed = 5f;
    private Transform target;
    private bool isHoming = false;
    public float despawnTime = 5f;
    public GameObject hitFX;

    // Start is called before the first frame update
    void Start()
    {
        // Find the nearest enemy
        GameObject nearestEnemy = GameObject.FindGameObjectWithTag("Player");

        if (nearestEnemy != null)
        {
            target = nearestEnemy.transform;
        }

        // Start homing after a half second
        Invoke("StartHoming", Random.Range(0.0f, 0.05f));
        Invoke("DestroyBullet", despawnTime);
    }

    public float acceleration = 0.1f; // The rate at which the missile accelerates
    public float rotationAcceleration = 10f; // The rate at which the missile's rotation speed increases

    void Update()
    {
        if (isHoming)
        {
            if (target == null)
            {
                // If there's no target, just move forward
                transform.position += transform.up * speed * Time.deltaTime;
            }
            else
            {
                // If there's a target, gradually change direction towards it
                Vector2 direction = (Vector2)target.position - (Vector2)transform.position + Random.insideUnitCircle * 0.1f;
                direction.Normalize();
                float rotateAmount = Vector3.Cross(direction, transform.up).z;
                GetComponent<Rigidbody2D>().angularVelocity = -rotateAmount * (300f + rotationAcceleration * Time.deltaTime);
                GetComponent<Rigidbody2D>().velocity = transform.up * (speed + acceleration * Time.deltaTime);
            }
        }
        else
        {
            // If not homing yet, just move forward
            transform.position += transform.up * speed * Time.deltaTime;
        }
    }

    void StartHoming()
    {
        isHoming = true;
    }

    GameObject FindNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Player");
        GameObject nearestEnemy = null;
        float minDistance = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position);

            if (distance < minDistance)
            {
                minDistance = distance;
                nearestEnemy = enemy;
            }
        }

        return nearestEnemy;
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (hitInfo.gameObject.CompareTag("Player"))
        {
            Debug.Log("Hit player");
            hitInfo.GetComponent<PlayerHealth>().TakeDamage(20);
            Instantiate(hitFX, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    // void OnBecameInvisible()
    // {
    //     Invoke("DestroyBullet", 1f);
    // }

    void DestroyBullet()
    {
        Destroy(gameObject);
    }
}