using System.Collections;
using UnityEngine;

public class FlyIntoPlayer2D : MonoBehaviour
{
    public Transform player;
    public float maxSpeed = 5f;
    public float acceleration = 0.5f; // Increased acceleration
    public float rotationSpeed = 50f;
    public float accuracy = 0.9f;

    private Vector2 targetDirection;
    private float currentSpeed = 0f;

    private void Start()
    {
        StartCoroutine(UpdateTargetDirection());
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if (player != null)
        {
            Vector2 direction2D = (new Vector2(player.position.x, player.position.y) - new Vector2(transform.position.x, transform.position.y)).normalized * accuracy + targetDirection * (1 - accuracy);
            Vector3 direction = new Vector3(direction2D.x, direction2D.y, 0);
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            Quaternion toRotation = Quaternion.Euler(0, 0, angle);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);

            // Accelerate to max speed
            currentSpeed = Mathf.Min(currentSpeed + acceleration * Time.deltaTime, maxSpeed);
            transform.position += transform.up * currentSpeed * Time.deltaTime;
        }
    }

    private IEnumerator UpdateTargetDirection()
    {
        while (true)
        {
            targetDirection = new Vector2(Random.Range(-0.2f, 0.2f), Random.Range(-0.2f, 0.2f)); // Reduced range for random direction
            yield return new WaitForSeconds(1f);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.transform == player)
        {
            Destroy(gameObject);
        }
    }
}