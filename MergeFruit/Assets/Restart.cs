using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Restart : MonoBehaviour
{
    public Button reStartBtn;
    public Button quitBtn;

    void Start()
    {
        reStartBtn.onClick.AddListener(OnClickRestartBtn);
        quitBtn.onClick.AddListener(OnClickQuitBtn);
    }

    public void OnClickRestartBtn()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void OnClickQuitBtn()
    {
        Application.Quit();
    }

}
