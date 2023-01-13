using BepInEx.Configuration;
using UnityEngine.InputSystem;

namespace JKSFix;

public static class HotkeyExtensions
{
    public static bool PressedThisFrame(this ConfigEntry<Key>? hotkey)
        => hotkey?.Value is Key key && key.PressedThisFrame();

    public static bool PressedThisFrame(this Key key)
        => key != Key.None && Keyboard.current[key].wasPressedThisFrame;
}
