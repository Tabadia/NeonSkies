using System.Collections;
using UnityEngine;
[System.Serializable]
public class EnemyPlaneSettings
{
    public GameObject prefab;
    public float minSpawnHeight;
    public float maxSpawnHeight;
    public float spawnFrequency;
}

public class EnemySpawner : MonoBehaviour
{
    public EnemyPlaneSettings bomberSettings;
    public EnemyPlaneSettings kamikazeSettings;
    public EnemyPlaneSettings mainEnemySettings;
    public GameObject player;
    public float spawnXDistance = 15f;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(SpawnPlanes(bomberSettings));
        StartCoroutine(SpawnPlanes(kamikazeSettings));
        StartCoroutine(SpawnPlanes(mainEnemySettings));
    }

    private IEnumerator SpawnPlanes(EnemyPlaneSettings settings)
{
    float timePassed = 0f;
    while (true)
    {
        float spawnHeight = player.transform.position.y + Random.Range(settings.minSpawnHeight, settings.maxSpawnHeight);
        float spawnEdge = player.transform.position.x + (Random.value < 0.5f ? -spawnXDistance : spawnXDistance);
        Vector3 spawnPosition = new Vector3(spawnEdge, spawnHeight, 0);
        Vector3 direction = spawnEdge < player.transform.position.x ? Vector3.right : Vector3.left;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        GameObject plane = Instantiate(settings.prefab, spawnPosition, Quaternion.Euler(0, 0, -angle));

        float randomOffset = Random.Range(-0.5f, 0.5f);

        float spawnFrequency = Mathf.Max(settings.spawnFrequency - timePassed / 600f, 1f);

        yield return new WaitForSeconds(spawnFrequency + randomOffset);

        timePassed += spawnFrequency + randomOffset;
    }
}
}