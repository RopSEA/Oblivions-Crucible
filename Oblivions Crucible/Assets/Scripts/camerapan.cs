using UnityEngine;

public class CameraPan2D : MonoBehaviour
{
    public float targetY = 5f; // The final Y position to move to
    public float duration = 3f; // Time in seconds to complete the movement
    public TitleScreenManager titleManager;
    public CanvasGroup[] uiCanvasGroups; // Array to handle multiple UI groups

    private Vector3 startPosition;
    private float elapsedTime = 0f;
    private bool hasShownTitle = false;

    void Start()
    {
        startPosition = transform.position; // Save initial position

        // Hide all UI Canvas Groups at the start
        if (uiCanvasGroups != null)
        {
            foreach (CanvasGroup canvasGroup in uiCanvasGroups)
            {
                if (canvasGroup != null)
                {
                    canvasGroup.alpha = 0f; // Make UI invisible
                    canvasGroup.interactable = false;
                    canvasGroup.blocksRaycasts = false;
                }
            }
        }
    }

    void Update()
    {
        if (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            float newY = Mathf.Lerp(startPosition.y, targetY, t);
            transform.position = new Vector3(startPosition.x, newY, transform.position.z);
        }
        else if (!hasShownTitle) // Call only once after camera stops moving
        {
            hasShownTitle = true; // Prevent multiple calls

            if (titleManager != null)
            {
                Invoke("ShowTitle", 1f); // Delay title screen by 1 second
            }

            // Show all UI canvases after the camera stops moving
            Invoke("EnableUI", 1f); // Delay UI appearance for 1 second
        }
    }

    void ShowTitle()
    {
        titleManager.ShowTitleScreen();
    }

    
    void EnableUI()
    {
        if (uiCanvasGroups != null)
        {
            foreach (CanvasGroup canvasGroup in uiCanvasGroups)
            {
                if (canvasGroup != null)
                {
                    canvasGroup.alpha = 1f; // Make UI visible
                    canvasGroup.interactable = true;
                    canvasGroup.blocksRaycasts = true;
                }
            }
        }
    }
}
