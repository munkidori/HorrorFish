using UnityEngine;

public class FishManager : MonoBehaviour
{
    [SerializeField] ItemData[] fishList;

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