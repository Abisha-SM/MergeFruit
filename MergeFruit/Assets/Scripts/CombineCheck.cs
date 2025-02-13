using System.Collections;
using TMPro;
using UnityEngine;

public class CombineCheck : MonoBehaviour
{
    public GameObject[] fruitPrefabs;

    public bool canCheckBoundary = false;

    private Coroutine mergingCoroutine;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Fruit"))
        {
            canCheckBoundary = true;

            if (collision.gameObject.name == transform.gameObject.name)
            {
                // ToDO : Add  score logic here
               

                // Determine master fruit
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

            // Make sure the rigidbody is not kinematic
            newFruit.GetComponent<Rigidbody2D>().isKinematic = false;

            // Delay the destruction of the current fruit
            yield return new WaitForSeconds(0.1f); // Adjust the delay as needed
            Destroy(gameObject);

            GameObject spawner = GameObject.Find("Spawner");
            if (spawner != null)
            {
                // Play the sound effect  here, any pop sound
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
