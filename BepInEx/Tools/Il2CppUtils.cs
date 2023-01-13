using System.Collections.Generic;

namespace JKSFix;

public static class Il2CppUtils
{
    public static IEnumerable<T> E<T>(this Il2CppSystem.Collections.Generic.List<T> list)
    {
        foreach (var i in list)
            yield return i;
    }
}
