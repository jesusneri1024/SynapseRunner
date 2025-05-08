using UnityEngine;

public class ChunkController : MonoBehaviour
{
    [Header("Obstáculos")]
    public GameObject[] obstaclePrefabs;
    public int numberOfObstacles = 3;

    [Header("Enemigos")]
    public GameObject[] enemyPrefabs;
    [Range(0f, 1f)] public float enemySpawnChance = 0.3f; // 30% de probabilidad

    public void GenerateContents()
    {
        // Generar obstáculos
        for (int i = 0; i < numberOfObstacles; i++)
        {
            Vector3 randomPos = new Vector3(Random.Range(-2f, 2f), 0, Random.Range(0f, 5f));
            GameObject obstacle = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];
            Instantiate(obstacle, transform.position + randomPos, Quaternion.identity, transform);
        }

        // Generar enemigo (si se cumple la probabilidad)
        if (Random.value < enemySpawnChance && enemyPrefabs.Length > 0)
        {
            Vector3 enemyPos = new Vector3(Random.Range(-2f, 2f), 0, Random.Range(1f, 4f));
            GameObject enemy = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
            Instantiate(enemy, transform.position + enemyPos, Quaternion.identity, transform);
        }
    }
}
