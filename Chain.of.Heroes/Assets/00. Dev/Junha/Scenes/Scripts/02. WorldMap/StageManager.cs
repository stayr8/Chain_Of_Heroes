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

    public bool isUnlock; // 스테이지 해금 여부 [false: Close_Chapter], [true: Open_Chapter]
    public bool isClear; // 스테이지 클리어 여부 [false: Open_Chapter], [true: Clear_Chapter]
}

public class StageManager : MonoBehaviour
{
    #region instance 화
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

    [Header("월드맵 데이터")]
    public int m_id; // [아이디]
    public int m_chapterNum; // [챕터 번호]
    public string m_chapterName; // [챕터 이름]
    public string m_resourcePath; // [챕터 이미지]

    private string ChapterName = "WorldMap"; // [JSON 파일 이름]
    private WorldMap[] _Array;
    private WorldMap firstArray;

    [SerializeField, Header("[챕터 이름] 텍스트")] public TMP_Text Txt_ChapterNum;
    [SerializeField, Header("[챕터 대표] 이미지")] public Image Img_ChapterImage;

    public int num;
    private GameObject _nextChapter;

    public int ClearID { get; set; } = -1;

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

        // initInfo();
    }

    // private void Start() { }

    public bool isInitStart = false;
    private void Update()
    {
        InitData();

        if (SceneManager.GetActiveScene().name.Contains("WorldMapScene"))
        {
            UpdateChapter();

            Controller_Clear();
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
                if (info[i].Stage == _nextChapter) // 현재 서있는 스테이지의 다음 스테이지가 해금되었다면
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
                if (info[i].Stage == _nextChapter) // 현재 서있는 스테이지의 다음 스테이지가 해금되지 않았다면
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

    #region 치트키 요소
    private void Controller_Clear() // 숫자 키를 통한 챕터 해금
    {
        for (int i = 0; i < 10; ++i)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                if (i == 0 || info[i - 1].isClear)
                {
                    info[i].isClear = true;
                    ClearID = i + 1;
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