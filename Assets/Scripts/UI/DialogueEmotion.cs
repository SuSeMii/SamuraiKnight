using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueEmotion : MonoBehaviour
{
    public GameObject[] emotions;

    public void ShowEmotion(string _emotionName)
    {
        foreach (GameObject emotion in emotions)
        {
            if (emotion.name == _emotionName)
            {
                emotion.SetActive(true);
            }
        }
    }

    public void ShowEmotion(int _emotionIndex)
    {
        if (_emotionIndex >= emotions.Length) return;
        emotions[_emotionIndex].SetActive(true);
    }

    public void HideEmotion()
    {
        foreach (GameObject emotion in emotions)
        {
            emotion.SetActive(false);
        }
    }
}
