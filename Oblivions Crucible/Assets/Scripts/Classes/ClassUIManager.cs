using UnityEngine;

public class ClassUIManager : MonoBehaviour
{
    public GameObject speedsterUI;
    public GameObject engineerUI;
    public GameObject mageUI;
    public GameObject tankUI;

    public void OpenSpeedsterUI() => ShowOnly(speedsterUI);
    public void OpenEngineerUI() => ShowOnly(engineerUI);
    public void OpenMageUI() => ShowOnly(mageUI);
    public void OpenTankUI() => ShowOnly(tankUI);

    public void CloseAllUIs()
    {
        speedsterUI.SetActive(false);
        engineerUI.SetActive(false);
        mageUI.SetActive(false);
        tankUI.SetActive(false);
    }

    private void ShowOnly(GameObject panel)
    {
        CloseAllUIs();
        if (panel != null)
            panel.SetActive(true);
    }
}
