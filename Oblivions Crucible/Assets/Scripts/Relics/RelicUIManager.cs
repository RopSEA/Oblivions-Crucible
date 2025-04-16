using System.Collections.Generic;
using UnityEngine;

public class RelicUIManager : MonoBehaviour
{
    public Transform relicContainer;
    public GameObject relicSlotPrefab;
    public List<RelicSO> allRelics;

    [Header("Debug")]
    public bool debugShowAll = false;

    void Start()
    {
        List<string> unlockedRelicNames = new List<string>();

        if (debugShowAll)
        {
            Debug.LogWarning("Debug Mode Enabled: All relics will be shown as unlocked.");
        }
        else if (DataPersistenceManager.instance != null && DataPersistenceManager.instance.HasGameData())
        {
            GameData saveData = DataPersistenceManager.instance.GameData;
            unlockedRelicNames = saveData.relicsAcquired;
            Debug.Log("Save file loaded. Unlocked relics: " + string.Join(", ", unlockedRelicNames));
        }
        else
        {
            Debug.LogWarning("No save file found. Showing all relics as locked.");
        }

        foreach (RelicSO relic in allRelics)
        {
            bool isUnlocked = debugShowAll || unlockedRelicNames.Contains(relic.relicName);

            Debug.Log($"Relic: {relic.relicName} | Unlocked: {isUnlocked}");

            GameObject slot = Instantiate(relicSlotPrefab, relicContainer);
            slot.GetComponent<RelicSlotUI>().Setup(relic, isUnlocked);
        }
    }
}
