using System.Collections.Generic;
using UnityEngine;

public class FishManager : MonoBehaviour
{
    public List<Fish> fishList = new List<Fish>();

    private void Start()
    {
        fishList.Add(new Fish("Trash", .5f, .2f, 1.5f, -0.3f, 4, 4));
        fishList.Add(new Fish("Salmon", 0.6f, 0.3f, 1.2f, 0.1f, 1, 1));
        fishList.Add(new Fish("Trout", 0.7f, 0.4f, 1.5f, 0.2f, 2, 3));
        fishList.Add(new Fish("Bass", 0.5f, 0.2f, 1f, 0.3f, 2, 1));
    }

    public Fish GetRandomFish()
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
