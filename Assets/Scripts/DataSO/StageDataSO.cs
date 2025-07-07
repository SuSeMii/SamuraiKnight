using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "data_", menuName = "StageDataSO")]
public class StageDataSO : ScriptableObject
{
    public List<StageData> stages = new List<StageData>();
}

[Serializable]
public class StageData
{
    public String title;
    public AudioClip music;
    public float defaultSpawnTime = 2f;
    public float musicPlayLatencyTime;
    public float noteArriveTime;
    public List<NoteInfo> noteInfos = new List<NoteInfo>();

}

[Serializable]
public class NoteInfo
{
    public int line;
    public int type;
    public float hitTime;
    public float addTime;
}