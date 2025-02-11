using UnityEngine;

public class ContainerLine : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Fruit")) // Check if it's a fruit
        {
            other.gameObject.tag = "SettledFruit"; // Change tag to SettledFruit
        }
    }
}
