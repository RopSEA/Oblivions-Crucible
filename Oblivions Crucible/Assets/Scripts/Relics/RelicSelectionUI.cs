using UnityEngine;
using TMPro;
using System.Text;

public class RelicSelectionUI : MonoBehaviour
{
    public static RelicSelectionUI instance;
    public TextMeshProUGUI selectedRelicText;

    void Awake()
    {
        instance = this;
    }

    public void UpdateSelectedList()
    {
        if (RelicRunData.instance.selectedRelics.Count == 0)
        {
            selectedRelicText.text = "None";
            return;
        }

        StringBuilder sb = new StringBuilder();

        foreach (var relic in RelicRunData.instance.selectedRelics)
        {
            sb.AppendLine($"• {relic.relicName}");
        }

        selectedRelicText.text = sb.ToString();
    }
}
