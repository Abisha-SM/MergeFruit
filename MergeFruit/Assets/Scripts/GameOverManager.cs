using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public void RestartGame()
    {
        Time.timeScale = 1; // Resume the game
        SceneManager.LoadScene("Main Scene"); // Restart scene
    }
}
