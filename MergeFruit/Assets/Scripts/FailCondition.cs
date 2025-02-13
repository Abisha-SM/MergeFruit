using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class FailCondition : MonoBehaviour
{
    public GameObject[] fruitPrefabs;
    private float topY; // Y position of the top boundary


    void Start()
    {
        // Get the top boundary position
        GameObject topBoundary = GameObject.Find("Fail");
        if (topBoundary != null)
        {
            topY = topBoundary.transform.position.y;
        }
    }

    void Update()
    {
        GameObject[] fruits = GameObject.FindGameObjectsWithTag("Fruit");

        foreach (GameObject fruit in fruits)
        {
            CombineCheck fruitScript = fruit.GetComponent<CombineCheck>();

            // Check if the fruit is allowed to check the boundary
            if (fruitScript != null && fruitScript.canCheckBoundary)
            {
                if (fruit.transform.position.y >= topY)
                {
                    Failure();
                    break;
                }
            }
        }
    }

    public void Failure()
    {
        Debug.Log("Elan  comes Failure");

        // Stop the spawner
        GameObject spawner = GameObject.Find("Spawner");
        if (spawner != null)
        {
            if (spawner.TryGetComponent<FruitSpawnManager>(out var fruitSpawner))
            {
                fruitSpawner.isRunning = false;
            }
        }
        Debug.Log("Elan  comes Failure 111111");
        // Slide the "LosingScreen" object into view
        GameObject losingScreen = GameObject.Find("LosingScreen");
        //GameObject bg = GameObject.Find("Background");
        if (losingScreen != null)
        {
            Debug.Log("Elan  comes Failure 222222");
            losingScreen.transform.position = Vector3.zero;
        }

        // Count every fruit in the scene and destroy them while adding to the score with the formula (index + 1) * 2
        GameObject[] fruits = GameObject.FindGameObjectsWithTag("Fruit");
        // Freeze the game
        foreach (GameObject fruit in fruits)
        {
            StartCoroutine(DeleteFruitWithDelay(fruit));
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Fruit")
        {
            //Failure();
        }
    }

    IEnumerator DeleteFruitWithDelay(GameObject fruit)
    {
        yield return new WaitForSeconds(0.1f);
        Destroy(fruit);
    }
}
