using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public Slider timer;
    public Canvas gameOverCanvas;
    public Canvas hud;
    public GameObject player;
    public FishingGame fishingGame;
    public ItemGrid grid;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (timer.value <= 0)
        {
            GameOver();
        }
    }

    public void GameOver()
    {
        fishingGame.HandleGameOver();
        hud.gameObject.SetActive(false);
        gameOverCanvas.gameObject.SetActive(true);
    }

    public void Restart()
    {    
        grid.ClearGrid();
        GameOver();
        timer.value = 1f;
        player.transform.position = new Vector3(15, 0, 7);
        player.transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    public void AddTime(float time)
    {
        timer.value += time;
    }
}
