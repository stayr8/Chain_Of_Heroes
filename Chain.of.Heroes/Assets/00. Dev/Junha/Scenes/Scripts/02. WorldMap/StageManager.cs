using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using System;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

[Serializable]
public class Info
{
    public GameObject Stage;

    public bool isUnlock; // �������� �ر� ���� [false: Close_Chapter], [true: Open_Chapter]
    public bool isClear; // �������� Ŭ���� ���� [false: Open_Chapter], [true: Clear_Chapter]
}

public class StageManager : MonoBehaviour
{
    #region instanceȭ
    /*
    public static StageManager instance;
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Initialize()
    {
        if (instance == null)
        {
            GameObject Entity = new GameObject("StageManager");

            instance = Entity.AddComponent<StageManager>();

            //Entity.AddComponent<AudioSource>();

            DontDestroyOnLoad(Entity.gameObject);
        }
    }
    */

    /*
    public static StageManager Instance { get; private set; }
    private void ins()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    */

    public static StageManager instance;
    #endregion

    private string ChapterName; // [JSON ���� �̸�]
    private WorldMap[] _Array;
    public Info[] info;
    private const int STAGE_LENGTH = 10;
    private void Awake()
    {
        //ins();
        instance = this;

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

        //info = new Info[STAGE_LENGTH];
        //for (int i = 0; i < STAGE_LENGTH; ++i)
        //{
        //    info[i].Stage = GameObject.Find("_" + (1 + i));
        //}

        initInfo();
    }

    private void Update()
    {
        UpdateChapter();

        Controller_Clear();
    }

    [Header("����� ������")]
    public int m_id; // [���̵�]
    public int m_chapterNum; // [é�� ��ȣ]
    public string m_chapterName; // [é�� �̸�]

    public string m_resourcePath; // [é�� �̹���]

    private WorldMap firstArray;
    public int num;
    private GameObject _nextChapter;

    private void initInfo()
    {
        firstArray = _Array[num];

        m_id = firstArray.ID;
        m_chapterNum = firstArray.WorldMapChNumber;
        m_chapterName = firstArray.ChapterName;

        m_resourcePath = firstArray.ChapterInfoResourcePath;

        _nextChapter = WorldMap_PlayerController.GetRightChapter();
    }

    [SerializeField, Header("[é�� �̸�] �ؽ�Ʈ")] private TMP_Text Txt_ChapterNum;
    [SerializeField, Header("[é�� ��ǥ] �̹���")] private Image Img_ChapterImage;
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

        Txt_ChapterNum = GameObject.Find("_ChapterText").GetComponent<TMP_Text>();
        Txt_ChapterNum.text = m_chapterName;

        Img_ChapterImage = GameObject.Find("_ChapterImage").GetComponent<Image>();
        Img_ChapterImage.sprite = Resources.Load<Sprite>(m_resourcePath);
    }

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
}