using App;
using BepInEx.Configuration;
using HarmonyLib;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace JKSFix;

public class FieldOfView : GenericMod<int>
{
    private Camera? _camera;

    public void SetUp(ConfigEntry<int> cfg, Slider slider, Text display)
    {
        SetUp(cfg, v =>
        {
            slider.value = v;
            display.text = $"{v}";
        }, ev =>
        {
            slider.onValueChanged.AddListener((UnityAction<float>)(value =>
            {
                ev((int)value);
                display.text = $"{Value}";

                if (_camera != null)
                    _camera.fieldOfView = Value;
            }));
        });

        Patches.InGameCameraRegistered += igc =>
        {
            if (_camera == null)
                _camera = igc.GetComponent<Camera>();

            if (_camera != null)
                _camera.fieldOfView = Value;
        };
    }
}

public static partial class Patches
{
    public static event Action<InGameCamera>? InGameCameraRegistered;

    [HarmonyPatch(typeof(InGameCamera), nameof(InGameCamera.Register)), HarmonyPrefix]
    public static void InGameCameraRegister(InGameCamera __instance)
    {
        InGameCameraRegistered?.Invoke(__instance);
    }
}
