using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace GameCore
{
    public class Boot : MonoBehaviour
    {
        private IEnumerator Start()
        {
            yield return new WaitUntil(() => AssetsManager.Instance != null);
            AssetsManager.Instance.LoadGameObject(ResConst.UIMenuCanvas, (obj) =>
            {
                RectTransform menuRect = obj.transform.Find("Menu").GetComponent<RectTransform>();
                GameObject cursor = menuRect.Find("Cursor").gameObject;
                menuRect.sizeDelta = new Vector2(0, -626f);
                menuRect.DOLocalMoveY(0f, 5f).Play().OnComplete(() => cursor.SetActive(true));
            });


            AssetsManager.Instance.LoadGameObject(ResConst.Player1, (obj) =>
            {
                PlayerControl control = obj.GetComponent<PlayerControl>();
                control.SetAsPlayer1();
                obj.transform.position = new Vector3(200, 200, 0);
                obj.SetActive(false);
            });
            AssetsManager.Instance.LoadGameObject(ResConst.Player2, (obj) =>
            {
                PlayerControl control = obj.GetComponent<PlayerControl>();
                control.SetAsPlayer2();
                obj.transform.position = new Vector3(200, 200, 0);
                obj.SetActive(false);
            });
        }
    }
}