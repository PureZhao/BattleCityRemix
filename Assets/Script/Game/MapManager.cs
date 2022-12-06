using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameCore;
using LitJson;
using Util;

public class MapManager : MonoBehaviour
{
    private static MapManager instance;
    public static MapManager Instance { get => instance; }
    private List<GameObject> mapElements = new List<GameObject>();
    private Dictionary<string, List<Vector2>> elementStructure = new Dictionary<string, List<Vector2>>();
    private GameObject curLiveTool;
    private int elementCountOfCurStage;
    private int elementCountOfNow;
    public bool MapCompleted { get => elementCountOfCurStage == elementCountOfNow; }


    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }

        AssetsManager.Instance.LoadAsset(ResConst.MapStructure, (obj) =>
        {
            JsonData data = JsonHelper.GetJsonData(obj as TextAsset);
            foreach (string k in data.Keys)
            {
                if (!elementStructure.ContainsKey(k))
                {
                    elementStructure.Add(k, new List<Vector2>());
                }
                JsonData values = data[k];
                for (int i = 0; i < values.Count; i++)
                {
                    JsonData axis = values[i];
                    Vector2 v = JsonHelper.JsonToVector2(axis);
                    elementStructure[k].Add(v);
                }
            }
        });
    }

    public void CreateMap(JsonData mapData)
    {
        elementCountOfCurStage = 0;
        elementCountOfNow = -1;
        // Calc Element Count
        foreach (string k in mapData.Keys)
        {
            JsonData values = mapData[k];
            elementCountOfCurStage += values.Count;
        }
        elementCountOfNow = 0;
        foreach (string k in mapData.Keys)
        {
            JsonData values = mapData[k];
            for (int i = 0; i < values.Count; i++)
            {
                JsonData axis = values[i];
                Vector2 v = JsonHelper.JsonToVector2(axis);
                CreateElement(k, v);
                elementCountOfNow++;
            }
        }
    }

    void CreateElement(string element, Vector2 pos)
    {
        elementStructure.TryGetValue(element, out List<Vector2> offsets);
        switch (element)
        {
            case "Heart":
                CreateElement(ResConst.HeartMapElement, pos, offsets);
                break;
            case "UpIron":
            case "DownIron":
            case "LeftIron":
            case "RightIron":
            case "WholeIron":
                CreateElement(ResConst.IronMapElement, pos, offsets);
                break;
            case "UpBrick":
            case "DownBrick":
            case "LeftBrick":
            case "RightBrick":
            case "WholeBrick":
                CreateElement(ResConst.BrickMapElement, pos, offsets);
                break;
            case "Grass":
                CreateElement(ResConst.GrassMapElement, pos, offsets);
                break;
            case "Ocean":
                CreateElement(ResConst.OceanMapElement, pos, offsets);
                break;
            case "SandLand":
                CreateElement(ResConst.SandLandMapElement, pos, offsets);
                break;
        }
    }

    void CreateElement(string path, Vector2 pos, List<Vector2> offsets)
    {
        if(offsets == null)
        {
            AssetsManager.Instance.LoadGameObject(path, (obj) =>
            {
                obj.transform.position = pos;
                mapElements.Add(obj);
            });
        }
        else
        {
            foreach(Vector2 v in offsets)
            {
                AssetsManager.Instance.LoadGameObject(path, (obj) =>
                {
                    obj.transform.position = pos + v;
                    mapElements.Add(obj);
                });
            }
        }
    }
    public void ClearMap()
    {
        for(int i = 0; i < mapElements.Count; i++)
        {
            if (mapElements[i] != null)
            {
                AssetsManager.Instance.FreeObject(mapElements[i]);
            }
        }
        mapElements.Clear();
    }

    public void ProtectHome()
    {
        // LeftBrick,0,-3.5,0
        // RightBrick,-1,-3.5,0
        // DownBrick,-0.25,-3,0
        // DownBrick,-0.75,-3,0
        StartCoroutine(nameof(ProtectHeart));
    }
    IEnumerator ProtectHeart()
    {
        List<GameObject> irons = new List<GameObject>();
        List<string> elements = new List<string>()
        {
            "LeftIron", "RightIron", "DownIron", "DownIron"
        };
        List<Vector2> elementsPos = new List<Vector2>()
        {
            new Vector2(0f, -3.5f),
            new Vector2(-1f, -3.5f),
            new Vector2(-0.25f, -3f),
            new Vector2(-0.75f, -3f)
        };

        for(int i = 0; i < 4; i++)
        {
            string elementName = elements[i];
            Vector2 elementPos = elementsPos[i];
            elementStructure.TryGetValue(elementName, out List<Vector2> offsets);
            if (offsets == null)
            {
                AssetsManager.Instance.LoadGameObject(ResConst.IronMapElement, (obj) =>
                {
                    obj.transform.position = elementPos;
                    irons.Add(obj);
                });
            }
            else
            {
                foreach (Vector2 v in offsets)
                {
                    AssetsManager.Instance.LoadGameObject(ResConst.IronMapElement, (obj) =>
                    {
                        obj.transform.position = elementPos + v;
                        irons.Add(obj);
                    });
                }
            }
        }
        yield return new WaitForSeconds(5f);
        for(int i = 0; i < 10; i++)
        {
            foreach (GameObject g in irons)
            {
                if (g.TryGetComponent(out SpriteRenderer renderer))
                {
                    renderer.enabled = !renderer.enabled;
                }
            }
            yield return new WaitForSeconds(0.2f);
        }
        foreach (GameObject g in irons)
        {
            AssetsManager.Instance.FreeObject(g, false);
        }
        irons.Clear();
    }
    //Tool
    public void CreateTool()
    {
        float x = 2.5f - Random.Range(0, 24) * 0.25f;
        float y = 0f;
        while (true)
        {
            if (x < 0f && x > -1f)
            {
                y = 2.5f - Random.Range(0, 24) * 0.25f;
                if (y >= -3f)
                {
                    break;
                }
                else
                {
                    continue;
                }
            }
            else
            {
                y = 2.5f - Random.Range(0, 24) * 0.25f;
                break;
            }
        }
        int id = Random.Range(0, 6);
        if(curLiveTool != null)
        {
            AssetsManager.Instance.FreeObject(curLiveTool);
            curLiveTool = null;
        }
        Vector3 appearPos = new Vector3(x, y, 0f);

        AudioManager.Play(ResConst.BonusAppearSound);
        AssetsManager.Instance.LoadGameObject(ResConst.Tools[id], (obj) =>
        {
            obj.transform.SetPositionAndRotation(appearPos, Quaternion.identity);
            curLiveTool = obj;
        });
    }

    private void OnDestroy()
    {
        mapElements.Clear();
    }
}
