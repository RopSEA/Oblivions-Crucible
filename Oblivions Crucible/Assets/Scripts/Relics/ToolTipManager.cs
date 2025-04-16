using UnityEngine;
using TMPro;

public class TooltipManager : MonoBehaviour
{
    public static TooltipManager instance;

    [Header("UI References")]
    public GameObject tooltipPanel;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI infoText;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        HideTooltip();
    }

    public void ShowTooltip(RelicSO relicData, bool isUnlocked)
    {
        tooltipPanel.SetActive(true);

        if (isUnlocked)
        {
            nameText.text = relicData.relicName;
            infoText.text = relicData.description;
        }
        else
        {
            nameText.text = "???";
            string hint = string.IsNullOrEmpty(relicData.unlockHint)
                ? "This relic is locked."
                : relicData.unlockHint;

            infoText.text = $"???\n{hint}";
        }
    }

    public void HideTooltip()
    {
        tooltipPanel.SetActive(false);
    }
}
