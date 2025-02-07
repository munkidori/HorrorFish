using UnityEngine.UI;
using UnityEngine;

public class FishingGame : MonoBehaviour
{
    public Slider progress;
    public Slider struggle;
    public Scrollbar scrollbar;
    public FishManager fishManager;
    private Fish currentFish;

    public void StartFishingGame(Fish selectedFish)
    {
        currentFish = selectedFish;
        Debug.Log($"Fishing for: {currentFish.name}");

        SetFish(currentFish); 

        gameObject.SetActive(true);
        progress.value = 0.5f;
        scrollbar.value = 0f;
    }

    public void SetFish(Fish fish)
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
            Debug.Log($"The {currentFish.name} escaped!");
            EndFishingGame(false);
        }
        else if (progress.value >= 1)
        {
            Debug.Log($"Caught {currentFish.name}!");
            EndFishingGame(true);
        }
    }

    private void EndFishingGame(bool caughtFish)
    {
        var fishingReel = GetComponentInChildren<FishingReel>();
        fishingReel.isClicked = false;

        progress.value = 0.5f;
        scrollbar.value = 0f;
        gameObject.SetActive(false);
        currentFish = null;
    }

}
