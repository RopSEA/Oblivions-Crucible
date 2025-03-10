using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClassHandler : MonoBehaviour
{
    public GameObject speedsterPrefab;
    public GameObject engineerPrefab;
    private GameObject playerInstance;
    public CameraFollow cameraFollow;

    private void Start()
    {
        if (ClassSelectionManager.Instance == null)
        {
            Debug.LogError("No class selected! Returning to selection scene.");
            UnityEngine.SceneManagement.SceneManager.LoadScene("Selection");
            return;
        }

        string selectedClass = ClassSelectionManager.Instance.selectedClass;
        Debug.Log("Spawning player as: " + selectedClass);

        if (selectedClass == "Speedster")
        {
            playerInstance = Instantiate(speedsterPrefab, Vector3.zero, Quaternion.identity);
        }
        else if (selectedClass == "Engineer")
        {
            playerInstance = Instantiate(engineerPrefab, Vector3.zero, Quaternion.identity);
        }
        else
        {
            Debug.LogError("Invalid class selected!");
        }
        if (playerInstance != null)
        {
            playerInstance.SetActive(true);
        }
        if (cameraFollow != null)
        {
            cameraFollow.SetTarget(playerInstance.transform);
            Debug.Log("Camera is now following the player!");
        }
    }
}
