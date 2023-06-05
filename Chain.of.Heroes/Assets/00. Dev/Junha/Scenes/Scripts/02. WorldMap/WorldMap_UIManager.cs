using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
using UnityEngine.UI;

public class WorldMap_UIManager : MonoBehaviour
{
    public static WorldMap_UIManager instance;

    private RectTransform chapterInfoRT;
    private GameObject Obj_Tip;

    [SerializeField] private GameObject _Menu;
    [SerializeField] private GameObject _Party;
    [SerializeField] private GameObject _Save;
    [SerializeField] private GameObject _ChapterInfo;

    [SerializeField] private GameObject _Tip;
    [SerializeField] private GameObject _f1;
    [SerializeField] private Sprite[] _image;
    [SerializeField] private Image _targetImage;
    public int currentIndex = 0;
    [SerializeField] private Image L_btn;
    [SerializeField] private Image R_btn;


    private TMP_Text _ExpInfo;
    private RectTransform _Guide;

    private enum STATE { INGAME, MENU, PARTY, SAVE, TIP }
    private STATE state = STATE.INGAME;

    private bool isMenuState = false;
    private bool isOnParty = false;
    private bool isOnSave = false;
    private bool isOnTip = false;
    public bool GetBool(string _bool)
    {
        if (_bool == "isMenuState")
        {
            return isMenuState;
        }
        else if (_bool == "isOnParty")
        {
            return isOnParty;
        }
        else if (_bool == "isOnSave")
        {
            return isOnSave;
        }
        else if(_bool == "isOnTip")
        {
            return isOnTip;
        }
        else
        {
            return false;
        }
    }

    private void Awake()
    {
        instance = this;

        chapterInfoRT = GameObject.Find("[Image] Info Background").GetComponent<RectTransform>();
        Obj_Tip = GameObject.Find("Main Camera").transform.GetChild(0).gameObject;
        _ExpInfo = GameObject.Find("[Txt] Exp").GetComponent<TMP_Text>();
        _Guide = GameObject.Find("[ Party ]").transform.GetChild(0)
                                             .transform.GetChild(0)
                                             .transform.GetChild(1)
                                             .transform.GetChild(0)
                                             .transform.GetChild(0).GetComponent<RectTransform>();
    }

    private void Start()
    {
        StageManager.instance.isInitStart = false;

        SoundManager.instance.Sound_WorldMapBGM();
    }

    private void Update()
    {
        UI_STATE();
    }

    private void UI_STATE()
    {
        switch (state)
        {
            case STATE.INGAME:
                OnMenu();
                _text.text = "저장";

                if (Input.GetKeyDown(KeyCode.F1))
                {
                    _f1.SetActive(false);
                    OnTip();
                }
                break;

            case STATE.MENU:
                _ExpInfo.text = MapManager.Instance.mapData[MapManager.Instance.stageNum].Clear_Exp.ToString();
                _Guide.anchoredPosition = new Vector2(0f, _Guide.anchoredPosition.y);

                OffMenu();
                break;

            case STATE.PARTY:
                OffParty();
                _text.text = "저장";
                break;

            case STATE.SAVE:
                //OffSave();
                break;

            case STATE.TIP:
                _targetImage.sprite = _image[currentIndex];
                BtnManager();
                
                break;
        }
    }

    #region [메뉴]
    private void OnMenu()
    {
        if (!WorldMap_PlayerController.isMoving && (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Return)))
        {
            SoundManager.instance.Sound_MenuUIOpen();

            isMenuState = true;

            _Menu.SetActive(true);
            chapterInfoRT.anchoredPosition = new Vector2(chapterInfoRT.anchoredPosition.x, 0f);

            Obj_Tip.SetActive(false);

            state = STATE.MENU;
        }
    }
    private void OffMenu()
    {
        if (!WorldMap_Cursor.isOnNextButton && (Input.GetKeyDown(KeyCode.Escape)))
        {
            SoundManager.instance.Sound_MenuUIOpen();

            isMenuState = false;

            _Menu.SetActive(false);
            chapterInfoRT.anchoredPosition = new Vector2(chapterInfoRT.anchoredPosition.x, -315f);

            Obj_Tip.SetActive(true);

            state = STATE.INGAME;
        }
    }
    #endregion

    #region [동료]
    public void OnParty()
    {
        isOnParty = true;

        _Menu.SetActive(false);
        _ChapterInfo.SetActive(false);

        _Party.SetActive(true);
        _Party.GetComponent<WorldMap_Party>().ForceUpdate();

        state = STATE.PARTY;
    }
    private void OffParty()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SoundManager.instance.Sound_SelectMenu();

            isOnParty = false;

            _Party.SetActive(false);

            _Menu.SetActive(true);
            _ChapterInfo.SetActive(true);

            state = STATE.MENU;
        }
    }
    #endregion

    #region [세이브]
    public void OnSave()
    {
        isOnSave = true;

        _Menu.SetActive(false);
        _ChapterInfo.SetActive(false);

        _Save.SetActive(true);

        state = STATE.SAVE;
    }
    private void OffSave()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            SoundManager.instance.Sound_SelectMenu();

            isOnSave = false;

            _Save.SetActive(false);

            _Menu.SetActive(true);
            _ChapterInfo.SetActive(true);

            state = STATE.MENU;
        }
    }

    [SerializeField] private TMP_Text _text;
    public void LieSave()
    {
        _text.text = "저장 완료";
    }
    #endregion

    #region [도움말]
    private void OnTip()
    {
        _Tip.SetActive(true);
        isOnTip = true;

        state = STATE.TIP;
    }
    private void BtnManager()
    {
        if (currentIndex == 0)
        {
            L_btn.gameObject.SetActive(false);
        }
        else
        {
            L_btn.gameObject.SetActive(true);
        }

        if (currentIndex == _image.Length - 1)
        {
            R_btn.gameObject.SetActive(false);
        }
        else
        {
            R_btn.gameObject.SetActive(true);
        }
    }

    public void OffTip()
    {
        _Tip.SetActive(false);
        _f1.SetActive(true);

        isOnTip = false;
        currentIndex = 0;

        state = STATE.INGAME;
    }
    public void ChangeImage(bool isNext)
    {
        if (isNext)
        {
            currentIndex = (currentIndex >= _image.Length - 1) ? currentIndex = _image.Length - 1 : ++currentIndex;
        }
        else
        {
            currentIndex = (currentIndex < 1) ? currentIndex = 0 : --currentIndex;
        }
    }
    #endregion
}