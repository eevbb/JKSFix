using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JKSFix;

public class SkirtPhysics : ToggleableMod
{
    public static bool IsSkirt(PhysicsBone pb)
        => pb.m_rootBones.Any() && pb.m_rootBones[0].name.Contains("_skirt_");

    private static bool IsEnabled(PhysicsBone pb)
        => BoneDict.TryGetValue(pb.GetInstanceID(), out var val) && val;

    private static readonly Dictionary<int, bool> BoneDict = new();

    private static void Enable(PhysicsBone pb)
    {
        if (!IsSkirt(pb) || IsEnabled(pb)) return;

        BoneDict[pb.GetInstanceID()] = true;

        var cb = pb.m_childBones;
        for (int i = 0; i < cb.Length; i++)
            cb[i] = cb[i].Prepend(cb[i][0].parent).ToArray();
    }

    private static void Disable(PhysicsBone pb)
    {
        if (!IsSkirt(pb) || !IsEnabled(pb)) return;
        BoneDict.Remove(pb.GetInstanceID());

        var cb = pb.m_childBones;
        for (int i = 0; i < cb.Length; i++)
            cb[i] = cb[i].Skip(1).ToArray();
    }

    public override void SetUp()
    {
        Patches.SkirtBoneUpdate += skirt =>
        {
            if (Enabled)
                Enable(skirt);
            else
                Disable(skirt);
        };
    }
}

public static partial class Patches
{
    public static event Action<PhysicsBone>? SkirtBoneUpdate;

    [HarmonyPatch(typeof(PhysicsBone), nameof(PhysicsBone.LateUpdate)), HarmonyPostfix]
    public static void PhysicsBoneLateUpdate(PhysicsBone __instance)
    {
        if (SkirtPhysics.IsSkirt(__instance))
            SkirtBoneUpdate?.Invoke(__instance);
    }
}
