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
            AudioManager.Instance.PlaySFX(AudioManager.Instance.mergeSound);
            if (fruitLevel == 0 || fruitLevel == 1 || fruitLevel == 2)
            {
                ScoreManager.instance.AddScore(1);
            }
            else if (fruitLevel == 3 || fruitLevel == 4)
            {
                ScoreManager.instance.AddScore(2);
            }
            else if (fruitLevel == 5)
            {
                ScoreManager.instance.AddScore(3);
            }
            else if (fruitLevel == 6)
            {
                ScoreManager.instance.AddScore(4);
            }
            else
            {
                ScoreManager.instance.AddScore(5);
            }


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
