using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewRelic", menuName = "Relics/Relic")]
public class RelicSO : ScriptableObject
{
    public string relicName;
    [TextArea(2, 5)]
    public string description;
    public Sprite icon;
    public Sprite lockedIcon;
    public string unlockHint;
    public List<RelicEffect> effects = new List<RelicEffect>();
}