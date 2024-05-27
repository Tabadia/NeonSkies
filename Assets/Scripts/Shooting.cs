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

    public float bulletCooldown = 0.5f; 
    public float missileCooldown = 2.0f;

    private float lastBulletShotTime;
    private float lastMissileShotTime;

    public AudioSource bulletShootAudio;
    public AudioSource missileShootAudio;

    void Update()
    {
        if (Input.GetButtonDown("Fire1") && Time.time - lastBulletShotTime >= bulletCooldown)
        {
            lastBulletShotTime = Time.time;
            StartCoroutine(ShootBullet());
        }
        else if (Input.GetButtonDown("Fire2") && Time.time - lastMissileShotTime >= missileCooldown)
        {
            lastMissileShotTime = Time.time;
            StartCoroutine(ShootMissiles());
        }
    }

    IEnumerator ShootBullet()
    {
        bulletShootAudio.Play();
        for (int i = 0; i < 3; i++) // Fire three bullets in a burst
        {
            GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.transform.position, transform.rotation);

            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

            Vector2 randomOffset = new Vector2(Random.Range(-0.03f, 0.03f), Random.Range(-0.03f, 0.03f));
            rb.velocity = (transform.up + (Vector3)randomOffset) * bulletSpeed;

            yield return new WaitForSeconds(0.04f);
        }
    }

    IEnumerator ShootMissiles()
    {
        missileShootAudio.Play();
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