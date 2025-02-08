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
                    ItemData randomFish = fishManager.GetRandomFish(); 
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
