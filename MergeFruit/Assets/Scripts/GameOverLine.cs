using UnityEngine;
using System.Collections; 

public class GameOverLine : MonoBehaviour
{
    public GameObject gameOverPanel; 
    private GameObject[] fruits; 

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("SettledFruit")) 
        {
            Debug.Log("Game Over!");
            gameOverPanel.SetActive(true); 
            Time.timeScale = 0; 
            StartCoroutine(DestroyFruitsOneByOne()); 
        }
    }

    private IEnumerator DestroyFruitsOneByOne()
    {
        fruits = GameObject.FindGameObjectsWithTag("SettledFruit");

        foreach (GameObject fruit in fruits)
        {
            Destroy(fruit);

            yield return new WaitForSecondsRealtime(0.1f); 
        }
    }
}
