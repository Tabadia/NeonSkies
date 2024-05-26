using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject bomberPrefab;
    public float spawnFrequency = 10f;
    public GameObject flyingToPlayerPrefab;
    public float flyingToPlayerSpawnFrequency = 5f;
    public float minBHeight = 5f;
    public float maxBHeight = 10f;
    public float minFHeight = -10f;
    public float maxFHeight = 10f;
    public GameObject player;
    public float spawnXDistance = 15f;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(SpawnBombers());
        StartCoroutine(SpawnFlyingToPlayerPlanes());
    }

    private IEnumerator SpawnBombers()
    {
        while (true)
        {
            float spawnHeight = player.transform.position.y + Random.Range(minBHeight, maxBHeight);
            float spawnEdge = player.transform.position.x + (Random.value < 0.5f ? -spawnXDistance : spawnXDistance);
            Vector3 spawnPosition = new Vector3(spawnEdge, spawnHeight, 0);
            Vector3 direction = spawnEdge < player.transform.position.x ? Vector3.right : Vector3.left;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            GameObject bomber = Instantiate(bomberPrefab, spawnPosition, Quaternion.Euler(0, 0, -angle));
            bomber.GetComponent<Bomber>().direction = direction;
            yield return new WaitForSeconds(spawnFrequency);
        }
    }

    private IEnumerator SpawnFlyingToPlayerPlanes()
    {
        while (true)
        {
            float spawnHeight = player.transform.position.y + Random.Range(minFHeight, maxFHeight);
            float spawnEdge = player.transform.position.x + (Random.value < 0.5f ? -spawnXDistance : spawnXDistance);
            Vector3 spawnPosition = new Vector3(spawnEdge, spawnHeight, 0);
            Vector3 direction = spawnEdge < player.transform.position.x ? Vector3.right : Vector3.left;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            GameObject flyingToPlayerPlane = Instantiate(flyingToPlayerPrefab, spawnPosition, Quaternion.Euler(0, 0, -angle));
            yield return new WaitForSeconds(flyingToPlayerSpawnFrequency);
        }
    }
}