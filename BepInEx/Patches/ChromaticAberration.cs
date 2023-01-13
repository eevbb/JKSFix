using HarmonyLib;
using Polaris.URP;
using System;
using System.Linq;
using UnityEngine.Rendering;
using Object = UnityEngine.Object;

namespace JKSFix;

public class ChromaticAberration : ToggleableMod
{
    public static PolarisJKS2DLighting? ExtractFX(Volume volume)
        => volume.name != "Volume"
            ? null
            : volume.profile?.components.E()
                .Select(v => v.TryCast<PolarisJKS2DLighting>())
                .FirstOrDefault(v => v != null);

    public override void SetUp()
    {
        Patches.JKS2DlightingEnable += fx =>
        {
            fx.chromaticAberration.value = Enabled ? 1 : 0;
        };
    }

    public override void OnToggled()
    {
        foreach (var volume in Object.FindObjectsOfType<Volume>())
        {
            var fx = ExtractFX(volume);
            if (fx != null)
                fx.chromaticAberration.value = Enabled ? 1 : 0;
        }
    }
}

public static partial class Patches
{
    public static event Action<PolarisJKS2DLighting>? JKS2DlightingEnable;

    [HarmonyPatch(typeof(Volume), nameof(Volume.OnEnable)), HarmonyPostfix]
    public static void VolumeTest(Volume __instance)
    {
        var fx = ChromaticAberration.ExtractFX(__instance);
        if (fx != null)
            JKS2DlightingEnable?.Invoke(fx);
    }
}
