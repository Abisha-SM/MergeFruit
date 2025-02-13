using UnityEngine;

public class WarningLine : MonoBehaviour
{
    public static WarningLine Instance;
    private SpriteRenderer spriteRenderer;
    private bool isBlinking = false;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = false;  
    }

    public void EnableWarning()
    {
        if (!isBlinking)
        {
            Debug.Log("Warning enabled, blinking should start.");
            isBlinking = true;
            StartCoroutine(BlinkLine());
        }
    }

    private System.Collections.IEnumerator BlinkLine()
    {
        while (true)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;
            yield return new WaitForSeconds(0.5f); 
        }
    }
}
