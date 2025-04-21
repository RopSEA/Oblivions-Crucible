using UnityEngine;
using System.Collections.Generic;

public class RelicCatalog : MonoBehaviour
{
    public static RelicCatalog instance;

    public List<RelicSO> allRelics = new List<RelicSO>();

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        // Load all relics from Resources/Relics
        RelicSO[] loadedRelics = Resources.LoadAll<RelicSO>("RelicsSOs");
        allRelics = new List<RelicSO>(loadedRelics);
    }

    public RelicSO GetRelicByName(string name)
    {
        return allRelics.Find(r => r.relicName == name);
    }
}
