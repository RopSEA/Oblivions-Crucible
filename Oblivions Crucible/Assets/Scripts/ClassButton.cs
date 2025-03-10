using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClassClickHandler : MonoBehaviour
{
    public string className; 

    void OnMouseDown() // Detects clicks on the object
    {
        Debug.Log("Sprite Clicked: " + className);

        if (ClassSelectionManager.Instance != null)
        {
            ClassSelectionManager.Instance.SelectClass(className);
            Debug.Log("Class Selected: " + ClassSelectionManager.Instance.selectedClass);
            SceneManager.LoadScene("Arena"); // Change scene
        }
        else
        {
            Debug.LogError("ClassSelectionManager not found in scene!");
        }
    }
}
