using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameCore;
using LitJson;
using Util;
using System.IO;
using System.Linq;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance { get => instance; }
    public GameMode gameMode { get; set; } = GameMode.Single;
    private int curStage = 0;
    private int deadPlayerCount = 0;
    private int player1LastRoundScore = 0;
    private int player2LastRoundScore = 0;
    private int[] player1Killed = new int[5] { 0, 0, 0, 0, 0 };     //N Q P H Tools
    private int[] player2Killed = new int[5] { 0, 0, 0, 0, 0 };
    private int totalKilled = 0;
    private int enemyQuantity = 20;
    private int liveEnemyCount = 0;
    private List<int> enemySpawnSequence = new List<int>();
    private List<Enemy> enemies = new List<Enemy>();
    private List<PlayerControl> players = new List<PlayerControl>();


    IEnumerator GameStart()
    {
        enemySpawnSequence.Clear();
        curStage++;
        if(curStage >= 36)
        {
            curStage = 1;
        }
        
        UIController.Instance.SwitchCurtain(true);
        string mapConfig = Path.Combine(GlobalConfig.GameCacheDir, "CustomerMap.json");
        if (!File.Exists(mapConfig))
        {
            mapConfig = ResConst.LevelConfigDir + curStage.ToString() + ".json";
        }
        yield return new WaitForSeconds(1.1f);
        AssetsManager.Instance.LoadAsset(mapConfig, (obj) =>
        {
            JsonData data = JsonHelper.GetJsonData((TextAsset)obj);
            JsonData enemies = data["enemyQuantity"];
            enemyQuantity = 0;
            List<int> sequence = new List<int>();
            foreach(string key in enemies.Keys)
            {
                int index = int.Parse(key);
                int count = int.Parse(enemies[key].ToString());
                enemyQuantity += count;
                sequence.Add(count, index);
            }
            enemySpawnSequence = sequence;
            UIController.Instance.UpdateUI(UIUpdateType.LevelChange, curStage);
            MapManager.Instance.CreateMap(data["map"]);
        });
        yield return new WaitForSeconds(0.4f);
        yield return new WaitUntil(()=> MapManager.Instance.MapCompleted);
        AudioManager.Play(ResConst.GameStartSound);
        yield return new WaitForSeconds(1f);
        UIController.Instance.UpdateUI(UIUpdateType.OnRoundStart);
        UIController.Instance.SwitchCurtain(false);
        yield return new WaitForSeconds(0.5f);
        for(int i = 0; i < players.Count; i++)
        {
            players[i].Respawn(false);
        }
        yield return new WaitForSeconds(2f);
        StartCoroutine(nameof(EnemySpawn));
    }

    IEnumerator RoundSuccess()
    {
        List<int> playerOne = player1Killed.ToList();
        playerOne.Add(player1LastRoundScore);
        for(int i = 0; i < player1Killed.Length; i++)
        {
            player1LastRoundScore += player1Killed[i] * DataConst.EachElement[i];
        }
        playerOne.Add(player1LastRoundScore);

        List<int> playerTwo = player2Killed.ToList();
        playerTwo.Add(player2LastRoundScore);
        for (int i = 0; i < player2Killed.Length; i++)
        {
            player2LastRoundScore += player2Killed[i] * DataConst.EachElement[i];
        }
        playerTwo.Add(player2LastRoundScore);

        List<object> parameters = ListEx.ConvertToObjectList(playerOne, playerTwo, curStage);
        UIController.Instance.ShowRoundData(parameters);
        yield return new WaitUntil(() => UIController.Instance.RoundCalcCompleted);
        StartCoroutine(nameof(GameStart));
    }

    IEnumerator RoundFail()
    {
        yield break;
    }

    //Enemy Spawn
    IEnumerator EnemySpawn() {
        yield return new WaitUntil(() => enemySpawnSequence.Count != 0);
        enemySpawnSequence.Shuffle();
        UIController.Instance.UpdateUI(UIUpdateType.EnemyInit, enemyQuantity);
        int spawnCount = DataConst.EnemySpawnPoint.Length;
        for (int i = 0; i < enemySpawnSequence.Count; i++)
        {
            yield return new WaitUntil(() => liveEnemyCount < 5);
            liveEnemyCount++;
            int posID = UnityEngine.Random.Range(0, spawnCount);
            int enemyID = enemySpawnSequence[i];
            AssetsManager.Instance.LoadGameObject(ResConst.BornEffect, (obj) =>
            {
                obj.transform.SetPositionAndRotation(DataConst.EnemySpawnPoint[posID], Quaternion.identity);
                UIController.Instance.UpdateUI(UIUpdateType.EnemySpawn);
            });
            yield return new WaitForSeconds(1.5f);
            AssetsManager.Instance.LoadGameObject(ResConst.EnemyPrefab[enemyID], (obj) =>
            {
                obj.transform.SetPositionAndRotation(DataConst.EnemySpawnPoint[posID], Quaternion.identity);
            });
        }
    }

    public void RegisteryPlayer(PlayerControl player)
    {
        players.Add(player);
    }

    public void PlayerLifeClear()
    {
        ++deadPlayerCount;
        if (gameMode == GameMode.Single && deadPlayerCount == 1)
        {
            UIController.Instance.StartCoroutine("GameOver");
        }
        if (gameMode == GameMode.Double && deadPlayerCount == 2)
        {
            UIController.Instance.StartCoroutine("GameOver");
        }
    }

    public void EnemyKilled(int PlayerID, EnemyType enemyType)
    {
        if(enemyType != EnemyType.Other)
        {
            totalKilled++;
            liveEnemyCount--;
        }
        int enemyID = (int)enemyType;
        switch (PlayerID)
        {
            case 1:
                player1Killed[enemyID]++;
                break;
            case 2:
                player2Killed[enemyID]++;
                break;
        }
        if(totalKilled >= enemyQuantity)
        {
            StartCoroutine(nameof(RoundSuccess));
        }
    }

    void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(this);
    }

    private IEnumerator Start()
    {
        yield return new WaitUntil(() =>
        {
            return 
            SceneManager.GetActiveScene().name == "Game"
            && UIController.Instance != null;
        });
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(nameof(GameStart));
    }
}
