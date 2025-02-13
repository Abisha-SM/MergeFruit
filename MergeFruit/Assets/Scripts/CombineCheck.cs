using System.Collections;
using TMPro;
using UnityEngine;

public class CombineCheck : MonoBehaviour
{
    public GameObject[] fruitPrefabs;

    public bool canCheckBoundary = false;

    private Coroutine mergingCoroutine;

    void Start()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.sharedMaterial = new PhysicsMaterial2D("Bouncy")
        {
            friction = 0.4f, // Increase friction to reduce movement
            bounciness = 0.2f
        };
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Fruit"))
        {
            canCheckBoundary = true;

            if (collision.gameObject.name == transform.gameObject.name)
            {
                // Check if the impact force is strong enough
                float impactForce = collision.relativeVelocity.magnitude;
                float minMergeForce = 1.5f; // Increase for higher difficulty

                if (impactForce >= minMergeForce)
                {
                    ScoreManager.instance.AddScore(1);
                    AudioManager.Instance.PlaySFX(AudioManager.Instance.mergeSound);

                    CombineCheck masterFruit = DetermineMasterFruit(collision.gameObject.GetComponent<CombineCheck>());
                    if (masterFruit == this)
                    {
                        if (mergingCoroutine != null)
                        {
                            StopCoroutine(mergingCoroutine);
                        }
                        mergingCoroutine = StartCoroutine(HandleCollision(collision));
                    }
                }
            }
        }
    }

    private CombineCheck DetermineMasterFruit(CombineCheck otherCombineCheck)
    {
        return (GetInstanceID() > otherCombineCheck.GetInstanceID()) ? this : otherCombineCheck;
    }

    private IEnumerator HandleCollision(Collision2D collision)
    {
        int fruitIndex = System.Array.FindIndex(this.fruitPrefabs, fruit => fruit.name == transform.gameObject.name);

        if (fruitIndex < this.fruitPrefabs.Length - 1)
        {
            Destroy(collision.gameObject);
            GameObject newFruit = Instantiate(this.fruitPrefabs[fruitIndex + 1], transform.position, Quaternion.identity);
            newFruit.name = this.fruitPrefabs[fruitIndex + 1].name;

            newFruit.GetComponent<CombineCheck>().canCheckBoundary = true;

            
            newFruit.GetComponent<Rigidbody2D>().isKinematic = false;

            
            yield return new WaitForSeconds(0.1f); 
            Destroy(gameObject);

            GameObject spawner = GameObject.Find("Spawner");
            if (spawner != null)
            {
                
            }
        }
    }

    private void OnDestroy()
    {
        // Stop the coroutine when the script is destroyed
        if (mergingCoroutine != null)
        {
            StopCoroutine(mergingCoroutine);
        }
    }
}
