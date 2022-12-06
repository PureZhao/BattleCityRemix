using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using LitJson;
using Util;
using GameCore;
using DG.Tweening;

public class MapEditor : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private GameObject curEditElement;
    public GameObject[] mapElement;     //brick 0 1 2 3 4 iron 5 6 7 8 9 grass 10 ocean 11 sand 12
    private Dictionary<Vector3, GameObject> map = new Dictionary<Vector3, GameObject>();
    private GameObject canvas;
    private SpriteRenderer mapLimition;
    private SpriteRenderer heart;
    private Dictionary<int, string> ID2Name = new Dictionary<int, string>()
    {
        { 1, "WholeBrick" },
        { 2, "UpBrick" }, 
        { 3, "RightBrick" }, 
        { 4, "DownBrick" }, 
        { 5, "LeftBrick" }, 
        { 6, "WholeIron" }, 
        { 7, "UpIron" }, 
        { 8, "RightIron" }, 
        { 9, "DownIron" }, 
        { 10, "LeftIron" }, 
        { 11, "Grass" }, 
        { 12, "Ocean" }, 
        { 13, "SandLand" }
    };
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        EditMap();
    }

    void EditMap() {
        if (Input.GetKeyDown(KeyCode.W))
        {

            if (transform.position.y == 2.5f)
                return;
            transform.position += Vector3.up * 0.5f;
            curEditElement = null;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {

            if (transform.position.x == 2.5f)
                return;
            transform.position += Vector3.right * 0.5f;
            curEditElement = null;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            if (transform.position.y == -3.5f)
                return;
            transform.position -= Vector3.up * 0.5f;
            curEditElement = null;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            if (transform.position.x == -3.5f)
                return;
            transform.position -= Vector3.right * 0.5f;
            curEditElement = null;
        }
        // 切换地图元素
        if (Input.GetKeyDown(KeyCode.Space)) {
            SwitchMapElement();
        }

        // 保存
        if (Input.GetKeyDown(KeyCode.O)) {
            SaveMap();
            AssetsManager.Instance.LoadGameObject(ResConst.UIMenuCanvas, (obj) =>
            {
                RectTransform menuRect = obj.transform.Find("Menu").GetComponent<RectTransform>();
                GameObject cursor = menuRect.Find("Cursor").gameObject;
                menuRect.sizeDelta = new Vector2(0, -626f);
                menuRect.DOLocalMoveY(0f, 5f).Play().OnComplete(() => cursor.SetActive(true));
                Destroy(transform.root.gameObject);
            });
            
        }
    }

    void SwitchMapElement()
    {
        bool exist = map.TryGetValue(transform.position, out GameObject obj);
        if (exist)
        {            
            string elemntIDStr = obj.name.Substring(0, 2);
            // 先销毁
            Destroy(obj);
            int index = int.Parse(elemntIDStr);
            int length = mapElement.Length;
            if(index >= length)
            {
                // 到头了就移除
                // 置空，下次调用走else
                map.Remove(transform.position);
            }
            else
            {
                // 下一个
                map[transform.position] = Instantiate(mapElement[index], transform.position, Quaternion.identity);
            }
        }
        else
        {
            GameObject element = Instantiate(mapElement[0], transform.position, Quaternion.identity);
            map.Add(transform.position, element);
        }
    }

    void SaveMap()
    {
        
        JsonData data = new JsonData();
        JsonData enemyQuantity = new JsonData()
        {
            ["0"] = 16,
            ["1"] = 2,
            ["2"] = 1,
            ["3"] = 1,
            ["4"] = 0,
            ["5"] = 0,
            ["6"] = 0,
            ["7"] = 0
        };
        data["enemyQuantity"] = enemyQuantity;

        JsonData mapData = new JsonData();

        Dictionary<string, List<JsonData>> mapDict = new Dictionary<string, List<JsonData>>();
        
        foreach (KeyValuePair<Vector3, GameObject> k in map)
        {
            if (k.Key == new Vector3(-0.5f, -3.5f, 0f) || k.Key == new Vector3(-1.5f, -3.5f, 0f) || k.Key == new Vector3(0.5f, -3.5f, 0f) || k.Key == new Vector3(-3.5f, 2.5f, 0f) || k.Key == new Vector3(-0.5f, 2.5f, 0f) || k.Key == new Vector3(2.5f, 2.5f, 0f))
            {
                Destroy(k.Value);
                continue;
            }
            string curElementName = k.Value.name.Substring(0, 2);
            int id = int.Parse(curElementName);
            string elementName = ID2Name[id];
            if (!mapDict.ContainsKey(elementName))
            {
                mapDict.Add(elementName, new List<JsonData>());
            }
            JsonData j = JsonHelper.Vector2Warp(new Vector2(k.Key.x, k.Key.y));
            mapDict[elementName].Add(j);
            Destroy(k.Value);
        }
        // Home
        // LeftBrick,0,-3.5,0
        // RightBrick,-1,-3.5,0
        // DownBrick,-0.25,-3,0
        // DownBrick,-0.75,-3,0
        mapData["Heart"] = JsonHelper.Vector2Warp(new Vector2(-0.5f, -3.5f));
        if (!mapDict.ContainsKey("LeftBrick"))
        {
            mapDict.Add("LeftBrick", new List<JsonData>());
        }
        if (!mapDict.ContainsKey("RightBrick"))
        {
            mapDict.Add("RightBrick", new List<JsonData>());
        }
        if (!mapDict.ContainsKey("DownBrick"))
        {
            mapDict.Add("DownBrick", new List<JsonData>());
        }
        mapDict["LeftBrick"].Add(JsonHelper.Vector2Warp(new Vector2(0, -3.5f)));
        mapDict["RightBrick"].Add(JsonHelper.Vector2Warp(new Vector2(-1, -3.5f)));
        mapDict["DownBrick"].Add(JsonHelper.Vector2Warp(new Vector2(-0.25f, -3f)));
        mapDict["DownBrick"].Add(JsonHelper.Vector2Warp(new Vector2(-0.75f, -3f)));
        foreach (KeyValuePair<string, List<JsonData>> pair in mapDict)
        {
            JsonData d = new JsonData();
            d.SetJsonType(JsonType.Array);
            for (int i = 0; i < pair.Value.Count; i++)
            {
                d.Add(pair.Value[i]);
            }
            mapData[pair.Key] = d;
        }

        data["map"] = mapData;
        FileUtil.CreateDirectory(GlobalConfig.GameCacheDir);
        string jsonFilePath = Path.Combine(GlobalConfig.GameCacheDir, "CustomerMap.json");
        JsonHelper.WriteJson2File(data, jsonFilePath);
    }

    IEnumerator SpriteAnimation()
    {
        Color untransparent = new Color(1, 1, 1, 1);
        Color transparent = new Color(1, 1, 1, 0);
        bool isAppear = true;
        WaitForSeconds waitTime = new WaitForSeconds(0.2f);
        while (true)
        {
            if (isAppear)
            {
                spriteRenderer.color = transparent;
                isAppear = false;
            }
            else
            {
                spriteRenderer.color = untransparent;
                isAppear = true;
            }
            yield return waitTime;
        }
    }

    private void OnEnable()
    {
        StartCoroutine(nameof(SpriteAnimation));
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

}
