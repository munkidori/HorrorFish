using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Slider timer;
    public Canvas gameOver;
    public Canvas hud;
    public Canvas inventory;
    public GameObject player;

    private void Update()
    {
        if (timer.value <= 0)
        {
            GameOver();
        }
    }

    public void GameOver()
    {
        hud.gameObject.SetActive(false);
        inventory.gameObject.SetActive(false);
        gameOver.gameObject.SetActive(true);
    }

    public void Restart()
    {
        hud.gameObject.SetActive(true);
        inventory.gameObject.SetActive(true);
        gameOver.gameObject.SetActive(false);
        timer.value = 1f;
        player.transform.position = new Vector3(10, 0, 10);
    }
}
