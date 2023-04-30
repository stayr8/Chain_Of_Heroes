using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class BattleReady_UnitFormationCursor : CursorBase
{
    private RectTransform rt;
    private GameObject currentSelected;
    private void Awake()
    {
        rt = GetComponent<RectTransform>();
    }

    private const float INIT_X = -790f;
    private const float INIT_Y = 375f;
    private void OnEnable()
    {
        Init(rt, INIT_X, INIT_Y, ref currentSelected, "_1");
    }

    public static bool isOnMenuSelect = false;
    private const float MOVE_DISTANCE_X = 313.5f; private const float MOVE_DISTANCE_Y = 125f;
    private const float MAX_POSITION_X = -476.5f; private const float MAX_POSITION_Y = 375f;
    private const float MIN_POSITION_X = -790f; private const float MIN_POSITION_Y = -125f;
    private void Update()
    {
        if(!isOnMenuSelect)
        {
            Movement(rt, ref currentSelected, MOVE_DISTANCE_X, MOVE_DISTANCE_Y, MIN_POSITION_X, MAX_POSITION_X, MIN_POSITION_Y, MAX_POSITION_Y);
        }
        else if(isOnMenuSelect)
        {
            Movement(ref currentSelected);
        }

        MenuFunction();
    }

    private void MenuFunction()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            isOnMenuSelect = true;

            switch (currentSelected.name)
            {
                case "_1":
                    Debug.Log("1번 슬롯 선택");
                    NextButton();

                    break;
                case "_2":
                    Debug.Log("2번 슬롯 선택");
                    NextButton();

                    break;
                case "_3":
                    Debug.Log("3번 슬롯 선택");
                    NextButton();

                    break;
                case "_4":
                    Debug.Log("4번 슬롯 선택");
                    NextButton();

                    break;
                case "_5":
                    Debug.Log("5번 슬롯 선택");
                    NextButton();

                    break;
                case "_6":
                    Debug.Log("6번 슬롯 선택");
                    NextButton();

                    break;
                case "_7":
                    Debug.Log("7번 슬롯 선택");
                    NextButton();

                    break;
                case "_8":
                    Debug.Log("8번 슬롯 선택");
                    NextButton();

                    break;
                case "_9":
                    Debug.Log("9번 슬롯 선택");
                    NextButton();

                    break;
                case "_10":
                    Debug.Log("10번 슬롯 선택");
                    NextButton();

                    break;



                case "_Formation":
                    Formation();

                    break;

                case "_Skill":
                    Skill();

                    break;
            }
        }
    }

    private void NextButton()
    {
        if(isOnMenuSelect)
        {
            BattleReady_UIManager.instance.OnMenuSelected();

            currentSelected = GameObject.Find("_Formation");
            currentSelected.GetComponent<Selectable>().Select();
        }
        else if(!isOnMenuSelect)
        {
            BattleReady_UIManager.instance.OffMenuSelected();

            currentSelected = GameObject.Find("");
            currentSelected.GetComponent<Selectable>().Select();
        }
    }

    private void Formation()
    {
        Debug.Log("편성");
    }

    private void Skill()
    {
        Debug.Log("스킬 확인");
    }
}