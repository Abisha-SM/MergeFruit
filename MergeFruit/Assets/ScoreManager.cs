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
    public TextMeshProUGUI wintext;
    public Button play;

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
        play.onClick.AddListener(OnPlayBtn);
    }

    public void Update()
    {
       
    }

    public void AddScore(int points)
    {
        score += points;
        scoreText.text = ""+score;
        wintext.text= "" + score;
        // Check for level up
        //  LevelManager.instance.CheckLevelUp(score);
    }

    public void OnclickBack()
    {
        SceneManager.LoadScene(1);
    }

    public void OnPlayBtn()
    {
        score = 0;
        scoreText.text = "" + score;
        // GameObject winningScreen = GameObject.Find("Win Panel");
        // winningScreen.transform.position=Vector3.down;
        SceneManager.LoadScene(1);
    }
 
}
