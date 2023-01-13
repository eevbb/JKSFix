using BepInEx;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using Il2CppInterop.Runtime.Attributes;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace JKSFix;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BasePlugin
{
    public static readonly string BasePath = Path.Combine("BepInEx", "plugins", "JKSFix");
    public static readonly string CanvasPath = Path.Combine(BasePath, "canvas");

    private static ManualLogSource? LogSource { get; set; }

    [HideFromIl2Cpp]
    public static Dictionary<string, Button> Buttons { get; } = new();

    [HideFromIl2Cpp]
    public static Dictionary<string, Toggle> Toggles { get; } = new();

    [HideFromIl2Cpp]
    public static Dictionary<string, Slider> Sliders { get; } = new();

    [HideFromIl2Cpp]
    public static Dictionary<string, Text> Texts { get; } = new();

    public static bool AdvancingFrame { get; set; }

    public override void Load()
    {
        LogSource = Log;

        Harmony.CreateAndPatchAll(typeof(Patches));

        Config.SaveOnConfigSet = true;

        AddComponent<UserInterface>().SetUp(
            Config.Bind("Main", "ShowUI", true, "Show the mod UI."),
            Config.Bind("Hotkeys", "ShowUI", Key.Delete, "Show the mod UI hotkey."),
            Config.Bind("Hotkeys", "ToggleCursor", Key.End, "Toggle cursor hotkey."));

        AddComponent<FrameAdvance>().SetUp(
            Buttons["FrameAdvance"],
            Config.Bind("Hotkeys", "FrameAdvance", Key.PageDown, "Frame Advance hotkey."));

        new SkirtPhysics().SetUp(
            Config.Bind("Main", "SkirtPhysics", false, "Enhance skirt physics."),
            Toggles["SkirtPhysics"]);

        new AntiVFX().SetUp(
            Config.Bind("Main", "AntiVFX", false, "Supress certain visual effects."),
            Toggles["AntiVFX"]);

        new KeepHPFull().SetUp(
            Config.Bind("Main", "KeepHPFull", false, "Continuously keep HP full."),
            Toggles["KeepHPFull"]);

        new TriggerStun().SetUp(Toggles["TriggerStun"]);

        new DeathAnimation().SetUp(Toggles["DeathAnimation"]);

        new BlackBars().SetUp(
            Config.Bind("Main", "BlackBars", true, "Disable to remove ultrawide black bars."),
            Toggles["BlackBars"]);

        new ChromaticAberration().SetUp(
            Config.Bind("Main", "ChromaticAberration", true, "Chromatic aberration visual effect."),
            Toggles["ChromaticAberration"]);

        new FieldOfView().SetUp(
            Config.Bind("Main", "FieldOfView", 40, "In-game camera field of view."),
            Sliders["FieldOfView"], Texts["FoVDisplay"]);

        Anim.SetUp(Toggles);

        Texts["MainTitle"].text = $"{MyPluginInfo.PLUGIN_NAME} {MyPluginInfo.PLUGIN_VERSION}";

        Log.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
    }

    public static void Print(Il2CppSystem.Object message)
    {
        Debug.Log(message);
        //LogSource?.LogInfo(message);
    }
}
