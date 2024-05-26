using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject bomberPrefab;
    public float spawnFrequency = 10f;
    public float minHeight = 5f;
    public float maxHeight = 10f;

    private void Start()
    {
        StartCoroutine(SpawnBombers());
    }

    private IEnumerator SpawnBombers()
{
    while (true)
    {
        float spawnHeight = Random.Range(Screen.height * 1 / 2, Screen.height * 11/12);
        float spawnEdge = Random.value < 0.5f ? 0 : Screen.width;
        Vector3 spawnPosition = Camera.main.ScreenToWorldPoint(new Vector3(spawnEdge, spawnHeight, Camera.main.nearClipPlane));
        GameObject bomber = Instantiate(bomberPrefab, spawnPosition, Quaternion.identity);
        bomber.GetComponent<Bomber>().direction = spawnEdge == 0 ? Vector3.right : Vector3.left;
        yield return new WaitForSeconds(spawnFrequency);
    }
}
}