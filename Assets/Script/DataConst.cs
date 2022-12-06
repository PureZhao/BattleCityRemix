using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameCore;

public static class DataConst
{
    public static Vector3 Player1SpanwPoint { get; } = new Vector3(-1.5f, -3.5f, 0f);
    public static Vector3 Player2SpanwPoint { get; } = new Vector3(0.5f, -3.5f, 0f);
    public static int[] EachElement { get; } = new int[5] { 100, 200, 300, 400, 500 };
    public static Vector3[] EnemySpawnPoint { get; } = new Vector3[3] { new Vector3(-3.5f, 2.5f, 0f), new Vector3(-0.5f, 2.5f, 0f), new Vector3(2.5f, 2.5f, 0f) };
}
