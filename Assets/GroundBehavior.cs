using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundBehavior : MonoBehaviour
{
    public Transform player;
    public Color dangerColor;
    public float boostOutStrength = 1f;
    public GameObject ground;
    public float safeDistance = 50f;

    private Color originalColor;
    private Material groundMat;

    private void Start()
    {
        groundMat = ground.transform.GetComponent<Renderer>().material;
        originalColor = groundMat.GetVector("_EmissionColor");
    }

    // Start is called before the first frame update
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            print("BOOOOOOOST");
            Transform player = collision.transform;
            player.GetComponent<Rigidbody2D>().AddForce(new Vector3(Random.Range(-20, 20), 40 * boostOutStrength, 0), ForceMode2D.Impulse);
            player.GetComponent<Movement>().Stall(2.5f);
            player.GetComponent<PlayerHealth>().TakeDamage(10);
        }
    }

    private void Update()
    {
        float distance = Vector3.Distance(player.position, new Vector3(player.position.x, ground.transform.position.y, 0));
        if (distance < safeDistance)
        {
            groundMat.SetVector("_EmissionColor", Vector4.Lerp(originalColor, dangerColor * 3f, 1 - distance / safeDistance));
        }
    }
}
