using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Game variables
    private int score;
    private int moves;
    private bool isGameOver;

    // UI references
    [SerializeField] private TextMeshProUGUI scoreValue;
    [SerializeField] private TextMeshProUGUI movesValue;
    [SerializeField] private GameObject gameOverScreen;

    // Esta función se llama automáticamente una sola vez cuando empieza el juego
    void Start()
    {
        StartGame();
    }

    // Initial game state
    void StartGame()
    {
        score = 0;
        moves = 5; // start with 5 movements

        isGameOver = false; // start with game over false
        gameOverScreen.SetActive(false); // hide gameover screen

        UpdateUI(); // update the UI
    }

    // Actualiza los textos de la UI con los valores actuales
    void UpdateUI()
    {
        scoreValue.text = score.ToString();
        movesValue.text = moves.ToString();
    }

    // this function is for "make move" button - to test the end game
    public void MakeMoveTest()
    {
        if (isGameOver)
        {
            return;
        }

        if (moves > 0)
        {
            moves--;
            score += 10;
            UpdateUI();

            if (moves <= 0)
            {
                TriggerGameOver();
            }
        }
    }

    // it shows the end game
    void TriggerGameOver()
    {
        isGameOver = true;
        gameOverScreen.SetActive(true);
    }

    public void Replay()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
