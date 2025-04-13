using UnityEngine;

public class RelicSelectable : MonoBehaviour
{
    public string relicName;
    private SpriteRenderer spriteRenderer;

    private Color originalColor;
    private Color hoverColor = new Color(0.6f, 0.6f, 0.6f, 1f); // Slight gray on hover

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
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
        RelicSelectionUI.instance.SelectRelic(relicName, this);
    }

    public void LockVisual()
    {
        spriteRenderer.color = Color.green; // Mark as selected
    }
}
