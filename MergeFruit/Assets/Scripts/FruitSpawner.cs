using UnityEngine;

public class FruitSpawner : MonoBehaviour
{
    public static FruitSpawner instance;
    public GameObject[] fruitPrefabs;
    private Camera mainCamera;
    private GameObject currentFruit;
    public float fallSpeed = 3f; 
    public float speedIncreaseRate = 3f; 

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        mainCamera = Camera.main;
        SpawnFruit();
    }

    void Update()
    {
        if (currentFruit != null)
        {
            Vector2 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            mousePos.x = Mathf.Clamp(mousePos.x, 0.5f, 4f);
            currentFruit.transform.position = new Vector2(mousePos.x, transform.position.y);

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
        currentFruit.GetComponent<Rigidbody2D>().gravityScale = 0;
    }

    void DropFruit()
    {
        Rigidbody2D rb = currentFruit.GetComponent<Rigidbody2D>();
        rb.gravityScale = fallSpeed; 
        fallSpeed += speedIncreaseRate; 
        currentFruit = null;
        Invoke("SpawnFruit", 1f);
    }

    public GameObject GetNextFruitPrefab(int nextLevel)
    {
        if (nextLevel < fruitPrefabs.Length)
        {
            return fruitPrefabs[nextLevel];
        }
        else
        {
            Debug.LogWarning("No bigger fruit available for level: " + nextLevel);
            return null;
        }
    }
}
