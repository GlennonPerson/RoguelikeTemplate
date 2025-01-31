using UnityEngine;
using System.Collections;

public class SkeletonSpawner : MonoBehaviour
{
    public GameObject skeletonPrefab;
    public float respawnDelay = 5f;

    private GameObject currentSkeleton;

    private void Start()
    {
        SpawnSkeleton();
    }

    private void Update()
    {
        // If skeleton is destroyed, start respawn timer
        if (currentSkeleton == null)
        {
            StartCoroutine(RespawnTimer());
            enabled = false; // Disable Update until new skeleton is spawned
        }
    }

    private IEnumerator RespawnTimer()
    {
        yield return new WaitForSeconds(respawnDelay);
        SpawnSkeleton();
        enabled = true; // Re-enable Update
    }

    private void SpawnSkeleton()
    {
        currentSkeleton = Instantiate(skeletonPrefab, transform.position, Quaternion.identity);
    }
}