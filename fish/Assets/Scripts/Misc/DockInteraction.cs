using TMPro;
using UnityEngine;

public class DockInteraction : MonoBehaviour
{
    public TextMeshProUGUI promptText;
    private bool isNearDock = false;
    public GameManager gameManager;

    private void Start()
    {
        if (promptText != null)
            promptText.gameObject.SetActive(false);
    }

    void Update()
    {
        if (isNearDock && Input.GetKeyDown(KeyCode.Space))
        {
            gameManager.GameOver();
        }
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("I'm at the dock!!!");
            isNearDock = true;
            if (promptText != null)
                promptText.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("I left the dock!!!");
            isNearDock = false;
            if (promptText != null)
                promptText.gameObject.SetActive(false);
        }
    }
}
