using System.Collections.Generic;
using UnityEngine;

public class FishManager : MonoBehaviour
{
    public List<ItemData> fishList = new List<ItemData>();

    private void Start()
    {
        fishList.Add(CreateFish("Trash", 0.5f, 0.2f, 1.5f, -0.3f, 4, 4));
        fishList.Add(CreateFish("Salmon", 0.6f, 0.3f, 1.2f, 0.1f, 1, 1));
        fishList.Add(CreateFish("Trout", 0.7f, 0.4f, 1.5f, 0.2f, 2, 3));
        fishList.Add(CreateFish("Bass", 0.5f, 0.2f, 1f, 0.3f, 2, 1));
    }

    private ItemData CreateFish(string name, float pullForce, float pushForce, float jumpCD, float healAmount, int width, int height)
    {
        ItemData newFish = ScriptableObject.CreateInstance<ItemData>();
        newFish.itemName = name;
        newFish.pullForce = pullForce;
        newFish.pushForce = pushForce;
        newFish.jumpCD = jumpCD;
        newFish.healAmount = healAmount;
        newFish.width = width;
        newFish.height = height;
        return newFish;
    }

    public ItemData GetRandomFish()
    {
        if (fishList.Count > 0)
        {
            return fishList[Random.Range(0, fishList.Count)];
        }
        else
        {
            Debug.LogError("Fish list is empty! Add some fish to the list.");
            return null;
        }
    }
}
