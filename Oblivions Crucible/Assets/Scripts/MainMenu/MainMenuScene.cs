using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuOption : MonoBehaviour
{
    public string sceneToSwitchTo = "GameScene"; // Assign in Inspector
    private Renderer objectRenderer;
    private Color originalColor;
    private Color hoverColor = new Color(0.5f, 0.5f, 0.5f, 0.5f); // Transparent Gray

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
        if (!string.IsNullOrEmpty(sceneToSwitchTo))
        {
            SwitchScene(sceneToSwitchTo);
            //New save data
            if (sceneToSwitchTo == "IntroCut"){
                Debug.Log("Starting a new game...");

                FindObjectOfType<DataPersistenceManager>().NewGame();
                FindObjectOfType<DataPersistenceManager>().SaveGame();
            }

        }
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

    void SwitchScene(string newScene)
    {
        Scene currentScene = SceneManager.GetActiveScene();

        if (!IsSceneLoaded(newScene))
        {
            StartCoroutine(LoadNewScene(newScene, currentScene));
        }
        else
        {
            Debug.Log("Scene already loaded, switching focus.");
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(newScene));
        }
    }

    System.Collections.IEnumerator LoadNewScene(string newScene, Scene currentScene)
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(newScene, LoadSceneMode.Additive);

        // Wait until the scene is fully loaded
        while (!loadOperation.isDone)
        {
            yield return null;
        }

        SceneManager.SetActiveScene(SceneManager.GetSceneByName(newScene));

        if (currentScene.name != newScene)
        {
            SceneManager.UnloadSceneAsync(currentScene.name);
        }
    }

    bool IsSceneLoaded(string sceneName)
    {
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            if (SceneManager.GetSceneAt(i).name == sceneName)
            {
                return true; // Scene is already loaded
            }
        }
        return false; // Scene is not loaded
    }
}
