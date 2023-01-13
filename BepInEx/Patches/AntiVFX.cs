using HarmonyLib;
using System;

namespace JKSFix;

public class AntiVFX : ToggleableMod
{
    public override void SetUp()
    {
        Patches.EffectEnabled += effect =>
        {
            if (!Enabled) return;
            if (effect.name.Contains("_STEP_") ||
                effect.name.Contains("_BUDDYATTACKC_BEAT2"))
            {
                effect.RequestReturnToPoolInstant();
            }
        };
    }
}

public static partial class Patches
{
    public static event Action<PooledEffectController>? EffectEnabled;

    [HarmonyPatch(typeof(PooledEffectController), nameof(PooledEffectController.OnEnable)), HarmonyPostfix]
    public static void EffectOnEnable(PooledEffectController __instance)
    {
        EffectEnabled?.Invoke(__instance);
    }
}
