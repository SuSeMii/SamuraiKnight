using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteSceneManager : MonoBehaviour
{
    public float duration;

    private float time;
    private bool isSceneOutable = true;

    // Start is called before the first frame update
    void Start()
    {
        time = 0;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if((time >= duration || Input.GetKeyDown(KeyCode.Escape)) && isSceneOutable)
        {
            isSceneOutable = false;
            GameManager.Instance.SceneOutSquare();
            StartCoroutine(GameManager.Instance.timeFunction(GameManager.Instance.maskEffectDuration, GameManager.Instance.LoadNextStory));
        }
    }
}
