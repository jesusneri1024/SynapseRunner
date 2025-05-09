using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChunkGenerator : MonoBehaviour
{
    public GameObject[] chunkPrefabs;
    public GameObject initialChunkPrefab; // El prefab específico para los primeros chunks
    public float chunkLength = 5f;
    public int chunksAhead = 7;
    public float scrollSpeed = 5f;
    public int initialChunksCount = 5; // Número de chunks iniciales específicos

    private Vector3 nextSpawnPoint = new Vector3(0, -2, 0);
    private List<GameObject> activeChunks = new List<GameObject>();
    private int chunksSpawned = 0; // Contador para saber cuántos chunks se han generado

    public int score = -60;
    public int pointsPerChunk = 10; // Puedes ajustar los puntos por chunk

    public TextMeshProUGUI scoreText;


    void Start()
    {
        for (int i = 0; i < chunksAhead; i++)
        {
            SpawnChunk();
        }
    }

    void Update()
    {
        // Mueve los chunks hacia atrás (Z negativo)
        foreach (GameObject chunk in activeChunks)
        {
            chunk.transform.position += Vector3.back * scrollSpeed * Time.deltaTime;
        }

        // Spawn si el último chunk está por acercarse
        if (activeChunks.Count == 0 || activeChunks[activeChunks.Count - 1].transform.position.z < chunkLength * (chunksAhead - 1))
        {
            SpawnChunk();
        }

        // Destruir chunks que ya pasaron mucho al jugador
        for (int i = activeChunks.Count - 1; i >= 0; i--)
        {
            if (activeChunks[i].transform.position.z < -chunkLength * 3f)
            {
                Destroy(activeChunks[i]);
                activeChunks.RemoveAt(i);
            }
        }

        if (scoreText != null)
        {
            scoreText.text = "Puntaje: " + score.ToString();
        }
    }

    void SpawnChunk()
    {
        GameObject prefab;

        scrollSpeed = scrollSpeed * 1.005f;

        // Usar el chunk específico para los primeros N chunks
        if (chunksSpawned < initialChunksCount && initialChunkPrefab != null)
        {
            prefab = initialChunkPrefab;
        }
        else
        {
            // Después usar chunks aleatorios
            prefab = chunkPrefabs[Random.Range(0, chunkPrefabs.Length)];
        }

        Vector3 spawnPosition = Vector3.zero;
        if (activeChunks.Count > 0)
        {
            spawnPosition = activeChunks[activeChunks.Count - 1].transform.position + Vector3.forward * chunkLength;
        }
        else
        {
            spawnPosition = new Vector3(0, -2, 0);
        }

        GameObject newChunk = Instantiate(prefab, spawnPosition, Quaternion.identity);

        // Ejemplo: agregar objetos dentro del chunk si tiene un "ChunkController"
        ChunkController controller = newChunk.GetComponent<ChunkController>();
        if (controller != null)
        {
            controller.GenerateContents();
        }

        activeChunks.Add(newChunk);
        chunksSpawned++; // Incrementar el contador de chunks generados

        score += pointsPerChunk;

    }



}