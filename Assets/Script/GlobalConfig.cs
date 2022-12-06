using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalConfig
{
    public static string Version { get; } = "v0.0.2";
    public static string AssetBundleDir { get; } = Application.dataPath + "/../Bundle";
    public static string AssetDir { get; } = Application.dataPath + "/Res";
    public static string GameCacheDir { get; } = Application.dataPath + "/../GameCache";
    public static string TestBundleDir { get; } = Application.dataPath + "/../TestBundle";
    

    public static string AssetBundleServerUrlFromGitee { get; } = "https://gitee.com/purezhao/BattleCityRemix/raw/main/Bundle/";
    public static string AssetBundleServerUrlFromGithub { get; } = "https://raw.githubusercontent.com/PureZhao/BattleCityRemix/blob/main/Bundle/";
}
