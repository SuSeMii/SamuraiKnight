using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageDirector : MonoBehaviour
{
    public void OnClickStageButton(int stageNumber)
    {
        GameManager.Instance.SceneOutSquare();
        GameManager.Instance.LoadStagePlayScene(stageNumber);
    }
}
