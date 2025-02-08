using TMPro;
using UnityEngine;

public class PuddleInteraction : MonoBehaviour
{
    public TextMeshProUGUI promptText; 
    public Canvas fishingMinigame; 
    private bool isNearPuddle = false;
    public FishManager fishManager;

    private void Start()
    {
        if (promptText != null)
            promptText.gameObject.SetActive(false);
            
        // Add debug check for fishManager
        if (fishManager == null)
            Debug.LogError("FishManager is not assigned to PuddleInteraction!");
    }

    void Update()
    {
        if (isNearPuddle && Input.GetKeyDown(KeyCode.Space))
        {
            if (fishingMinigame != null)
            {
                fishingMinigame.gameObject.SetActive(true);
                var fishingGame = fishingMinigame.GetComponent<FishingGame>();

                if (fishingGame != null)
                {
                    // Add debug logging
                    if (fishManager == null)
                    {
                        Debug.LogError("FishManager is null when trying to get random fish!");
                        return;
                    }

                    ItemData randomFish = fishManager.GetRandomFish();
                    Debug.Log($"Selected fish: {(randomFish != null ? randomFish.itemName : "NULL")}");

                    if (randomFish == null)
                    {
                        Debug.LogError("Selected fish is NULL in PuddleInteraction!");
                        return;
                    }

                    fishingGame.StartFishingGame(randomFish);
                }
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isNearPuddle = true;
            if (promptText != null)
                promptText.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isNearPuddle = false;
            if (promptText != null)
                promptText.gameObject.SetActive(false);
        }
    }
}
