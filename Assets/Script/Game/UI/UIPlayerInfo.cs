using GameCore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerInfo : MonoBehaviour
{
    public Text lifeText;
    int playerID;
    void Awake()
    {
        if(transform.name.ToLower() == "player1")
        {
            playerID = 1;
        }
        else
        {
            playerID = 2;
        }
    }

    private IEnumerator Start()
    {
        yield return new WaitUntil(() => UIController.Instance != null);
        UIController.Instance.OnUpdateUI += OnPlayerLifeChange;
    }

    private void OnDestroy()
    {
        UIController.Instance.OnUpdateUI -= OnPlayerLifeChange;
    }
    void OnPlayerLifeChange(params object[] parameters)
    {
        UIUpdateType type = (UIUpdateType)parameters[0];
        if (type != UIUpdateType.PlayerLifeChange) return;
        int id = (int)parameters[1];
        if(id == playerID)
        {
            lifeText.text = parameters[2].ToString();
        }
    }
}
