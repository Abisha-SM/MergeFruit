using UnityEngine;

public class FruitSpawner : MonoBehaviour
{
    public static FruitSpawner instance; // Singleton instance
    public GameObject[] fruitPrefabs; // Array of fruit prefabs

    private GameObject currentFruit;  // The fruit ready to be dropped

    void Awake()
    {
        // Ensure there's only one instance of FruitSpawner
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject); // Destroy duplicates if any
        }
    }

    void Start()
    {
        SpawnFruit();
    }

    void Update()
    {
        if (currentFruit != null)
        {
            // Move fruit with player's mouse or touch
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            currentFruit.transform.position = new Vector2(mousePos.x, transform.position.y);

            // Drop fruit when clicked
            if (Input.GetMouseButtonDown(0))
            {
                DropFruit();
            }
        }
    }

    void SpawnFruit()
    {
        int randomIndex = Random.Range(0, fruitPrefabs.Length);
        currentFruit = Instantiate(fruitPrefabs[randomIndex], transform.position, Quaternion.identity);
        currentFruit.GetComponent<Rigidbody2D>().gravityScale = 0; // Keep fruit floating
    }

    void DropFruit()
    {
        currentFruit.GetComponent<Rigidbody2D>().gravityScale = 1; // Enable gravity
        currentFruit = null;
        Invoke("SpawnFruit", 0.5f); // Spawn new fruit after a delay
    }
}
