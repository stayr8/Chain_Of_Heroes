using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using UnityEngine.UI;
using TMPro;

public class InGame_UIManager : MonoBehaviour
{
    #region instance화 :: Awake()함수 포함
    public static InGame_UIManager instance;
    private void Awake()
    {
        instance = this;

        Set_ChapterNumName();
    }
    #endregion

    [SerializeField] private GameObject _Panel;
    [SerializeField] private GameObject _Menu;
    [SerializeField] private GameObject _PartyInfo;

    private enum STATE { INGAME, MENU, PARTY_INFO }
    private STATE state = STATE.INGAME;

    [SerializeField, Header("[챕터 장] 텍스트")] private TMP_Text Txt_chapterNum;
    [SerializeField, Header("[챕터명] 텍스트")] private TMP_Text Txt_chapterName;

    public event EventHandler OnCharacterInstance;

    [SerializeField] private GameObject _fallUI;

    [SerializeField] private TMP_Text actionPointsText;
    [SerializeField] private TMP_Text turnPointsText;

    [SerializeField] private Image speed_Image;

    [SerializeField] private LayerMask UILayerMask;

    private List<Binding> Binds = new List<Binding>();

    private bool isinGameFall;
    private bool isgamestop;
    private float worldSpeed;

    private void Start()
    {
        Binding Bind = BindingManager.Bind(TurnSystem.Property, "IsPlayerTurn", (object value) =>
        {
            UpdateActionPoints();
        });
        Binds.Add(Bind);

        worldSpeed = 1;
    }

    private void Update()
    {
        if (isinGameFall)
        {
            if (InputManager.Instance.IsMouseButtonDown())
            {
                StartCoroutine(LoadScene());
            }
        }

        UI_STATE();
    }

    IEnumerator LoadScene()
    {
        yield return new WaitForSeconds(1f);
        LoadingSceneController.LoadScene("WorldMapScene");
    }

    private void UI_STATE()
    {
        switch (state)
        {
            case STATE.INGAME: // 인게임 상태에서 ESC키 입력
                OnMenu();
                break;

            case STATE.MENU: // 메뉴 상태에서 ESC키 입력
                UpdateActionPoints();

                OffMenu();
                break;

            case STATE.PARTY_INFO: // 아군 정보 상태에서 ESC키 입력
                OffPartyInfo();
                break;
        }
    }

    #region [메뉴 선택]
    private void OnMenu()
    {
        if (ScenesSystem.Instance.GetIsInGame() && Input.GetKeyDown(KeyCode.Escape))
        {
            SoundManager.instance.Sound_MenuUIOpen();

            OnCharacterInstance?.Invoke(this, EventArgs.Empty);

            _Panel.SetActive(true);
            _Menu.SetActive(true);

            OnGameStop();

            state = STATE.MENU;
        }
    }
    private void OffMenu()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SoundManager.instance.Sound_MenuUIOpen();
            
            _Menu.SetActive(false);
            _Panel.SetActive(false);

            OnGameStop();

            state = STATE.INGAME;
        }
    }
    #endregion

    #region [아군 정보]
    public void OnPartyInfo()
    {
        _PartyInfo.SetActive(true);
        _Menu.SetActive(false);

        state = STATE.PARTY_INFO;
    }
    private void OffPartyInfo()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _PartyInfo.SetActive(false);
            _Menu.SetActive(true);

            state = STATE.MENU;
        }
    }
    #endregion

    #region [턴 종료]
    public void OnTurnfo()
    {
        Debug.Log("턴 종료에는 들어오고");
        if (!TurnSystem.Property.IsTurnEnd && (TurnSystem.Property.IsPlayerTurn && (TurnSystem.Property.ActionPoints > 0)))
        {
            Debug.Log("여기까지는 문제 없다.");
            if (!AttackActionSystem.Instance.GetIsChainAtk_1() && !AttackActionSystem.Instance.GetIsChainAtk_2())
            {
                Debug.Log("턴 교체");
                TurnSystem.Property.ActionPoints = 0;
                TurnSystem.Property.IsPlayerTurn = false;
            }
        }

        _Menu.SetActive(false);
        _Panel.SetActive(false);

        OnGameStop();

        state = STATE.INGAME;
    }
    #endregion

    #region [포기]
    public void Onfallfo()
    {
        isinGameFall = true;
        TurnSystem.Property.IsTurnEnd = true;

        _Menu.SetActive(false);
        _Panel.SetActive(false);
        _fallUI.SetActive(true);
        
        OnGameStop();

        state = STATE.INGAME;
    }
    #endregion



    private void Set_ChapterNumName()
    {
        Txt_chapterNum.text = "제 " + StageManager.instance.m_chapterNum.ToString() + "장";
        Txt_chapterName.text = StageManager.instance.m_chapterName.ToString();
    }

    public bool GetIsinGameFall()
    {
        return isinGameFall;
    }

    private void UpdateActionPoints()
    {
        if (TurnSystem.Property.IsPlayerTurn)
        {
            actionPointsText.text = "" + TurnSystem.Property.ActionPoints;
            turnPointsText.text = "" + TurnSystem.Property.TurnNumber;
        }
        else
        {
            actionPointsText.text = "0";
            turnPointsText.text = "" + TurnSystem.Property.TurnNumber;
        }
    }

    private void OnGameStop()
    {
        if (!isgamestop)
        {
            Time.timeScale = 0f;
            isgamestop = true;
        }
        else
        {
            Time.timeScale = worldSpeed;
            isgamestop = false;
        }
    }

    public void OnDoubleSpeed()
    {
        if(worldSpeed <= 1)
        {
            speed_Image.sprite = Resources.Load<Sprite>("Speed_15");
            worldSpeed = 1.5f;
            Time.timeScale = worldSpeed;
        }
        else if (worldSpeed <= 1.5f)
        {
            speed_Image.sprite = Resources.Load<Sprite>("Speed_20");
            worldSpeed = 2f;
            Time.timeScale = worldSpeed;
        }
        else
        {
            speed_Image.sprite = Resources.Load<Sprite>("Speed_10");
            worldSpeed = 1f;
            Time.timeScale = worldSpeed;
        }
    }

    private void OnDisable()
    {
        foreach (var bind in Binds)
        {
            BindingManager.Unbind(TurnSystem.Property, bind);
        }
    }
}