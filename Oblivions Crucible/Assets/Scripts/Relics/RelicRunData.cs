using System.Collections.Generic;
using UnityEngine;

public class RelicRunData : MonoBehaviour
{
    public static RelicRunData instance;

    public List<RelicSO> selectedRelics = new List<RelicSO>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

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


    public void AddStats(Classes player)
    {
        foreach (RelicSO rel in selectedRelics)
        {
            if (rel != null)
            {
                foreach (var relEff in rel.effects)
                {
                    if (relEff.effectType == RelicEffectType.StrengthBoost)
                    {
                        player.addStren((int)relEff.value);
                    }
                    if (relEff.effectType == RelicEffectType.StaminaBoost)
                    {
                        player.addStam((int)relEff.value);
                    }
                    if (relEff.effectType == RelicEffectType.IntelligenceBoost)
                    {
                        player.addIntell((int)relEff.value);
                    }
                    if (relEff.effectType == RelicEffectType.DefenseBoost)
                    {
                        player.addDef((int)relEff.value);
                    }
                    if (relEff.effectType == RelicEffectType.VitalityBoost)
                    {
                        player.addVit((int)relEff.value);
                    }
                    if (relEff.effectType == RelicEffectType.HealthBonus)
                    {
                        player.addVit((int)relEff.value);
                    }
                    if (relEff.effectType == RelicEffectType.DamageMultiplier)
                    {
                        player.addDmgMult(relEff.value);
                    }
                    if (relEff.effectType == RelicEffectType.Lifesteal)
                    {
                        player.Lifesteal = relEff.value;
                    }
                    if (relEff.effectType == RelicEffectType.ReviveOnce)
                    {
                        player.ReviveOnce = relEff.value;
                    }
                }
            }
                
        }
    }

    public bool HasRelic(string name) =>
        selectedRelics.Exists(r => r.relicName == name);
}