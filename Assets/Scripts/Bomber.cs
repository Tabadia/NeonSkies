using System.Collections;
using UnityEngine;

public class Bomber : MonoBehaviour
{
    public float speed = 5f;
    public GameObject projectilePrefab;
    public float dropFrequency = 1f;
    public Vector3 direction;
    public float amplitude = 1f;
    public float frequency = 1f;

    private float initialY;

    private void Start()
    {
        initialY = transform.position.y;
        StartCoroutine(DropProjectiles());
    }

    private void Update()
    {
        float newY = initialY + amplitude * Mathf.Sin(frequency * Time.time);
        Vector3 newPosition = new Vector3(transform.position.x + direction.x * speed * Time.deltaTime, newY, transform.position.z);

        // Calculate the angle in the direction of movement
        Vector3 directionToNewPosition = newPosition - transform.position;
        float angle = Mathf.Atan2(directionToNewPosition.y, directionToNewPosition.x) * Mathf.Rad2Deg;

        // Rotate the bomber to face the direction of movement
        transform.rotation = Quaternion.Euler(0, 0, angle - 90);

        // Update the position after calculating the rotation
        transform.position = newPosition;
    }

    public int projectilesPerBurst = 5; // Number of projectiles in each burst
    public float cooldownTime = 5f; // Cooldown time between bursts

    private IEnumerator DropProjectiles()
    {
        while (true)
        {
            for (int i = 0; i < projectilesPerBurst; i++)
            {
                Instantiate(projectilePrefab, transform.position, Quaternion.identity);
                yield return new WaitForSeconds(dropFrequency);
            }
            yield return new WaitForSeconds(cooldownTime);
        }
    }
}