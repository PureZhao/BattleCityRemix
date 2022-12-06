using GameCore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICurtainLevel : MonoBehaviour
{
    public Text curLevelText;

    private IEnumerator Start()
    {
        yield return new WaitUntil(() => UIController.Instance != null);
        UIController.Instance.OnUpdateUI += OnLevelChange;
    }

    private void OnDestroy()
    {
        UIController.Instance.OnUpdateUI -= OnLevelChange;
    }

    void OnLevelChange(params object[] parameters)
    {
        UIUpdateType type = (UIUpdateType)parameters[0];
        if (type == UIUpdateType.LevelChange)
        {
            string t = parameters[1].ToString();
            if (t.Length < 2)
            {
                t = "0" + t;
            }
            curLevelText.text = "Stage " + t;
        }
        if(type == UIUpdateType.OnRoundStart)
        {
            curLevelText.text = "";
        }
        
    }
}
