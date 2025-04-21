using Unity.VisualScripting;
using UnityEngine;

public class RelicDraft : MonoBehaviour
{
    public GameObject relicSelectionUI;
    public GameObject oldUi;// Drag your UI Panel here

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
        AudioManager.instance.PlaySfx("butt", false);
        if (relicSelectionUI != null)
        {
            if (relicSelectionUI.activeSelf == true)
            {
                relicSelectionUI.SetActive(false);
                oldUi.SetActive(true);
            }
            else
            {
                oldUi.SetActive(false);
                relicSelectionUI.SetActive(true);
                Debug.Log("Opened Relic Selection UI");
            }
            
        }
    }
}