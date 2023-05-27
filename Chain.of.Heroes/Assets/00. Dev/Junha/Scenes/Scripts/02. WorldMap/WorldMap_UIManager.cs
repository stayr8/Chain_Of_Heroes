using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldMap_UIManager : MonoBehaviour
{
    #region instanceȭ :: Awake()�Լ� ����
    public static WorldMap_UIManager instance;
    private void Awake()
    {
        instance = this;
    }
    #endregion

    [SerializeField] private GameObject _Menu;
    [SerializeField] private GameObject _Party;
    [SerializeField] private GameObject _ChapterInfo;
    [SerializeField] private GameObject ChapterInfo_Background;
    private RectTransform rt_ChapterInfo;

    private enum STATE { INGAME, MENU, PARTY, SAVE }
    private STATE state = STATE.INGAME;

    public bool isMenuState = false;
    public bool isOnParty = false;

    [SerializeField, Header("[���� ī�޶� ��] ���ӿ�����Ʈ")] private GameObject tip;

    private void OnEnable()
    {
        StageManager.instance.isInitStart = false;
    }

    private void Start()
    {
        rt_ChapterInfo = ChapterInfo_Background.GetComponent<RectTransform>();
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

                break;

            case STATE.MENU:

                OffMenu();

                break;

            case STATE.PARTY:

                OffParty();

                break;

            case STATE.SAVE:

                break;
        }
    }

    #region [�޴�]
    private void OnMenu() // InGame ������ �� ESC Ű�� Enter Ű �Է� ��
    {
        if(!WorldMap_PlayerController.isMoving && (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Return)))
        {
            SoundManager.instance.Sound_WorldMapUIOpen();

            isMenuState = true;

            _Menu.SetActive(true);
            rt_ChapterInfo.anchoredPosition = new Vector2(rt_ChapterInfo.anchoredPosition.x, 0f);

            tip.SetActive(false);

            state = STATE.MENU;
        }
    }
    private void OffMenu() // Menu ������ �� ESC Ű �Է� ��
    {
        if (!WorldMap_Cursor.isOnNextButton && (Input.GetKeyDown(KeyCode.Escape)))
        {
            SoundManager.instance.Sound_WorldMapUIOpen();

            isMenuState = false;
            WorldMap_Cursor.isInitStart = false;

            _Menu.SetActive(false);
            rt_ChapterInfo.anchoredPosition = new Vector2(rt_ChapterInfo.anchoredPosition.x, -315f);

            tip.SetActive(true);

            state = STATE.INGAME;
        }
    }
    #endregion

    #region [����]
    public void OnParty() // Menu ������ �� ���� ��ư Ŭ�� ��
    {
        SoundManager.instance.Sound_SelectMenu();

        isOnParty = true;

        _Menu.SetActive(false);
        _ChapterInfo.SetActive(false);

        _Party.SetActive(true);

        state = STATE.PARTY;
    }
    private void OffParty() // Party ������ �� ESC Ű �Է� ��
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
}