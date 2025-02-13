using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitSpawnManager : MonoBehaviour
{
    public bool isRunning = true;
    int fruitNumber = 0;
    bool fruitExists = false;
    public GameObject[] fruitPrefabs;
    string currentFruitName = "ReleasedFruit";
    public GameObject gameOverScreen;
    public GameObject topBoundary;

    // Cooldown timer
    float previousSpawnTime = 0f;

    void Start()
    {
        fruitExists = false;
    }

    void Update()
    {
        if (!isRunning) return; // Stop processing if game is over

        // Move spawner based on mouse position within limits
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float leftLimit = GameObject.Find("LBOUND").transform.position.x;
        float rightLimit = GameObject.Find("RBOUND").transform.position.x;
        mousePos.x = Mathf.Clamp(mousePos.x, leftLimit, rightLimit);
        transform.position = new Vector3(mousePos.x, transform.position.y, transform.position.z);

        // Spawn new fruit if cooldown is over and no fruit exists
        if (Time.time - previousSpawnTime >= 1f && !fruitExists)
        {
            SpawnFruit();
        }

        // Move the spawned fruit with the spawner
        if (fruitExists)
        {
            GameObject fruit = GameObject.Find("Fruit");
            if (fruit != null)
            {
                fruit.transform.position = transform.position;
            }
        }

        // Release the fruit on mouse click
        if (Input.GetMouseButtonDown(0))
        {
            ReleaseFruit();
        }
    }

    void SpawnFruit()
    {
        int fruitIndex = Random.Range(0, fruitPrefabs.Length);
        if (fruitNumber < 5)
        {
            fruitIndex = Mathf.Min(fruitIndex, fruitNumber);
            if (fruitIndex >= fruitNumber) fruitNumber++;
        }

        currentFruitName = fruitPrefabs[fruitIndex].name;

        Quaternion randomRotation = Quaternion.Euler(0, 0, Random.Range(-15f, 15f));

        GameObject fruit = Instantiate(fruitPrefabs[fruitIndex], transform.position, randomRotation);
        AudioManager.Instance.PlaySFX(AudioManager.Instance.spawnSound);
        fruit.name = "Fruit";
        fruit.GetComponent<Rigidbody2D>().isKinematic = true;
        fruitExists = true;
    }

    void ReleaseFruit()
    {
        GameObject fruit = GameObject.Find("Fruit");
        if (fruit != null)
        {
            fruit.name = currentFruitName;
            fruit.GetComponent<Rigidbody2D>().isKinematic = false;
            fruit.GetComponent<Rigidbody2D>().gravityScale = 2f; // Increase gravity
            fruit.layer = 6;
            fruitExists = false;
            previousSpawnTime = Time.time;
        }
    }
}
