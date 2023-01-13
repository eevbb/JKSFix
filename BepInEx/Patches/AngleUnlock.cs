using HarmonyLib;

namespace JKSFix;

public static partial class Patches
{
    [HarmonyPatch(typeof(ChapterHomeCameraController), nameof(ChapterHomeCameraController.DeltaUpdate)), HarmonyPrefix]
    public static void AngleUnlock(ChapterHomeCameraController __instance)
    {
        __instance.m_fMaxTiltDegrees = 89;
        __instance.m_fMinTiltDegrees = -89;
        __instance.m_fMinZoom = .5f;
    }

    [HarmonyPatch(typeof(ChapterHomeCameraController), nameof(ChapterHomeCameraController.DeltaUpdateMobile)), HarmonyPrefix]
    public static void AngleUnlockMobile(ChapterHomeCameraController __instance)
        => AngleUnlock(__instance);
}
