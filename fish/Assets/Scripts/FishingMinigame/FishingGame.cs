using UnityEngine;
using UnityEngine.UI;

public class FishingGame : MonoBehaviour
{
    public Slider progress;
    public Scrollbar scrollbar;
    public FishManager fishManager;
    private ItemData currentFish;
    public InventoryManager inventoryManager; // Add reference to InventoryManager

    public void StartFishingGame(ItemData selectedFish)
    {
        currentFish = selectedFish;
        Debug.Log($"Fishing for: {currentFish.itemName}");

        SetFish(currentFish);

        gameObject.SetActive(true);
        progress.value = 0.5f;
        scrollbar.value = 0f;
    }

    public void SetFish(ItemData fish)
    {
        currentFish = fish;
        var fishingStruggle = GetComponentInChildren<FishingStruggle>();
        if (fishingStruggle != null)
        {
            fishingStruggle.GetFish(currentFish);
        }
    }

    private void Update()
    {
        if (progress.value <= 0)
        {
            Debug.Log($"The {currentFish.itemName} escaped!");
            EndFishingGame(false);
        }
        else if (progress.value >= 1)
        {
            Debug.Log($"Caught {currentFish.itemName}!");
            EndFishingGame(true);
        }
    }

    private void EndFishingGame(bool caughtFish)
    {
        var fishingReel = GetComponentInChildren<FishingReel>();
        fishingReel.isClicked = false;

        if (caughtFish && inventoryManager != null)
        {
            AddFishToInventory();
        }

        progress.value = 0.5f;
        scrollbar.value = 0f;
        gameObject.SetActive(false);
        currentFish = null;
    }

    private void AddFishToInventory()
    {
        if (inventoryManager.SelectedItemGrid != null && currentFish != null)
        {
            GameObject fishItemObj = new GameObject(currentFish.itemName);
            InventoryItem fishItem = fishItemObj.AddComponent<InventoryItem>();
            fishItem.Set(currentFish);

            inventoryManager.InsertItem(fishItem);
            Debug.Log($"Added {currentFish.itemName} to inventory.");
        }
    }
}
