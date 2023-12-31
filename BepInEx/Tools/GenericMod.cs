using BepInEx.Configuration;
using System;

namespace JKSFix;

public abstract class GenericMod<T>
{
    public T? Value { get; private set; }

    protected void SetUp(Action<T?> setter, Action<Action<T?>> register, T? @default = default)
    {
        setter(@default);
        register(value => Value = value);
    }

    protected void SetUp(ConfigEntry<T?> cfg, Action<T?> setter, Action<Action<T?>> register)
    {
        setter(Value = cfg.Value);
        register(value => cfg.Value = Value = value);
    }
}
