using UnityEngine;

public class ClassUIOpener : MonoBehaviour
{
    [Header("UI Panel to Show")]
    public GameObject uiPanelToShow;

    private void OnMouseEnter()
    {
        if (uiPanelToShow != null)
        {
            uiPanelToShow.SetActive(true);
        }
    }

    private void OnMouseExit()
    {
        if (uiPanelToShow != null)
        {
            uiPanelToShow.SetActive(false);
        }
    }
}
