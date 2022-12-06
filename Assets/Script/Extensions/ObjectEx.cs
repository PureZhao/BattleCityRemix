using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ObjectEx
{
    public static bool IsNull(this UnityEngine.Object obj)
    {
        return obj == null;
    }
}
