using UnityEngine;
using UnityEngine.AI;

public class Spawner : MonoBehaviour
{
    public GameObject prefabToScatter; // The prefab you want to scatter
    public int numberOfPrefabs = 10; // Number of prefabs to scatter
    public Terrain terrain; // Reference to your terrain
    public float maxDistanceFromTerrain = 10f; // Maximum distance from terrain for finding a valid position

    void Start()
    {
        ScatterPrefabs();
    }

    void ScatterPrefabs()
    {
        if (prefabToScatter == null || terrain == null)
        {
            Debug.LogError("Prefab or terrain reference missing!");
            return;
        }

        TerrainData terrainData = terrain.terrainData;
        Vector3 terrainSize = terrainData.size;

        for (int i = 0; i < numberOfPrefabs; i++)
        {
            // Get a random direction within unit sphere and scale it by terrain size
            Vector3 randomDirection = Random.insideUnitSphere;
            Vector3 randomPosition = transform.position + randomDirection * Random.Range(0f, Mathf.Max(terrainSize.x, terrainSize.z));

            float y = terrain.SampleHeight(randomPosition);

            // Attempt to find a valid position within the specified maximum distance from terrain
            NavMeshHit hit;
            if (NavMesh.SamplePosition(new Vector3(randomPosition.x, y, randomPosition.z), out hit, maxDistanceFromTerrain, NavMesh.AllAreas))
            {
                // Instantiate the prefab at the valid NavMesh position
                Instantiate(prefabToScatter, hit.position, Quaternion.identity);
            }
            else
            {
                Debug.LogWarning("Failed to find a valid NavMesh position for spawning prefab.");
            }
        }
    }
}