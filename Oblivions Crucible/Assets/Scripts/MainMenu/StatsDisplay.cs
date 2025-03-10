using UnityEngine;
using UnityEngine.UI;
using TMPro; // Import TextMeshPro namespace
using System.Collections;

public class StatsDisplay : MonoBehaviour
{
    public enum StatType { RoundsBeat, BossesBeat, EnemiesDefeated, DeathCount } // Selectable Stats

    [Header("Stat Selection")]
    public StatType selectedStat; // Dropdown in Inspector

    [Header("UI Elements")]
    public TMP_Text statText; // TextMeshPro Text Field
    public CanvasGroup statsCanvasGroup; 

    private int statValue = 0;

    void Start()
    {
        if (statsCanvasGroup != null)
        {
            statsCanvasGroup.alpha = 0f;
        }

        LoadStatValue();
        UpdateNumberDisplay(statValue);

        // Start fade-in effect after 1 second
        StartCoroutine(FadeInStats(1f, 1f)); 
    }

    void Update()
    {
        LoadStatValue();
        UpdateNumberDisplay(statValue);
    }

    void LoadStatValue()
    {
        if (DataPersistenceManager.instance != null && DataPersistenceManager.instance.HasGameData())
        {
            GameData data = DataPersistenceManager.instance.GameData;

            switch (selectedStat)
            {
                case StatType.RoundsBeat:
                    statValue = data.roundsBeat;
                    break;
                case StatType.BossesBeat:
                    statValue = data.bossesBeat;
                    break;
                case StatType.EnemiesDefeated:
                    statValue = data.enemiesDefeated;
                    break;
                case StatType.DeathCount:
                    statValue = data.deathCount;
                    break;
            }
        }
    }

    void UpdateNumberDisplay(int value)
    {
        if (statText != null)
        {
            statText.text = value.ToString(); // Directly set the text
        }
    }

    IEnumerator FadeInStats(float delay, float duration)
    {
        yield return new WaitForSeconds(delay); // Wait before fading in

        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            statsCanvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsedTime / duration);
            yield return null;
        }

        statsCanvasGroup.alpha = 1f; // Ensure it's fully visible at the end
    }
}
