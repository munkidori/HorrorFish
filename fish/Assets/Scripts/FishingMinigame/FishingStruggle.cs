using UnityEngine;
using UnityEngine.UI;

public class FishingStruggle : MonoBehaviour
{
    [SerializeField] private Slider slider;  // Make this visible in inspector
    private bool isPulling;
    private float cooldownTimer;
    private ItemData currentFish;

    private void Awake()
    {
        // Try to get slider if not assigned in inspector
        if (slider == null)
        {
            slider = GetComponent<Slider>();
            if (slider == null)
            {
                Debug.LogError($"No Slider component found on {gameObject.name}!");
            }
        }
    }

    private void Start()
    {
        // Double check slider reference
        if (slider == null)
        {
            Debug.LogError($"Slider is still null in Start() on {gameObject.name}!");
        }
    }

    private void Update()
    {
        if (currentFish == null) return;
        if (slider == null) return;  // Protect against null slider

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
        // Detailed null checks with specific error messages
        if (fish == null)
        {
            Debug.LogError($"Fish is NULL in GetFish() on {gameObject.name}!");
            return;
        }

        if (slider == null)
        {
            Debug.LogError($"Slider is NULL in GetFish() on {gameObject.name}! GameObject hierarchy: {GetGameObjectPath(gameObject)}");
            return;
        }

        currentFish = fish;
        isPulling = Random.value > 0.5f;
        cooldownTimer = fish.jumpCD;
        slider.value = 0.5f;
        Debug.Log($"Struggling with {fish.itemName} on {gameObject.name}!");
    }

    // Helper method to debug object hierarchy
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
}
