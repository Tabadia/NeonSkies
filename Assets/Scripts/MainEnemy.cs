using System.Collections;
using UnityEngine;

public class MainEnemy : MonoBehaviour
{
    private Transform player;
    public GameObject bulletPrefab;
    public GameObject missilePrefab;
    public float followSpeed = 5f;
    public float followDistance = 10f;
    public float bulletFireRate = 1f;
    public float missileFireRate = 5f;

    private void Start()
    {
        StartCoroutine(FireBullets());
        StartCoroutine(FireMissiles());
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public float fireDistance = 15f; // The distance within which the enemy will fire at the player

    private void Update()
    {
        if (player != null)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            Vector3 randomOffset = new Vector3(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f), 0);
            direction += randomOffset;

            if (Vector3.Distance(player.position, transform.position) > followDistance)
            {
                transform.position += direction * followSpeed * Time.deltaTime;
            }

            // Rotate the enemy to face the player with a slight random offset
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            transform.rotation = Quaternion.Euler(0, 0, angle + Random.Range(-5f, 5f));
        }
    }

    public float bulletSpeed = 10f; // The speed of the bullet

    private IEnumerator FireBullets()
    {
        while (true)
        {
            if (player != null && Vector3.Distance(player.position, transform.position) <= fireDistance)
            {
            for (int i = 0; i < 3; i++) // Fire three bullets in a burst
                {
                    ShootBullet();
                    yield return new WaitForSeconds(0.07f); // Wait a short time between each bullet in the burst
                }
            }
            yield return new WaitForSeconds(bulletFireRate);
        }
    }

    void ShootBullet()
    {
        // Create a new bullet instance
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.transform.position, transform.rotation);

        // Get the Rigidbody2D component from the bullet instance
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

        // Set the velocity of the bullet
        rb.velocity = transform.up * bulletSpeed;
    }

    public int missileCount = 3; // The number of missiles to fire in each burst
    public Transform bulletSpawnPoint; // The point from which the missiles are spawned

    private IEnumerator FireMissiles()
    {
        int halfCount = missileCount / 2;
        for (int i = 0; i <= halfCount; i++)
        {
            if (i != 0)
            {
                // Spawn a missile to the right of the center
                SpawnMissile(halfCount + i);
                yield return new WaitForSeconds(0.05f);
            }

            // Spawn a missile to the left of the center
            SpawnMissile(halfCount - i);
            yield return new WaitForSeconds(0.05f);
        }
    }

    void SpawnMissile(int index)
    {
        // Create a new missile instance
        GameObject missile = Instantiate(missilePrefab, bulletSpawnPoint.transform.position, transform.rotation);

        // Rotate the missile around the player in a semicircle
        float angle = index * (180f / (missileCount - 1)) - 90f; // Adjusted to create a semicircle
        missile.transform.RotateAround(transform.position, Vector3.forward, angle);
    }
}