using BepInEx.Configuration;
using Il2CppInterop.Runtime.Attributes;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace JKSFix;

public class UserInterface : MonoBehaviour
{
    public bool IsShown => _canvas?.activeSelf != false;

    private GameObject? _canvas;
    private bool _cursorWasVisible;
    private ConfigEntry<bool>? _showUiConfig;

    private static ConfigEntry<Key>? _uiHotkey;
    private static ConfigEntry<Key>? _cursorHotkey;

#pragma warning disable IDE0051
    private void Awake()
#pragma warning restore IDE0051
    {
        var prefab = AssetBundle.LoadFromFile(Plugin.CanvasPath)
            .LoadAsset("Canvas")
            .Cast<GameObject>();
        _canvas = Instantiate(prefab, transform);

        foreach (var selectable in _canvas.GetComponentsInChildren<Selectable>())
        {
            {
                Button? casted = selectable.TryCast<Button>();
                if (casted != null)
                    Plugin.Buttons[casted.name] = casted;
            }

            {
                Toggle? casted = selectable.TryCast<Toggle>();
                if (casted != null)
                    Plugin.Toggles[casted.name] = casted;
            }

            {
                Slider? casted = selectable.TryCast<Slider>();
                if (casted != null)
                    Plugin.Sliders[casted.name] = casted;
            }
        }

        foreach (var text in _canvas.GetComponentsInChildren<Text>())
            Plugin.Texts[text.name] = text;
    }

    [HideFromIl2Cpp]
    public void SetUp(ConfigEntry<bool> cfg, ConfigEntry<Key> uiHotkey, ConfigEntry<Key> cursorHotkey)
    {
        _showUiConfig = cfg;
        _uiHotkey = uiHotkey;
        _cursorHotkey = cursorHotkey;

        Plugin.Texts["Footer"].text =
            $"[{_uiHotkey.Value}] Toggle this window\n" +
            $"[{_cursorHotkey.Value}] Toggle mouse cursor";

        ShowUI(cfg.Value);
    }

    public void ShowUI(bool show)
    {
        {
            var cam = FindObjectOfType<App.InGameCamera>();
            if (cam != null)
                cam.enabled = !show;
        }

#pragma warning disable IDE0031
        if (_canvas != null)
            _canvas.SetActive(show);
#pragma warning restore IDE0031

        if (show)
            _cursorWasVisible = Cursor.visible;

        SetCursorState(show || _cursorWasVisible);

        if (_showUiConfig != null)
            _showUiConfig.Value = show;
    }

#pragma warning disable IDE0051
    private void Update()
#pragma warning restore IDE0051
    {
        if (_uiHotkey.PressedThisFrame())
            ShowUI(!IsShown);

        if (_cursorHotkey.PressedThisFrame())
            SetCursorState(!Cursor.visible);
    }

    private static void SetCursorState(bool enabled)
    {
        Cursor.visible = enabled;
        Cursor.lockState = enabled ? CursorLockMode.None : CursorLockMode.Locked;
    }
}
