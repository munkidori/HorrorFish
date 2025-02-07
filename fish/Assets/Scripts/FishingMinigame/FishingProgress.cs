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
    }

    private void FixedUpdate()
    {
        float slider1Value = fish.value;
        float scrollbarValue = target.value;

        if (Mathf.Abs(slider1Value - scrollbarValue) < 0.1f)
            slider.value = Mathf.MoveTowards(GetComponent<Slider>().value, 1f, .5f * Time.deltaTime);
        else
            slider.value = Mathf.MoveTowards(GetComponent<Slider>().value, 0f, .2f * Time.deltaTime);

    }
}
