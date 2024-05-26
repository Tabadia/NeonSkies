using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject bomberPrefab;
    public float spawnFrequency = 10f;
    public float minHeight = 5f;
    public float maxHeight = 10f;
    public GameObject player;
    public float farAwayX = 15f;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(SpawnBombers());
    }

    private IEnumerator SpawnBombers()
    {
        while (true)
        {
            float spawnHeight = player.transform.position.y + Random.Range(minHeight, maxHeight);
            float spawnEdge = player.transform.position.x + (Random.value < 0.5f ? -farAwayX : farAwayX);
            Vector3 spawnPosition = new Vector3(spawnEdge, spawnHeight, 0);
            Vector3 direction = spawnEdge < player.transform.position.x ? Vector3.right : Vector3.left;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            GameObject bomber = Instantiate(bomberPrefab, spawnPosition, Quaternion.Euler(0, 0, -angle));
            bomber.GetComponent<Bomber>().direction = direction;
            yield return new WaitForSeconds(spawnFrequency);
        }
    }
}