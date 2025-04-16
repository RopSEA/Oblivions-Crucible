using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RelicSlotUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Image iconImage;

    private RelicSO relicData;
    private bool isUnlocked;

    public void Setup(RelicSO relicData, bool isUnlocked)
    {
        this.relicData = relicData;
        this.isUnlocked = isUnlocked;

        iconImage.sprite = isUnlocked ? relicData.icon : relicData.lockedIcon;
        iconImage.color = Color.white;

        Debug.Log($"{relicData.relicName} | Sprite: {iconImage.sprite.name} | Unlocked: {isUnlocked}");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (TooltipManager.instance == null) return;
        TooltipManager.instance.ShowTooltip(relicData, isUnlocked);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (TooltipManager.instance != null)
        {
            TooltipManager.instance.HideTooltip();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!isUnlocked) return;

        if (RelicRunData.instance.HasRelic(relicData.relicName))
        {
            RelicRunData.instance.selectedRelics.Remove(relicData);
        }
        else if (RelicRunData.instance.selectedRelics.Count < 3)
        {
            RelicRunData.instance.selectedRelics.Add(relicData);
        }

        // Update the selected relics text display
        if (RelicSelectionUI.instance != null)
        {
            RelicSelectionUI.instance.UpdateSelectedList();
        }
    }
}
