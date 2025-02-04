using UnityEngine;

public class FruitController : MonoBehaviour
{
    public string fruitType; // Fruit type (e.g., "Apple", "Orange")
    public int sizeLevel = 1; // Size level (smallest = 1, medium = 2, large = 3)

    private void OnCollisionEnter2D(Collision2D other)
    {
        // Debug log to check collision
        Debug.Log("Collision detected with: " + other.gameObject.name);

        // Check if this fruit collided with another fruit of the same type and size
        if (other.gameObject.CompareTag("Fruit"))
        {
            FruitController otherFruit = other.gameObject.GetComponent<FruitController>();
            if (otherFruit != null && otherFruit.fruitType == fruitType)
            {
                Debug.Log("Fruits merged: " + fruitType + " -> New size level: " + (sizeLevel + 1));
                // Merge the two fruits into a bigger one
                MergeFruits(other.gameObject);
            }
        }
    }

    void MergeFruits(GameObject otherFruit)
    {
        // Remove the current and other fruit
        Destroy(otherFruit);
        Destroy(gameObject);

        // Create a new larger fruit (based on sizeLevel)
        GameObject newFruit = Instantiate(fruitPrefab(sizeLevel + 1), transform.position, Quaternion.identity);
        newFruit.GetComponent<Rigidbody2D>().gravityScale = 0; // Stop it from falling immediately

        Debug.Log("New fruit spawned: " + newFruit.name);
    }

    // This function should return the prefab of the fruit based on sizeLevel
    GameObject fruitPrefab(int level)
    {
        // Modify this part to return the correct prefab based on level
        if (level == 2)
        {
            Debug.Log("Returning larger fruit prefab (level 2)");
            return FruitSpawner.instance.fruitPrefabs[1]; // Larger fruit prefab
        }
        if (level == 3)
        {
            Debug.Log("Returning even larger fruit prefab (level 3)");
            return FruitSpawner.instance.fruitPrefabs[2]; // Even larger fruit prefab
        }
        Debug.Log("Returning smallest fruit prefab (level 1)");
        return FruitSpawner.instance.fruitPrefabs[0]; // Default smallest fruit
    }
}
