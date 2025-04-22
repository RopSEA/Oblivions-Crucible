using System.Collections.Generic;
using UnityEngine;

public class RelicUIManager : MonoBehaviour
{
    public Transform relicContainer;
    public GameObject relicSlotPrefab;
    public List<RelicSO> allRelics;

    [Header("Debug")]
    public bool debugShowAll = false;
    public bool debugHalfUnlocked = false;

        void Start()
    {
        List<string> unlockedRelicNames = new List<string>();

        if (debugShowAll)
        {
            Debug.LogWarning("Debug Mode Enabled: All relics will be shown as unlocked.");
        }
        else if (debugHalfUnlocked)
        {
            Debug.LogWarning("Debug Mode Enabled: Half of the relics will be shown as unlocked.");
            int halfCount = allRelics.Count / 2;
            for (int i = 0; i < halfCount; i++)
            {
                unlockedRelicNames.Add(allRelics[i].relicName);
            }
        }
        else if (DataPersistenceManager.instance != null && DataPersistenceManager.instance.HasGameData())
        {
            // FIRST: Check for new stat-based unlocks
            CheckStatBasedUnlocks();

            // THEN: Grab the updated list from the save file
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

        void CheckStatBasedUnlocks()
    {
        if (DataPersistenceManager.instance == null || !DataPersistenceManager.instance.HasGameData()) return;

        GameData data = DataPersistenceManager.instance.GameData;
        List<string> unlockedRelics = data.relicsAcquired;

        // Unlock conditions
        TryUnlock("Amulet of the Warlord", data.enemiesDefeated >= 200, unlockedRelics);
        TryUnlock("Saint’s Embrace", data.deathCount >= 50, unlockedRelics);
        TryUnlock("Stoneguard Amulet", data.deathCount >= 5, unlockedRelics);
        TryUnlock("Eye of Pi", data.bossesBeat >= 4, unlockedRelics);

        DataPersistenceManager.instance.SaveGame();
    }

    void TryUnlock(string relicName, bool condition, List<string> unlockedRelics)
    {
        if (condition && !unlockedRelics.Contains(relicName))
        {
            unlockedRelics.Add(relicName);
            Debug.Log($"Unlocked relic: {relicName} via stat condition!");
        }
    }
}
