using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
using UnityEngine.SceneManagement;

public class BattleReady_Cursor : CursorBase
{
    private RectTransform rt;
    public static GameObject currentSelected;

    private const float INIT_X = -860f;
    private const float INIT_Y = 200f;

    private bool isBattleStart = false;
    private bool isBack = false;
    private const float MOVE_DISTANCE = 100f;
    private const float MAX_POSITION_X = -860f; private const float MAX_POSITION_Y = 200f;
    private const float MIN_POSITION_X = -890f; private const float MIN_POSITION_Y = -100f;

    private void Awake()
    {
        rt = GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        Init(rt, INIT_X, INIT_Y, ref currentSelected, "_BattleStart");
    }


    private void Update()
    {
        if (!isBattleStart || !isBack)
        {
            Movement(rt, ref currentSelected, MOVE_DISTANCE, MIN_POSITION_X, MAX_POSITION_X, MIN_POSITION_Y, MAX_POSITION_Y);
        }

        MenuFunction();
    }

    private void LateUpdate()
    {
        rt.Rotate(150f * Time.deltaTime, 0f, 0f);
    }

    [SerializeField, Header("nextButton")] private GameObject nextButton;
    [SerializeField, Header("nextButton Text")] private TextMeshProUGUI text_nextButton;
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

                    text_nextButton.text = "전투를 진행하시겠습니까?";

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

                    text_nextButton.text = "월드 맵으로 돌아가시겠습니까?";

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
                    ResetCursorPosition();
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

    private void ResetCursorPosition()
    {
        if (currentSelected.name == "_BattleStart")
        {
            rt.anchoredPosition = new Vector2(-860f, MAX_POSITION_Y);
        }
        else if (currentSelected.name == "_Back")
        {
            rt.anchoredPosition = new Vector2(-890f, MIN_POSITION_Y);
        }
    }

    public static GameObject GetCurrentSelected()
    {
        return currentSelected;
    }
}