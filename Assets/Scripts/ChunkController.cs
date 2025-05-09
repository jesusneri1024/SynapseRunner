using UnityEngine;

public class ChunkController : MonoBehaviour
{
    [Header("Obstáculos")]
    public GameObject[] obstaclePrefabs;
    public int numberOfObstacles = 3;

    [Header("Enemigos de suelo")]
    public GameObject[] groundEnemies;

    [Header("Enemigos flotantes")]
    public GameObject[] floatingEnemies;

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

        // Generar enemigo de suelo
        if (Random.value < enemySpawnChance && groundEnemies.Length > 0)
        {
            Vector3 groundPos = new Vector3(Random.Range(-2f, 2f), 0f, Random.Range(1f, 4f));
            GameObject groundEnemy = groundEnemies[Random.Range(0, groundEnemies.Length)];
            Instantiate(groundEnemy, transform.position + groundPos, Quaternion.identity, transform);
        }

        // Generar enemigo flotante
        if (Random.value < enemySpawnChance && floatingEnemies.Length > 0)
        {
            Vector3 floatPos = new Vector3(Random.Range(-2f, 2f), Random.Range(1.5f, 7f), Random.Range(1f, 4f));
            GameObject flyingEnemy = floatingEnemies[Random.Range(0, floatingEnemies.Length)];
            Instantiate(flyingEnemy, transform.position + floatPos, Quaternion.identity, transform);
        }

    }
}
