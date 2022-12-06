using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using LitJson;
using System;
using System.IO;
using System.Text;
using Sirenix.Utilities;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Util;

namespace PureOdinTools
{
    [GlobalConfig(assetPath: "Odin")]
    public class TemporaryTool : GlobalConfig<TemporaryTool>
    {
        [ShowInInspector]
        public List<List<string>> browse = new List<List<string>>();
        public string levelID;
        //public string levelConfigSaveDir = Application.dataPath, "Res/LevelConfig";
        public string levelMapConfigDir = Application.streamingAssetsPath;
        [Button(name: "Export Enemy Mapping", buttonSize: ButtonSizes.Large)]
        public void ExportEnemyMapping()
        {
            Dictionary<int, string> mapping = new Dictionary<int, string>()
            {
                {0, "Normal" },
                {1, "NormalTool"},
                {2, "Quicker"},
                {3, "QuickerTool"},
                {4, "Heavyer"},
                {5, "HeavyerTool"},
                {6, "PretenderYellow"},
                {7, "PretenderGreen"},
            };
            string jsonFilePath = Path.Combine(GlobalConfig.AssetDir, "EnemyIDMapping.json");
            if (File.Exists(jsonFilePath))
            {
                File.Delete(jsonFilePath);
            }
            FileStream fileStream = File.Create(jsonFilePath);
            string jsonString = JsonMapper.ToJson(mapping);
            fileStream.Write(jsonString.ToByteArray(), 0, jsonString.Length);
            fileStream.Close();
            fileStream.Dispose();
        }
        [Button(name: "Convert to Json", buttonSize: ButtonSizes.Large)]
        public void ConvertToJson()
        {
            
            string levelConfigSaveDir = Path.Combine(Application.dataPath, "Res/LevelConfig");
            if (Directory.Exists(levelConfigSaveDir))
            {
                Directory.Delete(levelConfigSaveDir, true);
            }
            Directory.CreateDirectory(levelConfigSaveDir);
            string enemyInfo = File.ReadAllText(Application.streamingAssetsPath + "/EnemyQuantity.txt", Encoding.UTF8);
            
            string[] levels = enemyInfo.Split('\n');
            // 0 Normal
            // 1 NormalTool
            // 2 Quicker
            // 3 QuickerTool
            // 4 Heavyer
            // 5 HeavyerTool
            // 6 PretenderYellow
            // 7 PretenderGreen
            foreach(string level in levels)
            {
                string[] detail = level.Split(',');
                string levelID = detail[0];
                JsonData jsonData = new JsonData();
                JsonData enemyQuantity = new JsonData();
                for(int i = 1; i < detail.Length; i++)
                {
                    enemyQuantity[(i-1).ToString()] = int.Parse(detail[i]);
                }
                jsonData["enemyQuantity"] = enemyQuantity;


                string levelMapElementTxtPath = Path.Combine(Application.streamingAssetsPath, levelID + ".txt");
                JsonData elements = new JsonData();
                
                Dictionary<string, List<JsonData>> t = new Dictionary<string, List<JsonData>>();
                string mapElementTxt = File.ReadAllText(levelMapElementTxtPath);
                string[] eachElement = mapElementTxt.Split('\n');
                for(int i = 0; i < eachElement.Length; i++)
                {
                    string[] items = eachElement[i].Split(',');
                    string key = items[0];
                    float x = float.Parse(items[1]);
                    float y = float.Parse(items[2]);
                    JsonData v2 = JsonHelper.Vector2Warp(new Vector2(x, y));
                    if (!t.ContainsKey(key))
                    {
                        t.Add(key, new List<JsonData>());
                    }
                    t[key].Add(v2);
                }
                if (!t.ContainsKey("LeftBrick"))
                {
                    t.Add("LeftBrick", new List<JsonData>());
                }
                if (!t.ContainsKey("RightBrick"))
                {
                    t.Add("RightBrick", new List<JsonData>());
                }
                if (!t.ContainsKey("DownBrick"))
                {
                    t.Add("DownBrick", new List<JsonData>());
                }
                t["LeftBrick"].Add(JsonHelper.Vector2Warp(new Vector2(0, -3.5f)));
                t["RightBrick"].Add(JsonHelper.Vector2Warp(new Vector2(-1, -3.5f)));
                t["DownBrick"].Add(JsonHelper.Vector2Warp(new Vector2(-0.25f, -3f)));
                t["DownBrick"].Add(JsonHelper.Vector2Warp(new Vector2(-0.75f, -3f)));
                foreach (KeyValuePair<string, List<JsonData>> pair in t)
                {
                    JsonData data = new JsonData();
                    data.SetJsonType(JsonType.Array);
                    for(int i = 0; i < pair.Value.Count; i++)
                    {
                        data.Add(pair.Value[i]);
                    }
                    elements[pair.Key] = data;
                }
                jsonData["map"] = elements;

                string jsonFilePath = Path.Combine(levelConfigSaveDir, levelID + ".json");
                JsonHelper.WriteJson2File(jsonData, jsonFilePath);
            }
        }

