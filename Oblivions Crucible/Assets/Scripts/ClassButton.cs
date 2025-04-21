using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClassClickHandler : MonoBehaviour
{
    public string className;  // Name of the class, set in Inspector
    public string sceneToLoad; // Scene name to load, set in Inspector


    public string Descript;

    public TextMeshProUGUI text;

    private void OnMouseEnter()
    {
        text.text = Descript;
    }

    private void OnMouseExit()
    {
        text.text = "";
    }

    void OnMouseDown() // Detects clicks on the object
    {
        Debug.Log("Sprite Clicked: " + className);
        AudioManager.instance.PlaySfx("butt", false);
        if (ClassSelectionManager.Instance != null)
        {
            ClassSelectionManager.Instance.SelectClass(className);
            Debug.Log("Class Selected: " + ClassSelectionManager.Instance.selectedClass);
            
            if (!string.IsNullOrEmpty(sceneToLoad))
            {
                SceneManager.LoadScene(sceneToLoad); // Loads the scene from Inspector
            }
            else
            {
                Debug.LogError("Scene name not set in Inspector!");
            }
        }
        else
        {
            Debug.LogError("ClassSelectionManager not found in scene!");
        }
    }
}
