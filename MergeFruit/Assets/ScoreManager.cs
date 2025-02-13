using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    public TextMeshProUGUI scoreText;
    public int score = 0;
    public Button back;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void Start()
    {
        back.onClick.AddListener(OnclickBack);
    }

    public void AddScore(int points)
    {
        score += points;
        scoreText.text = ""+score;

        // Check for level up
      //  LevelManager.instance.CheckLevelUp(score);
    }

    public void OnclickBack()
    {
        SceneManager.LoadScene(1);
    }
}
