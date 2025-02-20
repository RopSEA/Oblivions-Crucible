using UnityEngine;

public class CameraPan2D : MonoBehaviour
{
    public float targetY = 5f; // The final Y position to move to
    public float duration = 3f; // Time in seconds to complete the movement
    public TitleScreenManager titleManager;
    private Vector3 startPosition;
    private float elapsedTime = 0f;
    private bool hasShownTitle = false;
    void Start()
    {
        startPosition = transform.position; // Save initial position
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
        }
    }

    void ShowTitle()
    {
        titleManager.ShowTitleScreen();
    }
}