using GameCore;
using Sirenix.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIEnemyCounter : MonoBehaviour
{

    List<GameObject> icons = new List<GameObject>();

    private IEnumerator Start()
    {
        yield return new WaitUntil(() => UIController.Instance != null);
        UIController.Instance.OnUpdateUI += OnEnemyCountChange;
    }

    private void OnDestroy()
    {
        UIController.Instance.OnUpdateUI -= OnEnemyCountChange;
    }

    void OnEnemyCountChange(params object[] parameters)
    {
        UIUpdateType type = (UIUpdateType)parameters[0];
        if (type == UIUpdateType.EnemyInit)
        {
            int count = (int)parameters[1];
            for (int i = 0; i < count; i++)
            {
                AssetsManager.Instance.LoadGameObject(ResConst.UIEnemyIcon, (obj) =>
                {
                    obj.transform.parent = transform;
                    icons.Add(obj);
                });
            }
        }
        else if (type == UIUpdateType.EnemySpawn)
        {
            if (icons.Count > 0)
            {
                GameObject g = icons.Back();
                icons.RemoveAt(icons.Count - 1);
                AssetsManager.Instance.FreeObject(g);
            }
        }
        else
        {
            return;
        }

    }
}
