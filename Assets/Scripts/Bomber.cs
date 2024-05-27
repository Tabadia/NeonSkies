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

    private Vector3 direction;
    private float initialY;
    private bool swoopingDown = true;
    private float swoopTimer = 0f;
    public float swoopDuration = 2f;
    public float circlingRadius = 5f;
    public float circlingSpeed = 2f;

    private void Start()
    {
        initialY = transform.position.y;
        direction = Vector3.left;
        StartCoroutine(DropProjectiles());
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void FixedUpdate()
    {
        Move();
        FaceDirection();
    }

    private void Move()
    {
        swoopTimer += Time.fixedDeltaTime;
        float offset = Mathf.Sin(swoopTimer * frequency) * amplitude;

        if (player.position.y > transform.position.y)
        {
            Vector3 targetPosition = new Vector3(player.position.x, initialY + offset, 0);
            Vector3 moveDirection = (targetPosition - transform.position).normalized * speed * Time.fixedDeltaTime;
            transform.position += moveDirection;
        }
        else
        {
            if (swoopingDown)
            {
                if (swoopTimer >= swoopDuration)
                {
                    swoopingDown = false;
                    swoopTimer = 0f;
                }
                offset = Mathf.Abs(offset);
            }
            else
            {
                if (swoopTimer >= swoopDuration)
                {
                    swoopingDown = true;
                    swoopTimer = 0f;
                }
                offset = -Mathf.Abs(offset);
            }

            Vector3 targetPosition = new Vector3(player.position.x, initialY + offset, 0);
            Vector3 moveDirection = (targetPosition - transform.position).normalized * speed * Time.fixedDeltaTime;
            transform.position += moveDirection;
        }
    }

    private void FaceDirection()
    {
        Vector3 moveDirection = new Vector3(player.position.x - transform.position.x, player.position.y - transform.position.y, 0).normalized;
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
