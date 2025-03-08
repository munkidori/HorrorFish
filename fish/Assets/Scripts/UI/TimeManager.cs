using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    public Slider slider;
    public float countdownSpeed; // 1 / countdownspeed = aantal seconden

    private void Start()
    {
        slider.value = slider.maxValue;
    }

    private void Update()
    {
        if (slider.value > 0)
            slider.value = Mathf.MoveTowards(slider.value, 0f, countdownSpeed * Time.deltaTime);
        else
            Debug.Log("Time's Up!");
    }
}
