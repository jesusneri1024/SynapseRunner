using System.Collections.Generic;
using UnityEngine;

public class ChunkGenerator : MonoBehaviour
{
    public GameObject[] chunkPrefabs;
    public float chunkLength = 5f;
    public int chunksAhead = 7;
    public float scrollSpeed = 5f;

    private Vector3 nextSpawnPoint = new Vector3(0, -2, 0);
    private List<GameObject> activeChunks = new List<GameObject>();

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
    }

    void SpawnChunk()
    {
        GameObject prefab = chunkPrefabs[Random.Range(0, chunkPrefabs.Length)];

        Vector3 spawnPosition = Vector3.zero;
        if (activeChunks.Count > 0)
        {
            spawnPosition = activeChunks[activeChunks.Count - 1].transform.position + Vector3.forward * chunkLength;
        }
        else
        {
            spawnPosition = new Vector3(0, -2, 0); // posición inicial
        }

        GameObject newChunk = Instantiate(prefab, spawnPosition, Quaternion.identity);
        activeChunks.Add(newChunk);
    }

}
