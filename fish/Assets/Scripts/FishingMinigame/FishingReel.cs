using UnityEngine;
using UnityEngine.UI;

public class FishingReel : MonoBehaviour
{
    private Scrollbar scrollbar;
    public float speed; 
    public bool isClicked = false;

    private void Start()
    {
        scrollbar = GetComponent<Scrollbar>();
        isClicked = false;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            isClicked = true;
        if (Input.GetMouseButtonUp(0))
            isClicked = false;

        if (isClicked)
        {
            if (scrollbar.value < 1) 
                scrollbar.value += speed * Time.deltaTime;
        }
        else
        {
            if (scrollbar.value > 0) 
                scrollbar.value -= speed * Time.deltaTime;
        }
    }
}
