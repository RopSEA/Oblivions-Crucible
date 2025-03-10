using UnityEngine;
using UnityEngine.SceneManagement;

public class QuitApp : MonoBehaviour
{
    private Renderer objectRenderer;
    private Color originalColor;
    private Color hoverColor = new Color(0.5f, 0.5f, 0.5f, 0.5f); // Transparent Gray

    public void doExitGame() {
    Application.Quit();
    }
    void Start()
    {
        objectRenderer = GetComponent<Renderer>();

        if (objectRenderer != null)
        {
            originalColor = objectRenderer.material.color;
        }
    }

    void OnMouseDown()
    {
        Debug.Log("Quitting Game...");
        doExitGame();
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }

    void OnMouseEnter()
    {
        if (objectRenderer != null)
        {
            objectRenderer.material.color = hoverColor; // Change color on hover
        }
    }

    void OnMouseExit()
    {
        if (objectRenderer != null)
        {
            objectRenderer.material.color = originalColor; // Restore color when exiting
        }
    }

}
