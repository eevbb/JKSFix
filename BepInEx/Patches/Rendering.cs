using App;
using BepInEx.Configuration;
using HarmonyLib;
using Il2CppInterop.Runtime.Attributes;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace JKSFix;

public class Rendering : MonoBehaviour
{
    private static void LoadFloat(ConfigFile config, string key, Action<float> setter)
    {
        var value = config.Bind("Rendering", key, -1f).Value;
        if (value >= 0)
            setter(value);
    }

    private static void LoadInt(ConfigFile config, string key, Action<int> setter)
    {
        var value = config.Bind("Rendering", key, -1).Value;
        if (value >= 0)
            setter(value);
    }

    private static void LoadGlobalSettings(ConfigFile config)
    {
        LoadInt(config, "AnisotropicFiltering", v => QualitySettings.anisotropicFiltering = (AnisotropicFiltering)v);
        LoadInt(config, "AnisotropicFilteringLevel", v => Texture.SetGlobalAnisotropicFilteringLimits(v, v));
        LoadInt(config, "MasterTextureLimit", v => Texture.masterTextureLimit = v);
        LoadInt(config, "MaximumLODLevel", v => QualitySettings.maximumLODLevel = v);
        LoadFloat(config, "LODBias", v => QualitySettings.lodBias = v);

        if (QualitySettings.renderPipeline.TryCast<UniversalRenderPipelineAsset>() is UniversalRenderPipelineAsset asset)
        {
            LoadFloat(config, "RenderScale", v => asset.renderScale = v);
            LoadInt(config, "MSAASampleCount", v => asset.msaaSampleCount = v);
            LoadInt(config, "ShadowCascadeCount", v => asset.shadowCascadeCount = v);
        }
    }

    private static Camera? _lastCam = null;
    private static void LoadCameraSettings(ConfigFile config, bool force = false)
    {
        var mainCam = Camera.main;
        if (mainCam == null || (mainCam == _lastCam && !force)) return;

        var data = mainCam.GetComponent<UniversalAdditionalCameraData>();
        if (data == null) return;

        _lastCam = mainCam;

        LoadInt(config, "AntialiasingMode", v => data.antialiasing = (AntialiasingMode)v);
        LoadInt(config, "RenderPostProcessing", v => data.renderPostProcessing = v == 1);
    }

    private ConfigFile? _config;
    private ConfigEntry<Key>? _hotkey;
    private float _counter = 0;
    private const float Interval = 2;

    [HideFromIl2Cpp]
    public void SetUp(ConfigFile config, ConfigEntry<Key> hotkey)
    {
        _config = config;
        _hotkey = hotkey;

        // Initial load
        Rendering.LoadGlobalSettings(_config);
        Rendering.LoadCameraSettings(_config);
    }

    private void Update()
    {
        if (_config == null)
            return;

        if (_hotkey.PressedThisFrame())
        {
            _config.Reload();
            Rendering.LoadGlobalSettings(_config);
            Rendering.LoadCameraSettings(_config, true);
        }

        // Reload, in case main camera changed
        _counter += Time.deltaTime;
        if (_counter >= Interval)
        {
            _counter -= Interval;
            Rendering.LoadCameraSettings(_config);
        }
    }
}
