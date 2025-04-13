using System.Collections.Generic;

public static class RelicUtility
{
    public static float GetTotalEffect(RelicEffectType type)
    {
        float total = 0f;
        foreach (var relic in RelicRunData.instance.selectedRelics)
        {
            foreach (var effect in relic.effects)
            {
                if (effect.effectType == type)
                    total += effect.value;
            }
        }
        return total;
    }

    public static bool HasEffect(RelicEffectType type)
    {
        foreach (var relic in RelicRunData.instance.selectedRelics)
        {
            foreach (var effect in relic.effects)
            {
                if (effect.effectType == type)
                    return true;
            }
        }
        return false;
    }
}
