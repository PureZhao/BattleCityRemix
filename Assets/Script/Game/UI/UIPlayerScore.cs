using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerScore : MonoBehaviour
{
    public Text[] killCount = new Text[4]; // N Q P H
    public Text[] killScore = new Text[4];
    public Text score;
    public Text totalKill;
    private bool calcCompleted = false;
    public bool CalcCompleted { get => calcCompleted; }

    /// <summary>
    /// GameManager.playerKillCount[] + lastScore + currentScore
    /// </summary>
    /// <param name="data"></param>
    public void StartCalcScore(List<int> data)
    {
        int len = data.Count;
        for (int i = 0; i < 4; i++)
        {
            killCount[i].text = "";
            killScore[i].text = "";
        }
        
        score.text = data[len - 2].ToString();
        totalKill.text = "";
        calcCompleted = false;
        StartCoroutine(nameof(CalcScore), data);
    }
    IEnumerator CalcScore(List<int> data)
    {
        int len = data.Count;
        int roundScore = 0;
        int roundKillCount = 0;
        for (int i = 0; i < 5; i++)
        {
            roundScore += data[i] * DataConst.EachElement[i];
            if (i < 4)
            {
                roundKillCount += data[i];
            }
        }
        yield return new WaitForSeconds(3f);
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j <= data[i]; j++)
            {
                killCount[i].text = j.ToString();
                killScore[i].text = (j * DataConst.EachElement[i]).ToString();
                yield return new WaitForSeconds(0.25f);
            }
            yield return new WaitForSeconds(0.5f);
        }

        yield return new WaitForSeconds(0.5f);
        totalKill.text = roundKillCount.ToString();
        score.text = data[len - 1].ToString();
        yield return new WaitForSeconds(3f);
        calcCompleted = true;
    }
}
