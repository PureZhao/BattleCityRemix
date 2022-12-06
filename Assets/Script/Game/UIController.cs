using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;
using GameCore;
using System;

public class UIController : MonoBehaviour
{
    private static UIController instance;
    public static UIController Instance { get => instance; }

    public delegate void OnDataUpdate(params object[] data);
    public event OnDataUpdate OnUpdateUI;

    //Round Start
    public RectTransform upCurtain;
    public RectTransform downCurtain;
    //In Game
    public GameObject player2Info;
    //Round End
    public UICalcScore uiCalcScore;
    //Game Over
    public RectTransform gameOver;
    public GameObject stageOver;

    private bool roundCalcCompleted = false;
    public bool RoundCalcCompleted { get => roundCalcCompleted; }
    public void UpdateUI(params object[] parameters)
    {
        OnUpdateUI(parameters);
    }

    public void SwitchCurtain(bool toClose)
    {
        if (toClose)
        {
            upCurtain.DOLocalMoveY(400f, 1f, true);
            downCurtain.DOLocalMoveY(-400f, 1f, true);
        }
        else
        {
            upCurtain.DOLocalMoveY(800f, 1f, true);
            downCurtain.DOLocalMoveY(-800f, 1f, true);
        }
    }

    public void ShowRoundData(List<object> parameters)
    {
        StartCoroutine(nameof(RoundSuccess), parameters);
    }


    IEnumerator RoundSuccess(List<object> parameters) {
        roundCalcCompleted = false;
        uiCalcScore.CalcRoundData(parameters);
        yield return new WaitUntil(()=> uiCalcScore.CalcCompleted);
        yield return new WaitForSeconds(1f);
        uiCalcScore.gameObject.SetActive(false);
        roundCalcCompleted = true;
    }

    IEnumerator RoundFail() {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject g in players)
        {
            g.GetComponent<PlayerControl>().enabled = false;
            g.GetComponent<AudioSource>().mute = true;
        }
        gameOver.DOLocalMoveY(0, 3f);
        AudioManager.Play(ResConst.GameOverSound);
        yield return new WaitForSeconds(6f);
        SceneManager.LoadScene("Menu");
    }
    IEnumerator StageOver() {
        stageOver.SetActive(true);
        yield return new WaitForSeconds(1f);
        AudioManager.Play(ResConst.GameOverSound);
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("Menu");
    }
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        SetDisplayMode();
    }

    public void SetDisplayMode()
    {
        GameMode mode = GameManager.Instance.gameMode;
        if(mode == GameMode.Single)
        {
            player2Info.SetActive(false);
        }
        else if(mode == GameMode.Double)
        {
            
        }
    }

}
