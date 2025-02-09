using UnityEngine;

public class FishManager : MonoBehaviour
{
    [SerializeField] ItemData[] fishList;

    private void Start()
    {
        // Add debug logging
        if (fishList == null || fishList.Length == 0)
        {
            Debug.LogError("Fish list is not initialized in FishManager!");
        }
        else
        {
            Debug.Log($"FishManager initialized with {fishList.Length} fish");
        }
    }

    public ItemData GetRandomFish()
    {
        if (fishList != null && fishList.Length > 0)
        {
            var fish = fishList[Random.Range(0, fishList.Length)];
            Debug.Log($"GetRandomFish returning: {fish.itemName}");
            return fish;
        }
        else
        {
            Debug.LogError("Fish list is empty or not initialized properly!");
            return null;
        }
    }
}