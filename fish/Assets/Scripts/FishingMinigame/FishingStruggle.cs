using UnityEngine;
using UnityEngine.UI;

public class FishingStruggle : MonoBehaviour
{
    private Slider slider;
    private bool isPulling;
    private float cooldownTimer;
    private ItemData currentFish;

    private void Start()
    {
        slider = GetComponent<Slider>();
    }

    private void Update()
    {
        if (currentFish == null) return;

        cooldownTimer -= Time.deltaTime;
        if (cooldownTimer <= 0f)
        {
            isPulling = !isPulling;
            cooldownTimer = currentFish.jumpCD;
        }

        if (isPulling)
            slider.value = Mathf.MoveTowards(slider.value, 1f, currentFish.pullForce * Time.deltaTime);
        else
            slider.value = Mathf.MoveTowards(slider.value, 0f, currentFish.pushForce * Time.deltaTime);
    }

    public void GetFish(ItemData fish)
    {
        currentFish = fish;
        isPulling = Random.value > 0.5f;
        cooldownTimer = fish.jumpCD;
        slider.value = 0.5f;
        Debug.Log($"Struggling with {fish.name}!");
    }

}
