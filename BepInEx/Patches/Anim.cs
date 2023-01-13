using HarmonyLib;
using Il2CppInterop.Runtime.Attributes;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static App.SecondaryAnimation;

namespace JKSFix;

public static class Anim
{
    public enum MeshType
    {
        Accessory, Costume, Eyes, Face, Hair, ItemH, ItemL, ItemR, Underwear, Skin, Censor,
    }

    public static GameObject? GetMesh(SecondaryAnimation_char src, MeshType type) => type switch
    {
        MeshType.Accessory => src.meshAcce,
        MeshType.Costume => src.meshCstm,
        MeshType.Eyes => src.meshEyes,
        MeshType.Face => src.meshFace,
        MeshType.Hair => src.meshHair,
        MeshType.ItemH => src.meshItemH,
        MeshType.ItemL => src.meshItemL,
        MeshType.ItemR => src.meshItemR,
        MeshType.Underwear => src.meshLing,
        MeshType.Skin => src.meshSkin,
        MeshType.Censor => src.meshHide,
        _ => null,
    };

    public const string EffectNameDirt = "Dirt";
    public const string EffectNameDamage = "Dmg";
    public const string EffectNameSweat = "Wet";
    public const string EffectNameTear = "Tear";

    private static int Toggled { get; set; }
    public static bool ToggleNow => Toggled == Time.frameCount;

    [HideFromIl2Cpp]
    public static Dictionary<MeshType, Toggle> Toggles { get; } = new();

    [HideFromIl2Cpp]
    public static Dictionary<string, int> EffectValues { get; } = new();

    [HideFromIl2Cpp]
    public static EffectFacialExpression? FacialExpression { get; set; }

    [HideFromIl2Cpp]
    public static void SetUp(Dictionary<string, Toggle> toggles)
    {
        // Effects
        {
            foreach (var name in new string[] { EffectNameDirt, EffectNameDamage, EffectNameSweat, EffectNameTear })
            {
                toggles[$"{name}Clear"].isOn = true;

                foreach (var (levelName, value) in new (string, int)[]
                {
                    ("Clear", -1),
                    ("Default", 0),
                    ("None", 1),
                    ("Low", 2),
                    ("High", 3),
                })
                {
                    toggles[$"{name}{levelName}"].onValueChanged.AddListener((UnityAction<bool>)(isOn =>
                    {
                        if (isOn)
                            EffectValues[name] = value;
                    }));
                }
            }
        }

        // Facial expression
        {
            UnityAction<bool> faceSetter(EffectFacialExpression? face) => (UnityAction<bool>)(isOn =>
            {
                if (isOn)
                    FacialExpression = face;
            });

            toggles["FaceClear"].isOn = true;
            toggles["FaceClear"].onValueChanged.AddListener(faceSetter(null));
            toggles["FaceBaffling"].onValueChanged.AddListener(faceSetter(EffectFacialExpression.Baffling));
            toggles["FaceJoy"].onValueChanged.AddListener(faceSetter(EffectFacialExpression.Joy));
            toggles["FaceAnger"].onValueChanged.AddListener(faceSetter(EffectFacialExpression.Anger));
            toggles["FaceSorrow"].onValueChanged.AddListener(faceSetter(EffectFacialExpression.Sorrow));
            toggles["FaceFun"].onValueChanged.AddListener(faceSetter(EffectFacialExpression.Fun));
            toggles["FaceSurprise"].onValueChanged.AddListener(faceSetter(EffectFacialExpression.Surprise));
            toggles["FaceSerious"].onValueChanged.AddListener(faceSetter(EffectFacialExpression.Serious));
            toggles["FaceWeary"].onValueChanged.AddListener(faceSetter(EffectFacialExpression.Weary));
            toggles["FaceDisappointment"].onValueChanged.AddListener(faceSetter(EffectFacialExpression.Disappointment));
            toggles["FaceFear"].onValueChanged.AddListener(faceSetter(EffectFacialExpression.Fear));
        }

        // Mesh toggle
        {
            Toggles.Clear();
            Toggles[MeshType.Accessory] = toggles["MeshAcce"];
            Toggles[MeshType.Costume] = toggles["MeshCstm"];
            Toggles[MeshType.Eyes] = toggles["MeshEyes"];
            Toggles[MeshType.Face] = toggles["MeshFace"];
            Toggles[MeshType.Hair] = toggles["MeshHair"];
            Toggles[MeshType.ItemH] = toggles["MeshItemH"];
            Toggles[MeshType.ItemL] = toggles["MeshItemL"];
            Toggles[MeshType.ItemR] = toggles["MeshItemR"];
            Toggles[MeshType.Underwear] = toggles["MeshLing"];
            Toggles[MeshType.Skin] = toggles["MeshSkin"];

            foreach (var kv in Toggles)
            {
                kv.Value.isOn = true;
                kv.Value.onValueChanged.AddListener((UnityAction<bool>)(value =>
                {
                    if (Time.timeScale == 0)
                        return;

                    Toggled = Time.frameCount;
                }));
            }
        }
    }
}

