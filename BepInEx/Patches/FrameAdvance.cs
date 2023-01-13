using BepInEx.Configuration;
using Il2CppInterop.Runtime.Attributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace JKSFix;

public class FrameAdvance : MonoBehaviour
{
    private Button? _button;

    public static bool IsAdvancing => _advancing > 0;
    private static int _advancing = 0;

    private static ConfigEntry<Key>? _hotkey;

    [HideFromIl2Cpp]
    public void SetUp(Button button, ConfigEntry<Key> hotkey)
    {
        _button = button;
        _hotkey = hotkey;

        _button.onClick.AddListener((UnityAction)Trigger);
        Plugin.Texts["FrameAdvanceLabel"].text = $"[{_hotkey.Value}] Frame Advance";
    }

    private void Trigger()
    {
        if (Time.timeScale == 0)
        {
            Plugin.AdvancingFrame = true;
            _advancing = 2;
        }
    }

#pragma warning disable IDE0051
    private void Update()
#pragma warning restore IDE0051
    {
        if (_hotkey.PressedThisFrame())
            Trigger();

        if (_advancing == 2)
            Time.timeScale = 1;

        if (_advancing == 1)
            Time.timeScale = 0;

        if (_advancing > 0)
            _advancing--;

        if (_button != null)
            _button.interactable = Time.timeScale == 0;
    }
}
