using UnityEngine;
using UnityEngine.SceneManagement;

public class ContinueButton : MonoBehaviour
{
    public string sceneToSwitchTo = "GameScene";
    private Renderer objectRenderer;

    [Header("Visual Feedback")]
    public Color disabledColor = new Color(0.2f, 0.2f, 0.2f, 0.5f);
    public Color hoverColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);
    private Color originalColor;

    private bool interactable = true;

    void Start()
    {
        objectRenderer = GetComponent<Renderer>();

        if (objectRenderer != null)
        {
            originalColor = objectRenderer.material.color;
        }

        // Disable if tutorial not done
        if (DataPersistenceManager.instance == null || !DataPersistenceManager.instance.HasGameData() ||
            !DataPersistenceManager.instance.GameData.tutorialDone)
        {
            DisableButton();
        }
    }

    void OnMouseDown()
    {
        if (!interactable) return;

        if (!string.IsNullOrEmpty(sceneToSwitchTo))
        {
            AudioManager.instance?.PlaySfx("butt", false);
            SceneManager.LoadScene(sceneToSwitchTo);
        }
    }

    void OnMouseEnter()
    {
        if (!interactable) return;

        if (objectRenderer != null)
        {
            objectRenderer.material.color = hoverColor;
        }
    }

    void OnMouseExit()
    {
        if (objectRenderer != null)
        {
            objectRenderer.material.color = interactable ? originalColor : disabledColor;
        }
    }

    void DisableButton()
    {
        interactable = false;

        if (objectRenderer != null)
        {
            objectRenderer.material.color = disabledColor;
        }
    }
}
