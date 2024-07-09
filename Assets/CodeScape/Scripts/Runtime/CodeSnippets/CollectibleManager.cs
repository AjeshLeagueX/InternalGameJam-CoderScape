using MalbersAnimations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleManager : MonoBehaviour
{
    public GameObject collectiblePrefab;
    public List<Transform> spawnPoints;
    private Dictionary<Transform, float> respawnTimers = new Dictionary<Transform, float>();
    public float respawnTime = 60f;

    void Start()
    {
        // Initialize the collectibles at the spawn points
        foreach (Transform spawnPoint in spawnPoints)
        {
            SpawnCollectible(spawnPoint);
            respawnTimers.Add(spawnPoint, 0f);
        }
    }

    void Update()
    {
        // Check for respawn timers
        List<Transform> keys = new List<Transform>(respawnTimers.Keys);
        foreach (Transform spawnPoint in keys)
        {
            if (respawnTimers[spawnPoint] > 0)
            {
                respawnTimers[spawnPoint] -= Time.deltaTime;
                if (respawnTimers[spawnPoint] <= 0)
                {
                    SpawnCollectible(spawnPoint);
                }
            }
        }
    }

    void SpawnCollectible(Transform spawnPoint)
    {
        GameObject collectible = Instantiate(collectiblePrefab, spawnPoint.position, spawnPoint.rotation);
        collectible.GetComponent<CollectibleCodeSnippet>().Init(this, spawnPoint);
    }

    public void Collect(Transform spawnPoint)
    {
        if (respawnTimers.ContainsKey(spawnPoint))
        {
            respawnTimers[spawnPoint] = respawnTime;
        }
    }
}
