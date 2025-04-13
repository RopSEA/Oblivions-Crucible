using UnityEngine;

public class RelicDraft : MonoBehaviour
{
    public GameObject relicSelectionUI; // Drag your UI Panel here

    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private Color hoverColor = new Color(0.7f, 0.7f, 0.7f, 1f); // Slight gray

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }

        if (relicSelectionUI != null)
        {
            relicSelectionUI.SetActive(false); // Start hidden
        }
    }

    void OnMouseEnter()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = hoverColor;
        }
    }

    void OnMouseExit()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = originalColor;
        }
    }

    void OnMouseDown()
    {
        if (relicSelectionUI != null)
        {
            relicSelectionUI.SetActive(true);
            Debug.Log("Opened Relic Selection UI");
        }
    }
}
