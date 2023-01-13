using BepInEx.Configuration;
using UnityEngine.Events;
using UnityEngine.UI;

namespace JKSFix;

public abstract class ToggleableMod : GenericMod<bool>
{
    public bool Enabled => Value;

    public void SetUp(Toggle toggle, bool @default = default)
    {
        SetUp(v => toggle.isOn = v, ev => toggle.onValueChanged.AddListener((UnityAction<bool>)ev), @default);
        toggle.onValueChanged.AddListener((UnityAction<bool>)(_ => OnToggled()));
        SetUp();
    }

    public void SetUp(ConfigEntry<bool> cfg, Toggle toggle)
    {
        SetUp(cfg, v => toggle.isOn = v, ev => toggle.onValueChanged.AddListener((UnityAction<bool>)ev));
        toggle.onValueChanged.AddListener((UnityAction<bool>)(_ => OnToggled()));
        SetUp();
    }

    public abstract void SetUp();

    public virtual void OnToggled()
    {
    }
}
