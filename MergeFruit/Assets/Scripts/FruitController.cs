using UnityEngine;

public class FruitController : MonoBehaviour
{
    public int fruitLevel; 
    private bool isMerging = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isMerging) return; 

        FruitController otherFruit = collision.gameObject.GetComponent<FruitController>();

       
        if (otherFruit != null && otherFruit.fruitLevel == fruitLevel)
        {
            isMerging = true;
            otherFruit.isMerging = true; 

            MergeFruits(otherFruit);
        }
    }

    void MergeFruits(FruitController otherFruit)
    {
        Debug.Log("Merging Fruit Level: " + fruitLevel); 
        Destroy(otherFruit.gameObject);
        Destroy(gameObject);

        
        GameObject nextFruitPrefab = FruitSpawner.instance.GetNextFruitPrefab(fruitLevel + 1);

        if (nextFruitPrefab != null) 
        {
            GameObject newFruit = Instantiate(nextFruitPrefab, transform.position, Quaternion.identity);
            newFruit.GetComponent<Rigidbody2D>().velocity = Vector2.down * 4f; 
        }
    }

}
