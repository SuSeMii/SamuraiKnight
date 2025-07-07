using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueDirector : MonoBehaviour
{
    public float testTime = 3f;
    public float waitTime = 0f;

    // Update is called once per frame
    void Update()
    {
        waitTime += Time.deltaTime;
        if(waitTime > testTime)
        {
            GameManager.Instance.LoadStageSelectScene();
        }
    }
}