public static partial class Patches
{
    [HarmonyPatch(typeof(SecondaryAnimation_char), nameof(SecondaryAnimation_char.Update)), HarmonyPrefix]
    public static void AnimUpdate(SecondaryAnimation_char __instance)
    {
        var c = __instance;

        // Effects
        foreach (var (name, setter) in new (string, Action<SecondaryAnimation_char, int>)[]
        {
            (Anim.EffectNameDirt, (c, v) => c.effectDirt = (EffectDirt)v),
            (Anim.EffectNameDamage, (c, v) => c.effectDamage = (EffectDamage)v),
            (Anim.EffectNameSweat, (c, v) => c.effectSweat = (EffectSweat)v),
            (Anim.EffectNameTear, (c, v) => c.effectTear = (EffectTear)v),
        })
        {
            if (Anim.EffectValues.TryGetValue(name, out var value) && value >= 0)
                setter(c, value);
        }

        // Facial expression
        if (Anim.FacialExpression.HasValue)
            c.effectFacialExpression = Anim.FacialExpression.Value;

        // Insta
        if (Time.timeScale == 0)
        {
            // Effects
            {
                c.facial_effect_damage_val = 1;
                c.facial_effect_damage_mag =
                c.facial_effect_damage_st =
                    c.facial_effect_damage_ed;

                c.facial_effect_dirt_val = 1;
                c.facial_effect_dirt_mag =
                c.facial_effect_dirt_st =
                    c.facial_effect_dirt_ed;

                c.facial_effect_sweat1_val = 1;
                c.facial_effect_sweat1_mag =
                c.facial_effect_sweat1_st =
                    c.facial_effect_sweat1_ed;

                c.facial_effect_sweat2_val = 1;
                c.facial_effect_sweat2_mag =
                c.facial_effect_sweat2_st =
                    c.facial_effect_sweat2_ed;

                c.facial_effect_tear_mag =
                c.facial_effect_tear_st =
                    c.facial_effect_tear_ed;
            }

            // Face parts
            //{
            //    c.facial_effect_blush_mag =
            //    c.facial_effect_blush_st =
            //        c.facial_effect_blush_ed;

            //    c.facial_effect_dark_mag =
            //    c.facial_effect_dark_st =
            //        c.facial_effect_dark_ed;

            //    c.facial_effect_excited_mag =
            //    c.facial_effect_excited_st =
            //        c.facial_effect_excited_ed;

            //    c.facial_effect_eyesAnim_mag =
            //    c.facial_effect_eyesAnim_st =
            //        c.facial_effect_eyesAnim_ed;
            //}
        }

        // Mesh toggle
        if (Time.timeScale == 0 || Anim.ToggleNow || FrameAdvance.IsAdvancing)
        {
            c.isVisible = true;

            foreach (Anim.MeshType meshType in Enum.GetValues(typeof(Anim.MeshType)))
            {
                var mesh = Anim.GetMesh(c, meshType);
                if (mesh != null && Anim.Toggles.TryGetValue(meshType, out var toggle))
                    mesh.SetActive(toggle.isOn);
            }

#pragma warning disable IDE0031
            if (c.meshHide != null)
                c.meshHide.SetActive(false);
#pragma warning restore IDE0031
        }
    }
}
