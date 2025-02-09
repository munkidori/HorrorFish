using UnityEngine;
using UnityEngine.UI;

public class FishingGame : MonoBehaviour
{
    public Slider progress;
    public Scrollbar scrollbar;
    public FishManager fishManager;
    private ItemData currentFish;
    public InventoryManager inventoryManager;
    [SerializeField] private FishingStruggle fishingStruggle;
    [SerializeField] private GameObject itemPrefab;  // Assign your inventory item prefab in inspector
    [SerializeField] private Transform canvasTransform;  // Assign your UI canvas transform
    [SerializeField] private ItemGrid defaultInventoryGrid; // Assign this in inspector

    void Start()
    {
        if (fishingStruggle == null)
        {
            fishingStruggle = FindFirstObjectByType<FishingStruggle>();
            if (fishingStruggle == null)
            {
                Debug.LogError("FishingStruggle is NOT found in the scene!");
            }
        }

        // Initialize progress to middle
        if (progress != null)
        {
            progress.value = 0.5f;
        }
    }

    private void Update()
    {
        if (currentFish == null) return; // Don't update if no fish

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

    public void StartFishingGame(ItemData selectedFish)
    {
        if (selectedFish == null)
        {
            Debug.LogError("Trying to start fishing game with null fish!");
            return;
        }

        currentFish = selectedFish;
        Debug.Log($"Fishing for: {currentFish.itemName}");

        // Initialize UI elements
        progress.value = 0.5f;  // Start in the middle
        scrollbar.value = 0f;

        // Set up the fish
        SetFish(currentFish);

        gameObject.SetActive(true);
    }

    public void SetFish(ItemData fish)
    {
        if (fish == null)
        {
            Debug.LogError("Selected fish is NULL in FishingGame.SetFish!");
            return;
        }

        currentFish = fish;
        var fishingStruggle = GetComponentInChildren<FishingStruggle>();
        if (fishingStruggle == null)
        {
            Debug.LogError($"FishingStruggle component not found in children of {gameObject.name}! Hierarchy: {GetGameObjectPath(gameObject)}");
            return;
        }

        Debug.Log($"Found FishingStruggle component on {fishingStruggle.gameObject.name}");
        fishingStruggle.GetFish(currentFish);
    }

    private string GetGameObjectPath(GameObject obj)
    {
        string path = obj.name;
        Transform parent = obj.transform.parent;
        while (parent != null)
        {
            path = parent.name + "/" + path;
            parent = parent.parent;
        }
        return path;
    }

    private void EndFishingGame(bool caughtFish)
    {
        var fishingReel = GetComponentInChildren<FishingReel>();
        if (fishingReel != null)
        {
            fishingReel.isClicked = false;
        }

        if (caughtFish && inventoryManager != null)
        {
            AddFishToInventory();
        }

        // Reset UI
        progress.value = 0.5f;
        scrollbar.value = 0f;
        
        currentFish = null;
        gameObject.SetActive(false);
    }

    private void AddFishToInventory()
    {
        // Store the current selected grid
        var originalSelectedGrid = inventoryManager.SelectedItemGrid;
        
        // Force select our default grid
        if (defaultInventoryGrid != null)
        {
            inventoryManager.SelectedItemGrid = defaultInventoryGrid;
        }

        GameObject itemObject = Instantiate(itemPrefab, canvasTransform);
        InventoryItem item = itemObject.GetComponent<InventoryItem>();
        
        if (item != null)
        {
            item.Set(currentFish);
            Debug.Log($"Created inventory item for {currentFish.itemName}");
            
            // Use InsertItem instead of direct placement
            inventoryManager.InsertItem(item);
        }
        else
        {
            Debug.LogError("Created item object doesn't have InventoryItem component!");
            Destroy(itemObject);
        }

        // Restore the original selected grid
        inventoryManager.SelectedItemGrid = originalSelectedGrid;
    }

    public void HandleGameOver()
    {
        if (gameObject.activeSelf && currentFish != null)
        {
            Debug.Log($"The {currentFish.itemName} escaped due to game over!");
            EndFishingGame(false);

            // Clear inventory when game is over
            if (defaultInventoryGrid != null)
            {
                defaultInventoryGrid.ClearGrid();
            }
        }
    }
}
