using App.ActorCartridge;
using HarmonyLib;
using System;

namespace JKSFix;

public static partial class Patches
{
    public static event Action<Actor>? TumuUpdate;

    [HarmonyPatch(typeof(ActorController), nameof(ActorController._lateFixedUpdate)), HarmonyPrefix]
    public static void ActorControllerUpdate(ActorController __instance)
    {
        if (__instance.name.Contains("_tumu_"))
            TumuUpdate?.Invoke(__instance.actor);
    }
}
