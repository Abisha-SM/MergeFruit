using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    public TextMeshProUGUI scoreText;
    public int score = 0;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void AddScore(int points)
    {
        score += points;
        scoreText.text = ""+score;

        // Check for level up
      //  LevelManager.instance.CheckLevelUp(score);
    }
}
