using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Slider timer;
    public Canvas gameOver;
    public Canvas hud;
    public GameObject player;
    public FishingGame fishingGame;
    public ItemGrid grid;

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
        gameOver.gameObject.SetActive(true);
    }

    public void Restart()
    {
        fishingGame.HandleGameOver();
        grid.ClearGrid();
        hud.gameObject.SetActive(true);
        gameOver.gameObject.SetActive(false);
        timer.value = 1f;
        player.transform.position = new Vector3(10, 0, 10);
        player.transform.rotation = Quaternion.Euler(0, 0, 0);
    }
}
