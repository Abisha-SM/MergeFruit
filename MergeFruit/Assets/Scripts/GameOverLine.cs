using UnityEngine;

public class GameOverLine : MonoBehaviour
{
    public GameObject gameOverPanel; // Assign the Game Over Panel in Inspector

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("SettledFruit")) // Check if it's a settled fruit
        {
            Debug.Log("Game Over!");
            gameOverPanel.SetActive(true); // Show the Game Over panel
            Time.timeScale = 0; // Pause the game
        }
    }
}
