using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public GameObject bulletPrefab;
    public GameObject missilePrefab;
    public float bulletSpeed = 20f;
    public GameObject bulletSpawnPoint;
    public int missileCount = 6;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            ShootBullet();
        }
        else if (Input.GetButtonDown("Fire2"))
        {
            StartCoroutine(ShootMissiles());
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

    IEnumerator ShootMissiles()
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