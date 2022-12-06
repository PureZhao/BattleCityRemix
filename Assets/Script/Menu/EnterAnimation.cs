using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class EnterAnimation : MonoBehaviour
{
    private GameObject cursor;
    private GameObject menu;
    void Awake()
    {
        menu = transform.Find("Option").gameObject;
        cursor = menu.transform.Find("Cursor").gameObject;

        menu.GetComponent<RectTransform>().DOLocalMoveY(0f, 5f).Play().OnComplete(() => cursor.SetActive(true));
    }

}