        [Button(name: "Map Element", buttonSize: ButtonSizes.Large)]
        public void ExportMapElementStructure()
        {
            Dictionary<string, List<Vector2>> mapElementData = new Dictionary<string, List<Vector2>>()
            {
                {"UpIron", new List<Vector2>(){
                    new Vector2(0.125f, 0.125f),
                    new Vector2(-0.125f, 0.125f)
                } },

                {"UpBrick", new List<Vector2>(){
                    new Vector2(0.125f, 0.125f),
                    new Vector2(-0.125f, 0.125f)
                } },

                {"RightIron", new List<Vector2>(){
                    new Vector2(0.125f, 0.125f),
                    new Vector2(0.125f, -0.125f)
                } },

                {"RightBrick", new List<Vector2>(){
                    new Vector2(0.125f, 0.125f),
                    new Vector2(0.125f, -0.125f)
                } },

                {"DownIron", new List<Vector2>(){
                    new Vector2(-0.125f, -0.125f),
                    new Vector2(0.125f, -0.125f)
                } },

                {"DownBrick", new List<Vector2>(){
                    new Vector2(-0.125f, -0.125f),
                    new Vector2(0.125f, -0.125f)
                } },

                {"LeftIron", new List<Vector2>(){
                    new Vector2(-0.125f, 0.125f),
                    new Vector2(-0.125f, -0.125f)
                } },

                {"LeftBrick", new List<Vector2>(){
                    new Vector2(-0.125f, 0.125f),
                    new Vector2(-0.125f, -0.125f)
                } },

                {"WholeIron", new List<Vector2>(){
                    new Vector2(0.125f, 0.125f),
                    new Vector2(0.125f, -0.125f),
                    new Vector2(-0.125f, 0.125f),
                    new Vector2(-0.125f, -0.125f)
                } },

                {"WholeBrick", new List<Vector2>(){
                    new Vector2(0.125f, 0.125f),
                    new Vector2(0.125f, -0.125f),
                    new Vector2(-0.125f, 0.125f),
                    new Vector2(-0.125f, -0.125f)
                } },
            };

            JsonData data = new JsonData();
            foreach(KeyValuePair<string, List<Vector2>> pair in mapElementData)
            {
                JsonData v = new JsonData();
                v.SetJsonType(JsonType.Array);
                for(int i = 0; i < pair.Value.Count; i++)
                {
                    v.Add(JsonHelper.Vector2Warp(pair.Value[i]));
                }
                data[pair.Key] = v;
            }
            string jsonFilePath = Path.Combine(Application.dataPath, "Res/MapStructure.json");
            JsonHelper.WriteJson2File(data, jsonFilePath);
        }

        [Button(name: "Generate Enemy Config", buttonSize: ButtonSizes.Large)]
        public void GenerateEnemyConfig()
        {
            
        }
    }
}
