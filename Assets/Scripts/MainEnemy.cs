using System.Collections;
using UnityEngine;

public class MainEnemy : MonoBehaviour
{
    private Transform player;
    public GameObject bulletPrefab;
    public GameObject missilePrefab;
    public float followSpeed = 20f; // Thrust power
    public float maxSpeed = 15f; // Maximum speed
    public float acceleration = 5f; // Acceleration rate
    public float deceleration = 0.5f; // Deceleration rate
    public float airBrakeDeceleration = 10f; // Air brake deceleration rate
    public float rotationSpeed = 1000f; // Rotation speed
    public float rotationSmoothing = 0.1f; // Rotation smoothing factor
    public float bulletFireRate = 2f;
    public float missileFireRate = 5f;
    public float fireDistance = 15f; // The distance within which the enemy will fire at the player
    public float bulletSpeed = 30f; // The speed of the bullet
    public int missileCount = 2; // The number of missiles to fire in each burst
    public Transform bulletSpawnPoint; // The point from which the missiles are spawned
    public float normalGravityScale = 2f; // Normal gravity scale when not thrusting
    public float reducedGravityScale = 0.2f; // Reduced gravity scale when thrusting

    private Rigidbody2D rb;
    private bool isThrusting = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = normalGravityScale; // Set initial gravity scale
        StartCoroutine(FireBullets());
        StartCoroutine(FireMissiles());
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void FixedUpdate()
    {
        if (player != null)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            float distanceToPlayer = Vector2.Distance(player.position, transform.position);

            if (distanceToPlayer > fireDistance)
            {
                // Apply thrust towards the player
                Vector2 thrustDirection = transform.up * followSpeed;
                rb.AddForce(thrustDirection * Time.fixedDeltaTime, ForceMode2D.Force);
                rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxSpeed);
                isThrusting = true;
            }
            else
            {
                // Gradually slow down when not thrusting
                rb.velocity = Vector2.Lerp(rb.velocity, Vector2.zero, deceleration * Time.fixedDeltaTime);
                isThrusting = false;
            }

            // Adjust gravity scale based on whether thrusting or not
            rb.gravityScale = isThrusting ? reducedGravityScale : normalGravityScale;

            // Smoothly rotate the enemy to face the player
            float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            float newRotation = Mathf.LerpAngle(rb.rotation, targetAngle, rotationSmoothing);
            rb.MoveRotation(Mathf.LerpAngle(rb.rotation, newRotation, rotationSpeed * Time.fixedDeltaTime));
        }
    }

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
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, transform.rotation);

        // Get the Rigidbody2D component from the bullet instance
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

        // Set the velocity of the bullet
        rb.velocity = transform.up * bulletSpeed;
    }

    private IEnumerator FireMissiles()
    {
        while (true)
        {
            if (player != null && Vector3.Distance(player.position, transform.position) <= fireDistance)
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
            yield return new WaitForSeconds(missileFireRate);
        }
    }

    void SpawnMissile(int index)
    {
        // Create a new missile instance
        GameObject missile = Instantiate(missilePrefab, bulletSpawnPoint.position, transform.rotation);

        // Rotate the missile around the player in a semicircle
        float angle = index * (180f / (missileCount - 1)) - 90f; // Adjusted to create a semicircle
        missile.transform.RotateAround(transform.position, Vector3.forward, angle);
    }
}
