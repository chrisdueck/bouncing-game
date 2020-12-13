using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private float Score = 0;
    [SerializeField] private SpawnManager spawnManager;
    [SerializeField] private GameObject titleScreen;
    [SerializeField] private Button startButton;
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private TextMeshProUGUI finalScoreText;
    [SerializeField] private Button restartButton;
    [SerializeField] private GameObject livesText;
    [SerializeField] private GameObject scoreText;

    public bool IsGameActive { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        startButton.onClick.AddListener(StartGame);
        restartButton.onClick.AddListener(Restart);
        spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
    }

    // Update is called once per frame
    void Update()
    {
        // Increase score once per second while game is active
        if (IsGameActive)
        {
            Score += Time.deltaTime % 60;
            scoreText.GetComponent<TextMeshProUGUI>().text = $"Score: {(int)Score}";
        }
    }

    private void StartGame()
    {
        // Deactivate title screen
        titleScreen.SetActive(false);
        livesText.SetActive(true);
        scoreText.SetActive(true);
        IsGameActive = true;
    }

    /// <summary>
    /// Halts the game and displays the Game Over screen
    /// </summary>
    public void GameOver()
    {
        IsGameActive = false;
        spawnManager.StopSpawning();
        finalScoreText.text = $"Score: {(int)Score}";
        gameOverScreen.SetActive(true);
        livesText.SetActive(false);
        scoreText.SetActive(false);
        Debug.Log("Game over!");
    }

    /// <summary>
    /// Starts the game from the beginning by reloading the scene
    /// </summary>
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
