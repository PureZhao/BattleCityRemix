using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
//此脚本放在Editor文件夹下，主要是对ProjectSettings/TagManager.asset这个预设资源的各种tag layer进行设置
[InitializeOnLoad]
public class TagManagerTools : MonoBehaviour
{
    //初始化SortingLayers
    [MenuItem("Tools/Pure/ClearSortingLayers")]
    public static void ClearSortingLayers()
    {
        SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
        SerializedProperty it = tagManager.GetIterator();
        while (it.NextVisible(true))
        {
            if (it.name == "m_SortingLayers")
            {
                it.ClearArray();
                tagManager.ApplyModifiedProperties();
                return;
            }
        }
    }
    //添加你的layers(按从上到下顺序写入)
    [MenuItem("Tools/Pure/AddSortingLayers")]
    public static void AddSortingLayers()
    {
        //这里书写自己的layer名字
        string[] layerNames = { "Grass", "Player","Bonus", "SeaSand" };
        foreach (string item in layerNames)
        {
            AddSortingLayer(item);
        }
    }

    static void AddSortingLayer(string layerName)
    {
        SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
        SerializedProperty it = tagManager.GetIterator();
        while (it.NextVisible(true))
        {
            if (it.name == "m_SortingLayers")
            {
                it.InsertArrayElementAtIndex(it.arraySize);
                SerializedProperty dataPoint = it.GetArrayElementAtIndex(it.arraySize - 1);
                while (dataPoint.NextVisible(true))
                {
                    if (dataPoint.name == "name")
                    {
                        dataPoint.stringValue = layerName;
                        tagManager.ApplyModifiedProperties();
                        return;
                    }
                }


            }
        }
    }

    //检查添加的SortingLayer
    [MenuItem("Tools/Pure/CheckSortingLayers")]
    public static void ReadSortingLayers()
    {
        SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
        SerializedProperty it = tagManager.GetIterator();
        while (it.NextVisible(true))
        {
            if (it.name == "m_SortingLayers")
            {
                var count = it.arraySize;
                for (int i = 0; i < count; i++)
                {
                    var dataPoint = it.GetArrayElementAtIndex(i);
                    while (dataPoint.NextVisible(true))
                    {
                        if (dataPoint.name == "name")
                        {
                            Debug.Log(dataPoint.stringValue);
                        }
                    }
                }
                return;
            }
        }
    }
}
