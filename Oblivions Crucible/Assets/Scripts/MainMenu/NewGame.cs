using UnityEngine;
using UnityEngine.SceneManagement;

public class NewGameMenu : MonoBehaviour 
{
    void OnMouseDown()
    {
        if (DataPersistenceManager.instance != null) 
        {
            DataPersistenceManager.instance.NewGame(); 
        }
        else
        {
            Debug.LogError("DataPersistenceManager instance is null!");
        }
    }
}
