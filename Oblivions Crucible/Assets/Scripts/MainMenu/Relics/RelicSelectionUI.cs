using UnityEngine;
using System.Collections.Generic;

public class RelicSelectionUI : MonoBehaviour
{
    public static RelicSelectionUI instance;

    public int maxSelectableRelics = 3;
    public List<string> selectedRelicNames = new List<string>();

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    public void SelectRelic(string relicName, RelicSelectable relicVisual)
    {
        if (selectedRelicNames.Contains(relicName) || selectedRelicNames.Count >= maxSelectableRelics)
            return;

        selectedRelicNames.Add(relicName);
        relicVisual.LockVisual();

        Debug.Log("Selected Relic: " + relicName);

        if (selectedRelicNames.Count == maxSelectableRelics)
        {
            Debug.Log("Max relics selected! Proceeding...");
            RelicRunData.instance.InitializeRun(selectedRelicNames);
            // Proceed to start the run or enable start button
        }
    }
}
