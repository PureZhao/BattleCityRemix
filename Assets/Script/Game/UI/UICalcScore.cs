using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICalcScore : MonoBehaviour
{
    public Text curStageText;
    public Text highestScoreText;
    public List<UIPlayerScore> playerScorePanels;
    private bool calcCompleted = false;
    public bool CalcCompleted { get => calcCompleted; }

    public void CalcRoundData(List<object> parameters)
    {
        int curStage = (int)parameters[2];
        calcCompleted = false;
        curStageText.text = curStage < 10 ? "0" + curStage.ToString() : curStage.ToString();
        gameObject.SetActive(true);
        StartCoroutine(nameof(CalcData), parameters);
    }

    IEnumerator CalcData(List<object> parameters)
    {

        playerScorePanels[0].StartCalcScore(parameters[0] as List<int>);
        playerScorePanels[1].StartCalcScore(parameters[1] as List<int>);
        yield return new WaitUntil(()=> playerScorePanels[0].CalcCompleted && playerScorePanels[1].CalcCompleted);
        calcCompleted = true;
    }
}
