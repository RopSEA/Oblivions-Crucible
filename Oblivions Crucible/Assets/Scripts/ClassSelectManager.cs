using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassSelectionManager : MonoBehaviour
{
    public static ClassSelectionManager Instance; 

    public string selectedClass; 

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SelectClass(string className)
    {
        selectedClass = className;
        Debug.Log("Class Selected: " + selectedClass);
    }
}

