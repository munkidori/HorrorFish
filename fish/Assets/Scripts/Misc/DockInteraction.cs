using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

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
            string currentScene = SceneManager.GetActiveScene().name;

            if (currentScene == "Hub")
                SceneManager.LoadScene("River");
            else if (currentScene == "River")
                SceneManager.LoadScene("Hub");
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
