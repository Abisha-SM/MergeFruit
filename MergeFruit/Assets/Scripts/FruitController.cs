using UnityEngine;

public class FruitController : MonoBehaviour
{
    public int fruitLevel;
    private bool isMerging = false;
    private Rigidbody2D rb;

    private bool isSettled = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.down * 4f;
    }

    private void Update()
    {
        if (rb.velocity.magnitude < 0.1f && !isMerging) // When fruit stops moving
        {
            gameObject.tag = "SettledFruit"; // Change tag to SettledFruit
        }
    }


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

            int scoreToAdd = (fruitLevel <= 2) ? 1 : (fruitLevel <= 4) ? 2 : (fruitLevel == 5) ? 3 : (fruitLevel == 6) ? 4 : 5;
            ScoreManager.instance.AddScore(scoreToAdd);
        }
    }

    void MergeFruits(FruitController otherFruit)
    {
        Destroy(otherFruit.gameObject);
        Destroy(gameObject);

        GameObject nextFruitPrefab = FruitSpawner.instance.GetNextFruitPrefab(fruitLevel + 1);

        if (nextFruitPrefab != null)
        {
            GameObject newFruit = Instantiate(nextFruitPrefab, transform.position, Quaternion.identity);
            newFruit.GetComponent<Rigidbody2D>().velocity = Vector2.down * 6f;
        }
    }
}
