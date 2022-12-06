using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using System.Text;
using GameCore;
using System;

public class OptionCursor : MonoBehaviour
{
    private Vector3[] pos = new Vector3[3] { new Vector3(-200f, -140f, 0f), new Vector3(-200f, -210f, 0f), new Vector3(-200f, -280f, 0f) };      //选择光标位置
    private Image cursorImage;
    public Sprite[] cursorSprite;
    private float spriteSwitchTimer = 0.2f;
    private int curSprite = 0;
    void Awake() {
        cursorImage = GetComponent<Image>();
    }

    void ChangeOption() {
        int curMode = (int)GameManager.Instance.gameMode;
        int length = Enum.GetNames(typeof(GameMode)).Length;

        if (Input.GetKeyDown(KeyCode.W)) {
            curMode--;
            if(curMode < 0)
            {
                curMode = length - 1;
            }
        }
        else if (Input.GetKeyDown(KeyCode.S)) {
            curMode++;
            if(curMode >= length)
            {
                curMode = 0;
            }
        }
        GameManager.Instance.gameMode = (GameMode)curMode;
        GetComponent<RectTransform>().localPosition = pos[curMode];
        if (Input.GetKeyDown(KeyCode.Space)) {
            
            DoChoice();
        }
    }

    IEnumerator SpriteAnimation()
    {
        WaitForSeconds waitTime = new WaitForSeconds(0.2f);
        int curSprite = 0;
        for(; ; )
        {
            cursorImage.sprite = cursorSprite[curSprite];
            yield return waitTime;
            curSprite = curSprite == 0 ? 1 : 0;
        }
    }

    void DoChoice() {
        if(GameManager.Instance.gameMode == GameMode.CustomerConstruct)
        {
            EnterCustomerMapEditor();
        }
        else
        {
            AssetsManager.Instance.LoadScene("Game");
        }
    }

    void EnterCustomerMapEditor()
    {
        AssetsManager.Instance.LoadGameObject(ResConst.MapEditor, (obj) =>
        {
            obj.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
            Destroy(transform.root.gameObject);
        });
    }

    void Update()
    {
        ChangeOption();
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
