using UnityEngine;
using UnityEngine.UI;

public class FishingProgress : MonoBehaviour
{
    public Slider slider;
    public Slider fish;
    public Scrollbar target;

    private void Start()
    {
        slider = GetComponent<Slider>();
        if (slider != null)
        {
            slider.value = 0.5f; // Start in the middle
        }
    }

    private void FixedUpdate()
    {
        if (slider == null || fish == null || target == null) return;

        float slider1Value = fish.value;
        float scrollbarValue = target.value;

        if (Mathf.Abs(slider1Value - scrollbarValue) < 0.1f)
            slider.value = Mathf.MoveTowards(slider.value, 1f, 0.5f * Time.deltaTime);
        else
            slider.value = Mathf.MoveTowards(slider.value, 0f, 0.2f * Time.deltaTime);
    }
}
