using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

[Serializable]
public class Info
{
    public GameObject Stage;

    public bool isUnlock; // 스테이지 해금 여부 [false: Close_Chapter], [true: Open_Chapter]
    public bool isClear; // 스테이지 클리어 여부 [false: Open_Chapter], [true: Clear_Chapter]
}

public class StageManager : MonoBehaviour
{
    public Info[] info;

    [Header("어떤 Json 파일을 불러올 것인가?")] public string ChapterName;
    [Header("월드맵 데이터")]
    public int m_id; // [아이디]
    public int m_chapterNum; // [챕터 번호]
    public string m_chapterName; // [챕터 이름]

    public string m_resourcePath; // [챕터 이미지]



    private WorldMap[] _Array;
    private WorldMap firstArray;

    private void Awake()
    {
        if (ChapterName == "") { return; }

        var data = Resources.Load<TextAsset>(ChapterName);
        var Root = SimpleJSON.JSON.Parse(data.text);
        _Array = new WorldMap[Root.Count];

        for(int i = 0; i < Root.Count; ++i)
        {
            var node = Root[i];

            var WorldMap = new WorldMap();
            WorldMap.Parse(node);

            _Array[i] = WorldMap;
        }

        initInfo();
    }

    private void initInfo()
    {
        firstArray = _Array[0];

        m_id = firstArray.ID;
        m_chapterNum = firstArray.WorldMapChNumber;
        m_chapterName = firstArray.ChapterName;

        m_resourcePath = firstArray.ChapterInfoResourcePath;
    }

    private void Update()
    {
        Controller_Stage();
    }

    private void Controller_Stage()
    {
        for (int i = 0; i < info.Length; ++i)
        {
            SpriteRenderer img = info[i].Stage.GetComponent<SpriteRenderer>();

            if (info[i].isUnlock)
            {
                img.sprite = info[i].isClear ? Resources.Load<Sprite>("Clear_Chapter") : Resources.Load<Sprite>("Open_Chapter");
            }
            else // !info[i].isUnlock
            {
                img.sprite = Resources.Load<Sprite>("Close_Chapter");
            }
        }
    }
}
