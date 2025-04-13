using System.Collections.Generic;
using UnityEngine;

public class RelicRunData
{
    public static RelicRunData instance = new RelicRunData();

    public List<RelicSO> selectedRelics = new List<RelicSO>();

    public void InitializeRun(List<string> relicNames)
    {
        selectedRelics.Clear();
        foreach (var name in relicNames)
        {
            RelicSO relic = RelicCatalog.instance.GetRelicByName(name);
            if (relic != null)
                selectedRelics.Add(relic);
        }
    }

    public bool HasRelic(string name) =>
        selectedRelics.Exists(r => r.relicName == name);
}
