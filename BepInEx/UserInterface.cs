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
        var ab = AssetBundle.LoadFromFile(Plugin.CanvasPath);

        {
            // Keep an inactive instance so it doesn't unload
            var togglePrefab = Instantiate(ab.LoadAsset("Toggle").Cast<GameObject>(), transform);
            togglePrefab.SetActive(false);
            Plugin.Prefabs["Toggle"] = togglePrefab;
        }

        _canvas = Instantiate(ab
            .LoadAsset("Canvas")
            .Cast<GameObject>(), transform);

        foreach (var t in _canvas.GetComponentsInChildren<Transform>(true))
        {
            if (t.name.EndsWith("Panel"))
                Plugin.Panels[t.name] = t.gameObject;

            if (t.GetComponent<Button>() is Button button)
                Plugin.Buttons[t.name] = button;

            if (t.GetComponent<Toggle>() is Toggle toggle)
                Plugin.Toggles[t.name] = toggle;

            if (t.GetComponent<Slider>() is Slider slider)
                Plugin.Sliders[t.name] = slider;

            if (t.GetComponent<Text>() is Text text)
                Plugin.Texts[t.name] = text;

            if (t.GetComponent<ScrollRect>() is ScrollRect scrollRect)
                Plugin.ScrollRects[t.name] = scrollRect;
        }
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
