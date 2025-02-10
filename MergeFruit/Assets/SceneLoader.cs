using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private Button play;

    private void Start()
    {
        play.onClick.AddListener(SwithScene);
    }

    private void SwithScene()
    {
        SceneManager.LoadScene(2);
    }
}
