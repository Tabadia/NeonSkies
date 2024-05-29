using System.Collections;
using UnityEngine;

public class Bomber : MonoBehaviour
{
    public float speed = 5f;
    public GameObject projectilePrefab;
    public float dropFrequency = 1f;
    public float amplitude = 1f;
    public float frequency = 1f;
    public int projectilesPerBurst = 5;
    public float cooldownTime = 5f;
    private Transform player;

    private float direction;
    private float initialY;
    public float swoopDuration = 2f;
    public float circlingRadius = 5f;
    public float circlingSpeed = 2f;

    private void Start()
    {
        initialY = transform.position.y;
        direction = Random.value < 0.5f ? -1f : 1f;
        StartCoroutine(DropProjectiles());
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        float newY = initialY + amplitude * Mathf.Sin(frequency * Time.time);
        Vector3 moveDirection = new Vector3(direction * speed * Time.fixedDeltaTime, newY - transform.position.y, 0);
        transform.position += moveDirection;
        float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90);
    }

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
