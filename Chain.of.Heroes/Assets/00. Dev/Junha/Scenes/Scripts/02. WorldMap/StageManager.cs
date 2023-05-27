using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using System;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

[Serializable]
public class Info
{
    public GameObject Stage;

    public bool isUnlock; // �������� �ر� ���� [false: Close_Chapter], [true: Open_Chapter]
    public bool isClear; // �������� Ŭ���� ���� [false: Open_Chapter], [true: Clear_Chapter]
}

public class StageManager : MonoBehaviour
{
    #region instance ȭ
    public static StageManager instance;
    #endregion

    [Header("����� ������")]
    public int m_id; // [���̵�]
    public int m_chapterNum; // [é�� ��ȣ]
    public string m_chapterName; // [é�� �̸�]
    public string m_resourcePath; // [é�� �̹���]

    [SerializeField, Header("[é�� �̸�] �ؽ�Ʈ")] private TMP_Text Txt_ChapterNum;
    [SerializeField, Header("[é�� ��ǥ] �̹���")] private Image Img_ChapterImage;
    private const int STAGE_LENGTH = 10;

    private string ChapterName; // [JSON ���� �̸�]
    private WorldMap[] _Array;
    private WorldMap firstArray;

    public int num;
    private GameObject _nextChapter;

    public Info[] info;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        ChapterName = "WorldMap";

        var data = Resources.Load<TextAsset>(ChapterName);
        var Root = SimpleJSON.JSON.Parse(data.text);
        _Array = new WorldMap[Root.Count];

        for (int i = 0; i < Root.Count; ++i)
        {
            var node = Root[i];

            var WorldMap = new WorldMap();
            WorldMap.Parse(node);

            _Array[i] = WorldMap;
        }

        // initInfo();
    }

    // private void Start() { }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name.Contains("WorldMapScene"))
        {
            UpdateChapter();

            Controller_Clear();
        }
    }

    private void initInfo()
    {
        firstArray = _Array[num];

        m_id = firstArray.ID;
        m_chapterNum = firstArray.WorldMapChNumber;
        m_chapterName = firstArray.ChapterName;

        m_resourcePath = firstArray.ChapterInfoResourcePath;

        _nextChapter = WorldMap_PlayerController.GetRightChapter();
    }

    private void UpdateChapter()
    {
        initInfo();

        info[0].isUnlock = true;
        for (int i = 0; i < STAGE_LENGTH; ++i)
        {
            SpriteRenderer img = info[i].Stage.GetComponent<SpriteRenderer>();
            if (info[i].isUnlock)
            {
                if (info[i].Stage == _nextChapter) // ���� ���ִ� ���������� ���� ���������� �رݵǾ��ٸ�
                {
                    WorldMap_PlayerController.isCan = true;
                }

                if (info[i].isClear)
                {
                    img.sprite = Resources.Load<Sprite>("Clear_Chapter");
                    info[i + 1].isUnlock = true;
                }
                else // !info[i].isClear
                {
                    img.sprite = Resources.Load<Sprite>("Open_Chapter");
                }
            }
            else // !info[i].isUnlock
            {
                if (info[i].Stage == _nextChapter) // ���� ���ִ� ���������� ���� ���������� �رݵ��� �ʾҴٸ�
                {
                    WorldMap_PlayerController.isCan = false;
                }
                img.sprite = Resources.Load<Sprite>("Close_Chapter");
            }
        }

        if (!PlayerCamera.isFree && !WorldMap_UIManager.instance.isOnParty)
        {
            Txt_ChapterNum = GameObject.Find("_ChapterText").GetComponent<TMP_Text>();
            Txt_ChapterNum.text = m_chapterNum < 10 ? "Chapter. 0" + m_chapterNum + "\n" + m_chapterName :
                                                      "Chapter. " + m_chapterNum + "\n" + m_chapterName;

            Img_ChapterImage = GameObject.Find("_ChapterImage").GetComponent<Image>();
            Img_ChapterImage.sprite = Resources.Load<Sprite>(m_resourcePath);
        }
    }

    #region ġƮŰ ���
    private void Controller_Clear() // ���� Ű�� ���� é�� �ر�
    {
        for (int i = 0; i < 10; ++i)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                if (i == 0 || info[i - 1].isClear)
                {
                    info[i].isClear = true;
                    break;
                }
                else
                {
                    return;
                }
            }
        }
    }
    #endregion
}