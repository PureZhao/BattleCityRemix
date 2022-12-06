using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalConfig
{
    public static string AssetBundleDir { get; } = Application.dataPath + "/../Bundle";
    public static string AssetDir { get; } = Application.dataPath + "/Res";
    public static string GameCacheDir { get; } = Application.dataPath + "../GameCache";
}
