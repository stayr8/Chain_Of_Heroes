using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Initialize()
    {
        if (instance == null)
        {
            GameObject Entity = new GameObject("StageManager");

            instance = Entity.AddComponent<StageManager>();

            DontDestroyOnLoad(Entity.gameObject);
        }
    }
    #endregion

    private const int STAGE_LENGTH = 10;
    public Info[] info;

    [Header("����� ������")]
    public int m_id; // [���̵�]
    public int m_chapterNum; // [é�� ��ȣ]
    public string m_chapterName; // [é�� �̸�]
    public string m_resourcePath; // [é�� �̹���]

    private string ChapterName = "WorldMap"; // [JSON ���� �̸�]
    private WorldMap[] _Array;
    private WorldMap firstArray;

    [SerializeField, Header("[é�� �̸�] �ؽ�Ʈ")] public TMP_Text Txt_ChapterNum;
    [SerializeField, Header("[é�� ��ǥ] �̹���")] public Image Img_ChapterImage;

    public int num = 0;
    private GameObject _nextChapter;

    public bool isInitStart = false;

    public int ClearID { get; set; } = -1;
    public int TotalUnlock = 0;

    private void Awake()
    {
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

        initInfo();
    }

    private void Update()
    {
        InitData();
        if (ClearID == -1)
        {
            TotalUnlock = 2;
        }
        else if (ClearID > 6)
        {
            TotalUnlock = 8;
        }
        else
        {
            TotalUnlock = ClearID + 2;
        }

        if (SceneManager.GetActiveScene().name.Contains("WorldMapScene"))
        {
            UpdateChapter();

            StageCheat();
        }
    }

    private void InitData()
    {
        if (!isInitStart)
        {
            if (info == null)
            {
                info = new Info[STAGE_LENGTH];

                for (int i = 0; i < STAGE_LENGTH; i++)
                {
                    info[i] = new Info();
                }
            }
            for (int i = 0; i < STAGE_LENGTH; i++)
            {
                if (info[i].Stage == null)
                {
                    GameObject stageObj = GameObject.Find("_" + (i + 1));
                    if (stageObj != null)
                    {
                        info[i].Stage = stageObj;
                    }
                }
            }

            if (Txt_ChapterNum == null)
            {
                GameObject chapterTextObj = GameObject.Find("_ChapterText");

                if (chapterTextObj != null)
                {
                    Txt_ChapterNum = chapterTextObj.GetComponent<TMP_Text>();
                }
            }

            if (Img_ChapterImage == null)
            {
                GameObject chapterImageObj = GameObject.Find("_ChapterImage");

                if (chapterImageObj != null)
                {
                    Img_ChapterImage = chapterImageObj.GetComponent<Image>();
                }
            }

            isInitStart = true;
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
                    if(i != 9)
                    {
                        info[i + 1].isUnlock = true;
                    }
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

        if (!PlayerCamera.isFree && !WorldMap_UIManager.instance.GetBool("isOnParty"))
        {
            Txt_ChapterNum = GameObject.Find("_ChapterText").GetComponent<TMP_Text>();
            Txt_ChapterNum.text = m_chapterNum < 10 ? "Chapter. 0" + m_chapterNum + "\n" + m_chapterName :
                                                      "Chapter. " + m_chapterNum + "\n" + m_chapterName;

            Img_ChapterImage = GameObject.Find("_ChapterImage").GetComponent<Image>();
            Img_ChapterImage.sprite = Resources.Load<Sprite>(m_resourcePath);
        }
    }

    public void StageClear(int _stageNumber)
    {
        if (_stageNumber == 0 || info[_stageNumber - 1].isClear)
        {
            info[_stageNumber].isClear = true;
            ClearID = _stageNumber + 1;
        }
    }

    #region ġƮŰ ���
    private void StageCheat()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            StageClear(0);
        }
        else if (info[0].isClear && Input.GetKeyDown(KeyCode.Alpha2))
        {
            StageClear(1);
        }
        else if (info[1].isUnlock && Input.GetKeyDown(KeyCode.Alpha3))
        {
            StageClear(2);
        }
        else if (info[2].isClear && Input.GetKeyDown(KeyCode.Alpha4))
        {
            StageClear(3);
        }
        else if (info[3].isClear && Input.GetKeyDown(KeyCode.Alpha5))
        {
            StageClear(4);
        }
        else if (info[4].isClear && Input.GetKeyDown(KeyCode.Alpha6))
        {
            StageClear(5);
        }
        else if (info[5].isClear && Input.GetKeyDown(KeyCode.Alpha7))
        {
            StageClear(6);
        }
        else if (info[6].isClear && Input.GetKeyDown(KeyCode.Alpha8))
        {
            StageClear(7);
        }
        else if (info[7].isClear && Input.GetKeyDown(KeyCode.Alpha9))
        {
            StageClear(8);
        }
    }
    #endregion
}