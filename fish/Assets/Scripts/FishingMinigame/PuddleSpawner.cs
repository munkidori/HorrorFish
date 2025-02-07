using UnityEngine;

public class PuddleSpawner : MonoBehaviour
{
    public GameObject puddlePrefab; 
    public float spawnIntervalMin = 2f; 
    public float spawnIntervalMax = 5f; 
    public float disappearIntervalMin = 2f; 
    public float disappearIntervalMax = 5f; 
    public Canvas fishingMinigame; 
    public FishManager fishManager;

    private void Start()
    {
        InvokeRepeating("SpawnPuddle", 0f, Random.Range(spawnIntervalMin, spawnIntervalMax));
    }

    private void SpawnPuddle()
    {
        GameObject[] floors = GameObject.FindGameObjectsWithTag("Terrain");
        if (floors.Length == 0) return; // No terrain found, exit

        // Pick a random floor
        GameObject selectedFloor = floors[Random.Range(0, floors.Length)];

        // Get the Mesh bounds to determine where to spawn
        MeshFilter meshFilter = selectedFloor.GetComponent<MeshFilter>();
        if (meshFilter == null) return;

        Bounds bounds = meshFilter.mesh.bounds;
        Vector3 floorPosition = selectedFloor.transform.position;

        // Generate a random position within the floor bounds
        float randomX = Random.Range(bounds.min.x, bounds.max.x) + floorPosition.x;
        float randomZ = Random.Range(bounds.min.z, bounds.max.z) + floorPosition.z;
        float randomY = floorPosition.y; // Meshes are flat, so just use the floor's Y

        Vector3 spawnPosition = new Vector3(randomX, randomY, randomZ);
        GameObject puddle = Instantiate(puddlePrefab, spawnPosition, Quaternion.identity);

        PuddleInteraction puddleInteraction = puddle.GetComponent<PuddleInteraction>();
        if (puddleInteraction != null)
        {
            puddleInteraction.fishingMinigame = fishingMinigame;
            puddleInteraction.fishManager = fishManager;
        }

        float disappearTime = Random.Range(disappearIntervalMin, disappearIntervalMax);
        Destroy(puddle, disappearTime);
    }

}
