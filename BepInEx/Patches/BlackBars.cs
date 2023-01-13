using HarmonyLib;
using System;
using Object = UnityEngine.Object;

namespace JKSFix;

public class BlackBars : ToggleableMod
{
    public static bool IsBlackBars(UXScreenBorder border)
        => border.name.StartsWith("CommonScreenBorder");

    public override void SetUp()
    {
        Patches.BlackBarsCreate += border =>
        {
            if (!Enabled)
                border.gameObject.SetActive(false);
        };
    }

    public override void OnToggled()
    {
        foreach (var border in Object.FindObjectsOfType<UXScreenBorder>(true))
            if (IsBlackBars(border))
                border.gameObject.SetActive(Enabled);
    }
}

public static partial class Patches
{
    public static event Action<UXScreenBorder>? BlackBarsCreate;

    [HarmonyPatch(typeof(UXFormattedComponent), nameof(UXFormattedComponent.Create)), HarmonyPostfix]
    public static void UXFormattedComponentCreate(UXFormattedComponent __instance)
    {
        var sb = __instance.TryCast<UXScreenBorder>();
        if (sb != null && BlackBars.IsBlackBars(sb))
            BlackBarsCreate?.Invoke(sb);
    }
}
