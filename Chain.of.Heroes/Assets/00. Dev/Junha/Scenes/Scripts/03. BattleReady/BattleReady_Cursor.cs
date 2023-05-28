using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
using UnityEngine.SceneManagement;

public class BattleReady_Cursor : CursorBase
{
    private RectTransform rt;

    private GameObject nextButton;
    private TMP_Text Txt_nextButton;

    //private bool isInitStart = false;

    private const float INIT_X = -860f;
    private const float INIT_Y = 200f;
    private GameObject currentSelected;

    private const float MOVE_DISTANCE = 100f;
    private const float MAX_POSITION_X = -860f; private const float MAX_POSITION_Y = 200f;
    private const float MIN_POSITION_X = -890f; private const float MIN_POSITION_Y = -100f;

    private bool isBattleStart = false;
    private bool isBack = false;

    private void Awake()
    {
        rt = GetComponent<RectTransform>();

        nextButton = GameObject.Find("[ Next Button ]").transform.GetChild(0).gameObject;
        Txt_nextButton = nextButton.transform.GetChild(1).
                                    transform.GetChild(0).GetComponent<TMP_Text>();

        //isInitStart = false;
    }

    private void Start()
    {
        Init(rt, INIT_X, INIT_Y, ref currentSelected, "_BattleStart");
    }

    private void Update()
    {
        MenuFunction();

        if (!isBattleStart && !isBack)
        {
            Movement(rt, ref currentSelected, MOVE_DISTANCE, MIN_POSITION_X, MAX_POSITION_X, MIN_POSITION_Y, MAX_POSITION_Y);
        }
        else if (isBattleStart || isBack)
        {
            Movement(ref currentSelected);
        }
    }

    private void LateUpdate()
    {
        rt.Rotate(150f * Time.deltaTime, 0f, 0f);
    }

    private void MenuFunction()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SoundManager.instance.Sound_SelectMenu();

            switch (currentSelected.name)
            {
                case "_BattleStart":
                    isBattleStart = true;
                    OnNextButton(nextButton, isBattleStart, gameObject, ref currentSelected, "_Yes");

                    Txt_nextButton.text = "전투를 진행하시겠습니까?";
                    break;

                case "_UnitFormation":
                    BattleReady_UIManager.instance.OnUnitFormation();
                    break;

                case "_ChangeFormation":
                    BattleReady_UIManager.instance.OnChangeFormation();
                    break;

                case "_Back":
                    isBack = true;
                    OnNextButton(nextButton, isBack, gameObject, ref currentSelected, "_Yes");

                    Txt_nextButton.text = "월드 맵으로 돌아가시겠습니까?";
                    break;

                case "_Yes":
                    if (isBattleStart)
                    {
                        ScenesSystem.Instance.ScenesChange();
                        UnitManager.Instance.SpawnAllPlayer();
                        GridSystemVisual.Instance.HideAllGridPosition();
                        StageUI.Instance.ConditionShow();
                        Invoke("Hide", 2f);
                    }
                    else if (isBack)
                    {
                        SceneManager.LoadScene("WorldMapScene");
                    }
                    break;

                case "_No":
                    if (isBattleStart)
                    {
                        isBattleStart = false;
                        OnNextButton(nextButton, isBattleStart, gameObject, ref currentSelected, "_BattleStart");
                    }
                    else if (isBack)
                    {
                        isBack = false;
                        OnNextButton(nextButton, isBack, gameObject, ref currentSelected, "_Back");
                    }
                    break;
            }
        }
    }

    void Show()
    {
        StageUI.Instance.ConditionShow();
    }

    void Hide()
    {
        StageUI.Instance.ConditionHide();
    }
}